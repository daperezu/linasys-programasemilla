using LinaSys.Auth.Application.Queries;
using LinaSys.Auth.Domain.Repositories;
using MediatR;

namespace LinaSys.Auth.Application.Handlers;

public class RoleHasAccessToProtectedResourceQueryHandler(IRolePermissionRepository rolePermissionRepository)
    : IRequestHandler<RoleHasAccessToProtectedResourceQuery, bool>
{
    public async Task<bool> Handle(RoleHasAccessToProtectedResourceQuery request, CancellationToken cancellationToken)
    {
        return await rolePermissionRepository.RoleHasAccessAsync(request.RoleIds, request.ProtectedResourceId, cancellationToken);
    }
}
