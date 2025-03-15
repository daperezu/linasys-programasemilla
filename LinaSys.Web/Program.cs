using System.Reflection;
using LinaSys.Auth.Application;
using LinaSys.Auth.Infrastructure;
using LinaSys.Notification.Application;
using LinaSys.Notification.Infrastructure;
using LinaSys.SystemFeatures.Application;
using LinaSys.SystemFeatures.Infrastructure;
using LinaSys.Web;
using LinaSys.Web.Application.Behaviors;
using LinaSys.Web.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

//// Aspire extension
builder.AddServiceDefaults();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

//// Notification Domain
builder.Services.AddNotificationInfrastructure();
builder.Services.AddNotificationApplication();
//// Auth Domain
builder.AddAuthInfrastructure();
builder.Services.AddAuthApplication();
//// SystemFeatures Domain
builder.AddSystemFeaturesInfrastructure();
builder.Services.AddSystemFeaturesApplication();

builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();

//// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuthorizationMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.MapDefaultEndpoints();

await app.RunAsync();
