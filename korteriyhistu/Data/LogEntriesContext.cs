using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace korteriyhistu.Models
{
    public class LogEntriesContext : DbContext
    {
        public LogEntriesContext (DbContextOptions<LogEntriesContext> options)
            : base(options)
        {
        }

        public DbSet<korteriyhistu.Models.LogEntry> LogEntry { get; set; }
    }
}
