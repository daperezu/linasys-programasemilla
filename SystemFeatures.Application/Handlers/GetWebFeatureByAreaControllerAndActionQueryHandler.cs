using LinaSys.SystemFeatures.Application.Queries;
using LinaSys.SystemFeatures.Domain.AggregatesModel.WebFeatureAggregate;
using MediatR;

namespace LinaSys.SystemFeatures.Application.Handlers;

public class GetWebFeatureByAreaControllerAndActionQueryHandler(IWebFeatureRepository webFeatureRepository)
    : IRequestHandler<GetWebFeatureByAreaControllerAndActionQuery, WebFeatureDto?>
{
    public async Task<WebFeatureDto?> Handle(GetWebFeatureByAreaControllerAndActionQuery request, CancellationToken cancellationToken)
    {
        var feature = await webFeatureRepository.GetByAreaControllerAndActionAsync(request.Area, request.Controller, request.Action, cancellationToken);
        if (feature is null)
        {
            return null;
        }

        return new WebFeatureDto
        {
            ExternalId = feature.ExternalId,
            Name = feature.Name,
            IsPublic = feature.IsPublic,
        };
    }
}
