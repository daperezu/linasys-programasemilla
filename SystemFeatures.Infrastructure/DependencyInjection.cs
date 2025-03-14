using LinaSys.SystemFeatures.Domain.Repositories;
using LinaSys.SystemFeatures.Infrastructure.Persistence;
using LinaSys.SystemFeatures.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinaSys.SystemFeatures.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddSystemFeaturesInfrastructure(this IHostApplicationBuilder builder, string connectionName = "DefaultConnection")
    {
        //// Aspire extension
        builder.AddSqlServerDbContext<SystemFeaturesDbContext>(
            connectionName,
            configureDbContextOptions: opts =>
            {
                //// opts.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                //// opts.EnableThreadSafetyChecks(false);
                opts.EnableSensitiveDataLogging();
                opts.EnableDetailedErrors();
            });

        builder.Services.AddScoped<IWebFeatureRepository, WebFeatureRepository>();
        return builder;
    }
}
