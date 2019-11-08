using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public interface IEmailService
    {
        Task Send(EmailMessage emailMessage);
        //Task Send();
    }
}
