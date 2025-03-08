namespace LinaSys.Notification.Domain;

public class Email
{
    public required string To { get; set; }

    public required string Subject { get; set; }

    public required string Body { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
