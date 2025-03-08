using LinaSys.Aspire.AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var linaDbConnectionString = builder.AddConnectionString("DefaultConnection");
var appSettingsJson = AppsettingsLoader.SerializeUserJsonConfiguration();

builder.AddProject<LinaSys_Web>("lina-web")
    .WithReference(linaDbConnectionString)
    .WithEnvironment("AspireAppsettings", appSettingsJson)
    .WithExternalHttpEndpoints();

builder.Build().Run();
