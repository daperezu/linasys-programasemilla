using LinaSys.Auth.Infrastructure.Persistence;
using LinaSys.Shared;
using LinaSys.SystemFeatures.Infrastructure.Persistence;

namespace LinaSys.Web.Infrastructure.Persistence;

public class DbContextFactory(IServiceProvider serviceProvider) : IDbContextFactory
{
    private readonly Dictionary<string, Type> _mapping = new()
    {
        { "Auth", typeof(AuthDbContext) },
        { "SystemFeatures", typeof(SystemFeaturesDbContext) },
    };

    public IDbContext GetDbContextForRequest<TRequest>()
    {
        var requestNs = typeof(TRequest).Namespace!;
        var dbContextType = _mapping.FirstOrDefault(x => requestNs.Contains(x.Key)).Value;
        return (IDbContext)serviceProvider.GetRequiredService(dbContextType);
    }
}
