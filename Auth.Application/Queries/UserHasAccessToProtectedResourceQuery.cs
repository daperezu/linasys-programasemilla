using MediatR;

namespace LinaSys.Auth.Application.Queries;

public record UserHasAccessToProtectedResourceQuery(string UserId, long InternalProtectedResourceId) : IRequest<bool>;
