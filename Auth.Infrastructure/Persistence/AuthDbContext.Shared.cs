using System.Data;
using LinaSys.Auth.Domain.SeedWork;
using LinaSys.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LinaSys.Auth.Infrastructure.Persistence;

public partial class AuthDbContext : IUnitOfWork, IDbContext
{
    private IDbContextTransaction? _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<IDbContextTransaction> TryBeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _currentTransaction ??= await Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted, cancellationToken: cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction != _currentTransaction)
        {
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

        try
        {
            await SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Database.CreateExecutionStrategy();
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this);
        _ = await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }
}
