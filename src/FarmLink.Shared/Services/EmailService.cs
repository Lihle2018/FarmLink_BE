
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace FarmLink.Shared.Services
{
    public interface IEmailService
    {
        Task SendEmail(string recipientEmail, string subject, string body);

    }
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string recipientEmail, string subject, string body)
        {
            var userName = _configuration.GetSection("SmtpUserName").Value;
            // Create a new MailMessage object
            MailMessage message = new MailMessage(userName, recipientEmail, subject, body) { Priority = MailPriority.High, ReplyTo = new MailAddress("student24donotreply@gmail.com") };
            // Set the server and credentials for sending the email
            var password = _configuration.GetSection("Password").Value;
            var smtp = new SmtpClient
            {
                Host = _configuration.GetSection("Host").Value,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName, password)
            };
            smtp.SendAsync(message, null);
        }
    }
}
