using LinaSys.Notification.Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LinaSys.Notification.Infrastructure.Workers;

public class EmailSenderWorker(EmailQueueService emailQueueService, EmailSenderService emailSenderService, ILogger<EmailSenderWorker> logger) : BackgroundService
{
    private static readonly SemaphoreSlim _signal = new(0);

    public static void NotifyNewEmail()
    {
        _signal.Release(); // Wake up the worker when an email is queued
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Email sender worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await _signal.WaitAsync(stoppingToken); // Wait until an email is queued

            while (emailQueueService.TryDequeue(out var email))
            {
                try
                {
                    await emailSenderService.SendEmailAsync(email.To, email.Subject, email.Body);
                    logger.LogInformation("Email sent to {Recipient}", email.To);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send email to {Recipient}", email.To);
                }
            }
        }

        logger.LogInformation("Email sender worker ended.");
    }
}
