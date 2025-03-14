using LinaSys.Auth.Domain.Entities;

namespace LinaSys.Auth.Domain.Repositories;

public interface IProtectedResourceRepository
{
    Task<ProtectedResource?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken);
}
