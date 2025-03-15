using LinaSys.SystemFeatures.Domain.AggregatesModel.WebFeatureAggregate;
using LinaSys.SystemFeatures.Infrastructure.Persistence;
using LinaSys.SystemFeatures.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinaSys.SystemFeatures.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddSystemFeaturesInfrastructure(this IHostApplicationBuilder builder, string connectionName = "DefaultConnection")
    {
        var connectionString = builder.Configuration.GetConnectionString(connectionName) ?? throw new InvalidOperationException($"Connection string '{connectionName}' not found.");

        builder.Services.AddDbContext<SystemFeaturesDbContext>(opts =>
        {
            opts.UseSqlServer(connectionString);

            //// opts.EnableThreadSafetyChecks(false);
            opts.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            opts.EnableSensitiveDataLogging();
            opts.EnableDetailedErrors();
        });

        //// Aspire extension
        builder.EnrichSqlServerDbContext<SystemFeaturesDbContext>(settings =>
        {
            settings.CommandTimeout = 30;
        });

        builder.Services.AddScoped<IWebFeatureRepository, WebFeatureRepository>();
        return builder;
    }
}
