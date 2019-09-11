using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using korteriyhistu.Models;

namespace korteriyhistu.Models
{
    public class BillsContext : DbContext
    {
        public BillsContext (DbContextOptions<BillsContext> options)
            : base(options)
        {
        }

        public DbSet<Bill> Bill { get; set; }
    }
}
