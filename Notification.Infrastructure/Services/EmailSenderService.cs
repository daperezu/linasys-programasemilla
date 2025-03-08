using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace LinaSys.Notification.Infrastructure.Services;

public class EmailSenderService(IConfiguration configuration, ILogger<EmailSenderService> logger)
{
    private const string PasswordKey = "EmailSender:Password";
    private const string SmtpPortKey = "EmailSender:SmtpPort";
    private const string SmtpServerKey = "EmailSender:SmtpServer";
    private const string UsernameKey = "EmailSender:Username";
    private const string FromNameKey = "EmailSender:FromName";
    private const string FromAddressKey = "EmailSender:FromAddress";

    private readonly Lazy<MailtrapConfiguration> _mailtrapConfiguration = new(
        () => new MailtrapConfiguration(
            configuration[SmtpServerKey] ?? throw new ArgumentNullException(SmtpServerKey),
            int.Parse(configuration[SmtpPortKey] ?? "587"),
            configuration[UsernameKey] ?? throw new ArgumentNullException(UsernameKey),
            configuration[PasswordKey] ?? throw new ArgumentNullException(PasswordKey),
            configuration[FromNameKey] ?? throw new ArgumentNullException(FromNameKey),
            configuration[FromAddressKey] ?? throw new ArgumentNullException(FromAddressKey)),
        LazyThreadSafetyMode.PublicationOnly);

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_mailtrapConfiguration.Value.FromName, _mailtrapConfiguration.Value.FromAddress));
        message.To.Add(new MailboxAddress(string.Empty, to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_mailtrapConfiguration.Value.SmtpServer, _mailtrapConfiguration.Value.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_mailtrapConfiguration.Value.Username, _mailtrapConfiguration.Value.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        logger.LogInformation("Email sent to {Recipient}", to);
    }

    private record MailtrapConfiguration(string SmtpServer, int SmtpPort, string Username, string Password, string FromName, string FromAddress);
}
