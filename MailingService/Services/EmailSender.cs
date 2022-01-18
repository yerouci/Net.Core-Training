using MailingService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingService.Services
{
    public class EmailSender: IEmailSender
    {
        public async Task SendEmailAsync(string fromAddress, string destinationAddress, string subject, string textMessage) {
            
            //Sending Email
            await Task.CompletedTask;
        }
    }
}
