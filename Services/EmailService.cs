using MailKit.Net.Smtp;
using MimeKit;

namespace FinancialAppMvc.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var emailMimeMessage = new MimeMessage();
            emailMimeMessage.From.Add(new MailboxAddress("FinancialAppMvc", "hello@example.com"));
            emailMimeMessage.To.Add(new MailboxAddress("Email", email));
            emailMimeMessage.Subject = subject;
            emailMimeMessage.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.mailtrap.io", 2525, false);
            await client.AuthenticateAsync("7741aad668610c", "f8bcd62aefc99b");
            await client.SendAsync(emailMimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}