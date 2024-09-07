using CommVill.Controllers;
using CommVill.DAL.Helper;
using CommVill.DAL.Interface;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;

namespace CommVill.DAL.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ILogger _logger;
        private readonly CommVillDBContext _context;
        private readonly NVelocityHelper _nVelocityHelper;
        public EmailRepository(ILogger<EmailRepository> logger,
                               CommVillDBContext context)
        {
            _logger = logger;
            _context = context;
            _nVelocityHelper = new NVelocityHelper();

        }

        public async Task SendEmail(string email, string subject, string body)
        {
            try
            {
                var activeSmtpEmailData = await _context.EmailConfigs.FirstOrDefaultAsync(x => x.Active == true);
                var smtpUsername = activeSmtpEmailData.Email;
                var smtpPassword = activeSmtpEmailData.Password;
                var cc = activeSmtpEmailData.EmailCC;
                using var client = new SmtpClient(activeSmtpEmailData.SmtpServer);
                client.Port = (int)activeSmtpEmailData.SmtpPort;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;
                var from = new MailAddress(smtpUsername);
                var to = new MailAddress(email);
                var mailMessage = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    var ccRecipients = cc.Split(';');
                    foreach (var ccRecipient in ccRecipients)
                    {
                        mailMessage.CC.Add(ccRecipient.Trim());
                    }
                }
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to send email:{e}");
            }
        }
    }
}
