﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage emailMessage);
        //Task Send();
    }
}
