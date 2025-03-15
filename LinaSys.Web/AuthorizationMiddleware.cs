using System.Security.Claims;
using LinaSys.Auth.Application.Queries;
using LinaSys.SystemFeatures.Application.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using OpenTelemetry.Trace;

namespace LinaSys.Web;

public class AuthorizationMiddleware(
    RequestDelegate next,
    IServiceScopeFactory scopeFactory,
    IMemoryCache cache,
    ILogger<AuthorizationMiddleware> logger)
{
    private static readonly HashSet<string> _staticFileExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".ico", ".woff", ".woff2", ".ttf", ".otf", ".eot", ".mp4", ".webm", ".json", ".txt", ".xml",
    };

    public async Task Invoke(HttpContext context)
    {
        using var scope = scopeFactory.CreateScope();
        var tracer = scope.ServiceProvider.GetRequiredService<Tracer>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        using var span = tracer.StartActiveSpan(nameof(AuthorizationMiddleware));

        if (IsStaticFileRequest(context))
        {
            await next(context);
            return;
        }

        var requestPath = context.Request.Path.Value?.ToLower();
        var routeData = context.GetRouteData();
        var area = routeData.Values["area"]?.ToString() ?? string.Empty;
        var controller = routeData.Values["controller"]?.ToString() ?? "Home";
        var action = routeData.Values["action"]?.ToString() ?? "Index";
        var protectedResourceInWebFeature = routeData.Values["id"]?.ToString();

        ArgumentException.ThrowIfNullOrWhiteSpace(requestPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(controller);
        ArgumentException.ThrowIfNullOrWhiteSpace(action);

        var webFeatureInternalId = await DemandValidWebFeature(area, controller, action, requestPath, mediator, span);
        switch (webFeatureInternalId)
        {
            case 0: // Public Access
                await next(context);
                return;
            case -1: // Failed Authorization or other internal settings
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
        }

        var authenticatedUser = await DemandValidAuthenticatedUserAsync(context.User, requestPath, mediator, span);
        if (!authenticatedUser.HasValue)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var userId = authenticatedUser.Value.UserId;
        var userRoles = authenticatedUser.Value.Roles;

        var hasAccessToWebFeature = await DemandUserHasAccessToProtectedResource(userId, userRoles, webFeatureInternalId, requestPath, mediator, span);

        if (!hasAccessToWebFeature)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var hasAccessToProtectedResource = await DemandUserHasAccessToProtectedResourceInWebFeatureIfNecessary(userId, userRoles, protectedResourceInWebFeature, requestPath, mediator, span);
        if (hasAccessToProtectedResource == -1)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        //// span.SetAttribute("authorization.status", "Access Granted");

        await next(context);
    }

    private static bool IsStaticFileRequest(HttpContext context)
    {
        var path = context.Request.Path;

        return path.HasValue && _staticFileExtensions.Contains(Path.GetExtension(path.Value));
    }

    private async Task<bool> DemandUserHasAccessToProtectedResource(
        string userId,
        IReadOnlyList<string> roles,
        long protectedResourceInternalId,
        string requestPath,
        IMediator mediator,
        TelemetrySpan span)
    {
        var hasRoleAccess = await mediator.Send(new RoleHasAccessToProtectedResourceQuery(roles.ToList(), protectedResourceInternalId));
        if (!hasRoleAccess)
        {
            var hasUserAccess = await mediator.Send(new UserHasAccessToProtectedResourceQuery(userId, protectedResourceInternalId));
            if (!hasUserAccess)
            {
                logger.LogWarning("Forbidden Access Attempt: {RequestPath} - User {UserId} - ProtectedResource {ProtectedResource} - None Role or User denial", requestPath, protectedResourceInternalId, userId);
                span.SetAttribute("authorization.status", "Forbidden - None Role Or User Denial");
                return false;
            }
        }

        return true;
    }

    private async Task<int> DemandUserHasAccessToProtectedResourceInWebFeatureIfNecessary(
        string userId,
        IReadOnlyList<string> roles,
        string? protectedResourceExternalId,
        string requestPath,
        IMediator mediator,
        TelemetrySpan span)
    {
        if (!string.IsNullOrEmpty(protectedResourceExternalId))
        {
            if (Guid.TryParse(protectedResourceExternalId, out var protectedResourceExternalIdAsGuid))
            {
                var protectedResource = await cache.GetOrCreateAsync($"{nameof(GetProtectedResourceByExternalIdQuery)}_{protectedResourceExternalId}", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    return await mediator.Send(new GetProtectedResourceByExternalIdQuery(protectedResourceExternalIdAsGuid));
                });

                if (protectedResource is null)
                {
                    logger.LogWarning("Access Attempt: {RequestPath} - Protected Resource {ProtectedResource} - Id not registered in ProtectedResources", requestPath, protectedResourceExternalId);
                    span.SetAttribute("authorization.status", "Forbidden - ProtectedResources Denial");
                    return -1; // Protected Resource not registered
                }

                var hasAccessToProtectedResource = await DemandUserHasAccessToProtectedResource(userId, roles, protectedResource.InternalId, requestPath, mediator, span);
                return hasAccessToProtectedResource ? 1 : -1;
            }
            else
            {
                logger.LogInformation("Access Attempt: {RequestPath} - User {UserId} - Invalid External Protected Resource {ProtectedResourceExternalId} ", requestPath, userId, protectedResourceExternalId);
                span.SetAttribute("authorization.status", "Forbidden - ProtectedResources Denial");
                return -1; // Invalid External Protected Resource
            }
        }

        return 0; // No protected resource in the request
    }

    private async Task<(string UserId, IReadOnlyList<string> Roles)?> DemandValidAuthenticatedUserAsync(
        ClaimsPrincipal user,
        string requestPath,
        IMediator mediator,
        TelemetrySpan span)
    {
        if (user.Identity is null)
        {
            logger.LogWarning("Invalid identity: {RequestPath}", requestPath);
            span.SetAttribute("authorization.status", "Not valid identity");
            return null;
        }

        if (!user.Identity.IsAuthenticated)
        {
            logger.LogWarning("Unauthorized Access Attempt: {RequestPath}", requestPath);
            span.SetAttribute("authorization.status", "Unauthorized");
            return null;
        }

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("Unauthorized Access Attempt: {RequestPath} - User ID missing", requestPath);
            span.SetAttribute("authorization.status", "Unauthorized - Missing User ID");
            return null;
        }

        var userRoles = await cache.GetOrCreateAsync($"{nameof(GetUserRolesQuery)}_{userId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await mediator.Send(new GetUserRolesQuery(userId));
        });

        if (userRoles is null || userRoles.Count == 0)
        {
            logger.LogWarning("Unauthorized Access Attempt: {RequestPath} - User doesn't have roles assigned", requestPath);
            span.SetAttribute("authorization.status", "Unauthorized - No roles assigned");
            return null;
        }

        return (userId, userRoles);
    }

    private async Task<long> DemandValidWebFeature(
        string area,
        string controller,
        string action,
        string requestPath,
        IMediator mediator,
        TelemetrySpan span)
    {
        var webFeature = await cache.GetOrCreateAsync($"{nameof(GetWebFeatureByAreaControllerAndActionQuery)}_{action}_{controller}_{action}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await mediator.Send(new GetWebFeatureByAreaControllerAndActionQuery(area, controller, action));
        });

        if (webFeature is null)
        {
            logger.LogWarning("Unauthorized Access Attempt: {RequestPath}", requestPath);
            span.SetAttribute("authorization.status", "Feature not found in our DB. It might exists in the MVC but is not registered yet in the SystemFeatures module.");
            return -1;
        }

        if (webFeature.IsPublic)
        {
            logger.LogInformation("Public Access: {RequestPath}", requestPath);
            span.SetAttribute("authorization.status", "Public Access");
            return 0;
        }

        var protectedWebFeature = await cache.GetOrCreateAsync($"{nameof(GetProtectedResourceByExternalIdQuery)}_{webFeature.ExternalId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await mediator.Send(new GetProtectedResourceByExternalIdQuery(webFeature.ExternalId));
        });

        if (protectedWebFeature is null)
        {
            logger.LogWarning("Access Attempt: {RequestPath} - WebFeature {WebFeatureId} - WebFeature not registered in ProtectedResources", requestPath, webFeature.ExternalId);
            span.SetAttribute("authorization.status", "Forbidden - ProtectedResources Denial");
            return -1;
        }

        return protectedWebFeature.InternalId;
    }
}
