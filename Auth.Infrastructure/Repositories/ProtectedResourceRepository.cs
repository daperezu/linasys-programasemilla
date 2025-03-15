using LinaSys.Auth.Domain.Entities;
using LinaSys.Auth.Domain.Repositories;
using LinaSys.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.Auth.Infrastructure.Repositories;

public class ProtectedResourceRepository(AuthDbContext dbContext) : IProtectedResourceRepository
{
    public async Task<ProtectedResource?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await dbContext.ProtectedResources
            .Where(e => e.ExternalId == externalId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
