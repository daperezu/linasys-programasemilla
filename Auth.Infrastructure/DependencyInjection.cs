using LinaSys.Auth.Domain.Repositories;
using LinaSys.Auth.Infrastructure.Persistence;
using LinaSys.Auth.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinaSys.Auth.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddAuthInfrastructure(this IHostApplicationBuilder builder, string connectionName = "DefaultConnection")
    {
        //// Aspire extension
        builder.AddSqlServerDbContext<AuthDbContext>(
            connectionName,
            configureDbContextOptions: opts =>
            {
                //// opts.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                //// opts.EnableThreadSafetyChecks(false);
                opts.EnableSensitiveDataLogging();
                opts.EnableDetailedErrors();
            });

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<Domain.Entities.User>(
            opts =>
            {
                opts.SignIn.RequireConfirmedAccount = true;
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "0123456789";
                })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();

        builder.Services.AddScoped<IProtectedResourceRepository, ProtectedResourceRepository>();
        builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        builder.Services.AddScoped<IUserProtectedResourcePermissionRepository, UserProtectedResourcePermissionRepository>();
        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        return builder;
    }
}
