using MediatR;

namespace LinaSys.Notification.Application.Commands;

public record SendEmailCommand(string To, string Subject, string Body) : IRequest;
