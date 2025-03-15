namespace LinaSys.SystemFeatures.Domain.AggregatesModel.WebFeatureAggregate;

public interface IWebFeatureRepository
{
    Task<WebFeature?> GetByAreaControllerAndActionAsync(string area, string controller, string action, CancellationToken cancellationToken);
}
