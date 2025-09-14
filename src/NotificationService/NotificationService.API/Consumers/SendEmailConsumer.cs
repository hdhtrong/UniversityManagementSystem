using MassTransit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using NotificationService.API.Config;
using SharedKernel.Messages;

namespace NotificationService.API.Consumers;

public class SendEmailConsumer : IConsumer<SendEmailMessage>
{
    private readonly EmailSettings _settings;

    public SendEmailConsumer(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task Consume(ConsumeContext<SendEmailMessage> context)
    {
        var msg = context.Message;

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_settings.SenderName, _settings.Sender));
        email.To.Add(MailboxAddress.Parse(msg.To));
        email.Subject = msg.Subject;
        email.Body = new TextPart("plain") { Text = msg.Body };

        using var smtp = new SmtpClient();

        var secureOption = SecureSocketOptions.None;
        if (_settings.UseSSL) secureOption = SecureSocketOptions.SslOnConnect;
        else if (_settings.UseStartTls) secureOption = SecureSocketOptions.StartTls;

        await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, secureOption);
        await smtp.AuthenticateAsync(_settings.User, _settings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
