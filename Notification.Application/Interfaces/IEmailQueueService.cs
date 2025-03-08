namespace LinaSys.Notification.Application.Interfaces;

public interface IEmailQueueService
{
    void QueueEmail(string to, string subject, string body);
}
