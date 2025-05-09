using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Loushop.services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailSender : IEmailSender
    {
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string emailFrom;
        private readonly string emailPassword;

        public EmailSender(IConfiguration configuration)
        {
            smtpHost = configuration["EmailSettings:SmtpHost"];
            smtpPort = int.TryParse(configuration["EmailSettings:SmtpPort"], out var port) ? port : 587;
            emailFrom = configuration["EmailSettings:EmailFrom"];
            emailPassword = configuration["EmailSettings:EmailPassword"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(emailFrom, emailPassword);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(emailFrom),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

            }
            catch (SmtpException ex)
            {
                throw new Exception($"خطا در ارسال ایمیل: {ex.Message} - کد خطا: {ex.StatusCode}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطا در ارسال ایمیل: {ex.Message}", ex);
            }
        }
    }
}
