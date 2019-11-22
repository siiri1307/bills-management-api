using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string Username { get; set; }
        string Password { get; set; }
        string MailFrom { get; set; }
        List<string> MailTo { get; set; }
    }
}
