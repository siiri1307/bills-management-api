using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration;
            
        }
        public async Task SendAsync(EmailMessage email)
        {
            using (var smtpClient = new SmtpClient(this.emailConfiguration.SmtpServer, this.emailConfiguration.SmtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(this.emailConfiguration.Username, this.emailConfiguration.Password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                //Message part
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(this.emailConfiguration.MailFrom);

                for(int j = 0; j < this.emailConfiguration.MailTo.Count; j++)
                {
                    mail.To.Add(this.emailConfiguration.MailTo.ElementAt(j));
                }
             
                mail.Subject = email.Subject;
                mail.Body = email.Content; 

                foreach(KeyValuePair<string, byte[]> kvp in email.BillsFileNamesWithBinary)
                {
                    mail.Attachments.Add(new Attachment(new MemoryStream(kvp.Value), kvp.Key + ".pdf"));
                }

                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}