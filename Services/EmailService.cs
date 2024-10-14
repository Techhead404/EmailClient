using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.Json;

namespace EmailClient.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _logFilePath;

        public EmailService(string smtpHost, int smtpPort, string smtpUsername, string smtpPassword, string logFilePath)
        {
            _smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };
            _logFilePath = logFilePath;
        }

        public async Task SendEmail(string recipient, string subject, string body)
        {
            var mailMessage = new MailMessage("techhead404@gmail.com", recipient, subject, body);
            int retryCount = 0;
            bool emailSent = false;

            while (retryCount < 3 && !emailSent)
            {
                try
                {
                    await _smtpClient.SendMailAsync(mailMessage);
                    LogEmailAttempt(recipient, subject, body, "Success");
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    LogEmailAttempt(recipient, subject, body, $"Failed: {ex.Message}");
                    if (retryCount == 3)
                    {
                        throw new Exception($"Failed to send email after 3 attempts: {ex.Message}");
                    }
                }
            }
        }

        private void LogEmailAttempt(string recipient, string subject, string body, string status)
        {
            var logEntry = new
            {
                Recipient = recipient,
                Subject = subject,
                Body = body,
                Status = status,
                Timestamp = DateTime.Now
            };
            var logJson = JsonSerializer.Serialize(logEntry);
            File.AppendAllText(_logFilePath, logJson + Environment.NewLine);
        }
    }

}
