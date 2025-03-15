using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace LinaSys.Shared;

public interface IDbContext
{
    DatabaseFacade Database { get; }

    bool HasActiveTransaction { get; }

    Task<IDbContextTransaction> TryBeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);

    IExecutionStrategy CreateExecutionStrategy();
}
