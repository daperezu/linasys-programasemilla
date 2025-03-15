using LinaSys.Auth.Application.Queries;
using LinaSys.Auth.Domain.Repositories;
using MediatR;

namespace LinaSys.Auth.Application.Handlers;

public class UserHasAccessToProtectedResourceQueryHandler(IUserProtectedResourcePermissionRepository userProtectedResourcePermissionRepository)
    : IRequestHandler<UserHasAccessToProtectedResourceQuery, bool>
{
    public async Task<bool> Handle(UserHasAccessToProtectedResourceQuery request, CancellationToken cancellationToken)
    {
        return await userProtectedResourcePermissionRepository.UserHasAccessAsync(request.UserId, request.InternalProtectedResourceId, cancellationToken);
    }
}
