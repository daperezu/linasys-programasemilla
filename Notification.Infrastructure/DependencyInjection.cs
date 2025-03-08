using LinaSys.Notification.Application.Interfaces;
using LinaSys.Notification.Infrastructure.Services;
using LinaSys.Notification.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace LinaSys.Notification.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<EmailQueueService>();
        services.AddSingleton<IEmailQueueService>(sp => sp.GetRequiredService<EmailQueueService>()); // Ensure shared instance

        services.AddSingleton<EmailSenderService>(); // Handles actual email sending
        services.AddHostedService<EmailSenderWorker>(); // Background worker for processing queued emails
        return services;
    }
}
