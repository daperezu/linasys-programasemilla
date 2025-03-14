using LinaSys.SystemFeatures.Domain.Entities;

namespace LinaSys.SystemFeatures.Domain.Repositories;

public interface IWebFeatureRepository
{
    Task<WebFeature?> GetByAreaControllerAndActionAsync(string area, string controller, string action, CancellationToken cancellationToken);
}
