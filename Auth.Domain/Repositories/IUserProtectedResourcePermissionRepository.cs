namespace LinaSys.Auth.Domain.Repositories;

public interface IUserProtectedResourcePermissionRepository
{
    Task<bool> UserHasAccessAsync(string userId, long protectedResourceId, CancellationToken cancellationToken);
}
