using LinaSys.Auth.Application.Queries;
using LinaSys.Auth.Domain.Repositories;
using MediatR;

namespace LinaSys.Auth.Application.Handlers;

public class GetUserRolesQueryHandler(IUserRoleRepository userRoleRepository)
    : IRequestHandler<GetUserRolesQuery, IReadOnlyList<string>>
{
    public async Task<IReadOnlyList<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await userRoleRepository.GetUserRolesAsync(request.UserId, cancellationToken);
    }
}
