namespace LinaSys.Auth.Domain.Repositories;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken);
}
