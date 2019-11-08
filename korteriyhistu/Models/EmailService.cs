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
using System.Threading;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public class EmailService : IEmailService
    {
        private EmailConfiguration emailConfiguration;

        public EmailService()
        {
            //this.emailConfiguration = emailConfiguration;
            
        }
        public async Task Send(EmailMessage email)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("", "");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                //Message part
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email.FromAddress.Address);// 

                for(int j = 0; j < email.ToAddresses.Count; j++)
                {
                    mail.To.Add(email.ToAddresses.ElementAt(j).Address);
                }
                //mail.To.Add("");

                mail.Subject = email.Subject; //"Novembri arved";
                mail.Body = email.Content; //"Saadan teile novembri arved";

                for(int i = 0; i < email.Attachments.Length; i++)
                {
                    mail.Attachments.Add(new Attachment(new MemoryStream(email.Attachments.ElementAt(i)), "bill" + (i+1) + ".pdf"));
                }
                //mail.Attachments.Add(new Attachment(new MemoryStream(billsAsBinary[0]), "bill1.pdf"));
                //mail.Attachments.Add(new Attachment(new MemoryStream(billsAsBinary[1]), "bill2.pdf"));
                //mail.Attachments.Add(new Attachment(new MemoryStream(billsAsBinary[2]), "bill3.pdf"));

                
                await smtpClient.SendMailAsync(mail);
            }
        }
            
    }
}