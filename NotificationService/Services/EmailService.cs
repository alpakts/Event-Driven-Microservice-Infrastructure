using MailKit.Net.Smtp;
using MimeKit;
using NotificationService.Settings;

namespace NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Alpion", _settings.SmtpSender));
            message.To.Add(new MailboxAddress("", toEmail));  

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, false);
                }
                catch (Exception)
                {

                    throw;
                }
                await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
