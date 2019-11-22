using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public class EmailMessage
    {
        public List<EmailAddress> ToAddresses { get; set; }
        public EmailAddress FromAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public byte[][] Attachments;
        public Dictionary<string, byte[]> BillsFileNamesWithBinary { get; set; }

        public EmailMessage(string subject, string body, Dictionary<string, byte[]> billsFileNamesWithBinary)
        {
            this.Subject = subject;
            this.Content = body;
            this.BillsFileNamesWithBinary = billsFileNamesWithBinary;

        }

        public EmailMessage(EmailAddress from, List<EmailAddress> to, string subject, string body, byte[][] attachements)
        {
            this.FromAddress = from;
            this.ToAddresses = to;
            this.Subject = subject;
            this.Content = body;
            this.Attachments = attachements;
        }
    }
}
