using MediatR;

namespace LinaSys.Auth.Application.Queries;

public record RoleHasAccessToProtectedResourceQuery(List<string> RoleIds, long ProtectedResourceId) : IRequest<bool>;
