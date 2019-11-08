using korteriyhistu.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Data
{
    public class BillRepository : IBillRepository
    {
        private readonly BillsContext context;

        public BillRepository(BillsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Bill>> GetAllAsync() => 
            await this.context.Bill.Include(bill => bill.Logs).ToListAsync();

        public async Task<IEnumerable<Bill>> GetAllCurrentMonthAsync() => 
            await this.context.Bill.Where(bill => bill.MonthToPayFor == (DateTime.Now.Month)).ToListAsync();

        public async Task<Bill> GetBillCurrentMonthAsync(int apartmentNumber)
        {
            return await this.context.Bill.Where(b => b.MonthToPayFor == DateTime.Now.Month && b.Apartment == apartmentNumber).FirstOrDefaultAsync();
        }

        public async Task<double> GetDebtAsync(int apartmentNumber)
        {
            return await this.context.Bill.Where(b => b.Apartment == apartmentNumber && b.MonthToPayFor != DateTime.Now.Month).SumAsync(i => i.SumToPay);
        }
    }
}
