using MediatR;

namespace LinaSys.SystemFeatures.Application.Queries;

public record GetWebFeatureByAreaControllerAndActionQuery(string Area, string Controller, string Action) : IRequest<WebFeatureDto?>;
