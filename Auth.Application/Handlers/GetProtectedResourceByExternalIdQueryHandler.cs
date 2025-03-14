using LinaSys.Auth.Application.Queries;
using LinaSys.Auth.Domain.Repositories;
using MediatR;

namespace LinaSys.Auth.Application.Handlers;

public class GetProtectedResourceByExternalIdQueryHandler(IProtectedResourceRepository protectedResourceRepository)
    : IRequestHandler<GetProtectedResourceByExternalIdQuery, ProtectedResourceDto?>
{
    public async Task<ProtectedResourceDto?> Handle(GetProtectedResourceByExternalIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await protectedResourceRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return entity is null
            ? null
            : new ProtectedResourceDto
            {
                InternalId = entity.Id,
                ExternalId = entity.ExternalId,
                ResourceType = entity.ResourceType,
                Name = entity.Name,
            };
    }
}
