using korteriyhistu.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Data
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly ApartmentsContext context;

        public ApartmentRepository(ApartmentsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Apartment>> GetAllAsync() =>
            await this.context.Apartment.ToListAsync();

        public async Task<Apartment> GetByNumber(int apartmentNumber)
        {
            var apartment = await this.context.Apartment.Where(ap => ap.number == apartmentNumber).FirstOrDefaultAsync();

            return apartment;

        }
    }
}
