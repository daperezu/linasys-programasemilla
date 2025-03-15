using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LinaSys.SystemFeatures.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddSystemFeaturesApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        return services;
    }
}
