using LinaSys.Auth.Application;
using LinaSys.Auth.Infrastructure;
using LinaSys.Notification.Application;
using LinaSys.Notification.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//// Aspire extension
builder.AddServiceDefaults();

//// Notification Domain
builder.Services.AddNotificationInfrastructure();
builder.Services.AddNotificationApplication();
//// Auth Domain
builder.AddAuthInfrastructure();
builder.Services.AddAuthApplication();

//// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
