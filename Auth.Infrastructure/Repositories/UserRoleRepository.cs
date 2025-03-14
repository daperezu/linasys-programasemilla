using LinaSys.Auth.Domain.Entities;
using LinaSys.Auth.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LinaSys.Auth.Infrastructure.Repositories;

public class UserRoleRepository(UserManager<User> userManager) : IUserRoleRepository
{
    public async Task<IReadOnlyList<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user == null ? [] : (IReadOnlyList<string>)await userManager.GetRolesAsync(user);
    }
}
