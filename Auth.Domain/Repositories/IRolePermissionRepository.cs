namespace LinaSys.Auth.Domain.Repositories;

public interface IRolePermissionRepository
{
    Task<bool> RoleHasAccessAsync(List<string> roleIds, long entityId, CancellationToken cancellationToken);
}
