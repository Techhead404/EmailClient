using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;

namespace EmailClient.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _logFilePath;

        public EmailService(string Host, int Port, string LogFilePath, string UserName, string Password)
        {
            _smtpClient = new SmtpClient(Host, Port)
            {
                //Host = Host,
                //Port = Port,
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = true, // Ensure SSL is enabled
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 20000 // Set timeout (optional)
            };
            _logFilePath = LogFilePath;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body, string userName)
        {
            int retryCount = 0;
            bool success = false;

            var mailMessage = new MailMessage
            {
                From = new MailAddress("testmain404@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(recipient);

            while (retryCount < 3 && !success)
            {
                try
                {
                    await _smtpClient.SendMailAsync(mailMessage);
                    LogEmailAttempt(recipient, subject, "Success");
                    success = true;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Debug.WriteLine($"Attempt {retryCount}: Failed to send email - {ex.Message}");
                    LogEmailAttempt(recipient, subject, $"Failed: {ex.Message}");
                    if (retryCount == 3)
                    {
                        throw new Exception($"Failed to send email after 3 attempts: {ex.Message}");
                    }
                }
            }

            
        }

        private void LogEmailAttempt(string recipient, string subject, string status)
        {
            var logEntry = new
            {
                Recipient = recipient,
                Subject = subject,
                Status = status,
                Timestamp = DateTime.UtcNow
            };
            var logJson = JsonSerializer.Serialize(logEntry);
            File.AppendAllText(_logFilePath, logJson + Environment.NewLine);
        }
    }

}
