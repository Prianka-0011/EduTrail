using System.Net;
using System.Net.Mail;
namespace EduTrail.Application.Shared
{

    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your@email.com", "app-password"),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress("your@email.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            await smtpClient.SendMailAsync(mail);
        }
    }
}
