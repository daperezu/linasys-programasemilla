using LinaSys.SystemFeatures.Domain.AggregatesModel.WebFeatureAggregate;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.SystemFeatures.Infrastructure.Persistence.Repositories;

public class WebFeatureRepository(SystemFeaturesDbContext dbContext)
    : AbstractRepository<WebFeature>(dbContext), IWebFeatureRepository
{
    private readonly SystemFeaturesDbContext _dbContext = dbContext;

    public Task<WebFeature?> GetByAreaControllerAndActionAsync(string area, string controller, string action, CancellationToken cancellationToken)
    {
        return _dbContext.WebFeatures
            .Where(f => f.Area == area && f.Controller == controller && f.Action == action)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
