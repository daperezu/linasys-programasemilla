using MediatR;

namespace LinaSys.Auth.Application.Queries;

public record GetUserRolesQuery(string UserId) : IRequest<IReadOnlyList<string>>;
