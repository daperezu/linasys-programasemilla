using MediatR;

namespace LinaSys.Auth.Application.Queries;

public record GetProtectedResourceByExternalIdQuery(Guid ExternalId) : IRequest<ProtectedResourceDto?>;
