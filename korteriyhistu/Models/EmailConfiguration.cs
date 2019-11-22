using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string MailFrom { get;  set; }
        
        public List<string> MailTo { get; set; }
    }
}
