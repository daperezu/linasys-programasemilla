using LinaSys.SystemFeatures.Domain.Entities;
using LinaSys.SystemFeatures.Domain.Repositories;
using LinaSys.SystemFeatures.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.SystemFeatures.Infrastructure.Repositories;

public class WebFeatureRepository(SystemFeaturesDbContext dbContext) : IWebFeatureRepository
{
    public Task<WebFeature?> GetByAreaControllerAndActionAsync(string area, string controller, string action, CancellationToken cancellationToken)
    {
        return dbContext.WebFeatures
            .Where(f => f.Area == area && f.Controller == controller && f.Action == action)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
