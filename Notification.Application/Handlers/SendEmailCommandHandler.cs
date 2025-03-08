using LinaSys.Notification.Application.Commands;
using LinaSys.Notification.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LinaSys.Notification.Application.Handlers;

public class SendEmailCommandHandler(IEmailQueueService emailSender, ILogger<SendEmailCommandHandler> logger) : IRequestHandler<SendEmailCommand>
{
    public Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Queuing email to {Recipient}", request.To);
        emailSender.QueueEmail(request.To, request.Subject, request.Body);
        return Task.CompletedTask;
    }
}
