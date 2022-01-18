using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingService.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string fromAddress, string destinationAddress, string subject, string textMessage);
    }
}
