using LinaSys.Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinaSys.Auth.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddAuthInfrastructure(this WebApplicationBuilder builder, string connectionName = "DefaultConnection")
    {
        //// Aspire extension
        builder.AddSqlServerDbContext<AuthDbContext>(
            connectionName,
            configureDbContextOptions: opts =>
            {
                //// opts.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                //// opts.EnableThreadSafetyChecks(false);
                opts.EnableSensitiveDataLogging();
                opts.EnableDetailedErrors();
            });

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<Domain.Entities.User>(
            opts =>
            {
                opts.SignIn.RequireConfirmedAccount = true;
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "0123456789";
                })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();

        return builder;
    }
}
