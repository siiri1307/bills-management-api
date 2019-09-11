using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace korteriyhistu.Models
{
    public class ApartmentsContext : DbContext
    {
        public ApartmentsContext (DbContextOptions<ApartmentsContext> options)
            : base(options)
        {
        }

        public DbSet<korteriyhistu.Models.Apartment> Apartment { get; set; }
    }
}
