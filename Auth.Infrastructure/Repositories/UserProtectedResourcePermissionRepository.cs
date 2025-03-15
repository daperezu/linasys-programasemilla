using LinaSys.Auth.Domain.Repositories;
using LinaSys.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.Auth.Infrastructure.Repositories;

public class UserProtectedResourcePermissionRepository(AuthDbContext dbContext) : IUserProtectedResourcePermissionRepository
{
    public async Task<bool> UserHasAccessAsync(string userId, long protectedResourceId, CancellationToken cancellationToken)
    {
        return await dbContext.UserProtectedResourcePermissions
            .AnyAsync(ep => ep.UserId == userId && ep.ProtectedResourceId == protectedResourceId, cancellationToken);
    }
}
