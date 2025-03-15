using LinaSys.UserProfile.Application.Extensions;
using MediatR;

namespace LinaSys.Web.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);
        var response = await next();
        logger.LogInformation("Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), response);

        return response;
    }
}
