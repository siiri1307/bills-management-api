using korteriyhistu.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Data
{
    public class MailContext: DbContext
    {
        public MailContext(DbContextOptions<MailContext> options): base(options)
        {}

        public DbSet<Mail> Mail { get; set; }
    }
}
