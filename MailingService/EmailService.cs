using MailingService.Contracts;
using System;
using System.Threading.Tasks;

namespace MailingService
{
    public class EmailService
    {
        private readonly IEmailSender emailSender;
        public EmailService(IEmailSender _emailSender)
        {
            emailSender = _emailSender;
        }

        public async Task SendEmailAsync(string fromAddress, string destinationAddress, string subject, string textMessage)
        {
            await emailSender.SendEmailAsync(fromAddress, destinationAddress, subject, textMessage);
        }
    }
}
