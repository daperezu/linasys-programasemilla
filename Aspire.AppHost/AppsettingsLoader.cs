using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace LinaSys.Aspire.AppHost;

public static class AppsettingsLoader
{
    public static IConfiguration LoadJsonConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Ensure correct path resolution
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Base config
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true); // Environment override

        return builder.Build();
    }

    public static string SerializeUserJsonConfiguration()
    {
        var appSettingsConfig = LoadJsonConfiguration().AsEnumerable()
            .Where(kv => kv.Value != null
                         && !kv.Key.StartsWith("ConnectionStrings")
                         && !kv.Key.StartsWith("Logging"))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        return JsonSerializer.Serialize(appSettingsConfig);
    }
}
