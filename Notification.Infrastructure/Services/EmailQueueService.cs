using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using LinaSys.Notification.Application.Interfaces;
using LinaSys.Notification.Domain;
using LinaSys.Notification.Infrastructure.Workers;
using Microsoft.Extensions.Logging;

namespace LinaSys.Notification.Infrastructure.Services;

public class EmailQueueService(ILogger<EmailQueueService> logger) : IEmailQueueService
{
    private readonly ConcurrentQueue<Email> _emailQueue = new();

    public void QueueEmail(string to, string subject, string body)
    {
        var email = new Email
        {
            To = to,
            Subject = subject,
            Body = body,
        };

        _emailQueue.Enqueue(email);
        logger.LogInformation("Email queued for {Recipient}", to);

        EmailSenderWorker.NotifyNewEmail(); // Immediately notify the worker
    }

    public bool TryDequeue([NotNullWhen(true)] out Email? email)
    {
        return _emailQueue.TryDequeue(out email);
    }
}
