using LinaSys.UserProfile.Application.Extensions;
using LinaSys.Web.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.Web.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IDbContextFactory dbContextFactory, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            var dbContext = dbContextFactory.GetDbContextForRequest<TRequest>();

            if (dbContext.HasActiveTransaction)
            {
                return await next();
            }

            var strategy = dbContext.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.TryBeginTransactionAsync(cancellationToken);

                using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                {
                    logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await dbContext.CommitTransactionAsync(transaction, cancellationToken);
                }

                //// await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
            });

            return response!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}
