using LinaSys.Auth.Domain.Repositories;
using LinaSys.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.Auth.Infrastructure.Repositories;

public class RolePermissionRepository(AuthDbContext dbContext) : IRolePermissionRepository
{
    public async Task<bool> RoleHasAccessAsync(List<string> roleIds, long entityId, CancellationToken cancellationToken)
    {
        return await dbContext.RoleProtectedResourcePermissions
            .AnyAsync(rp => roleIds.Contains(rp.RoleId) && rp.ProtectedResourceId == entityId, cancellationToken);
    }
}
