using korteriyhistu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Data
{
    public interface IBillRepository
    {
        Task<IEnumerable<Bill>> GetAllAsync();
        Task<IEnumerable<Bill>> GetAllCurrentMonthAsync();
        Task<Bill> GetBillCurrentMonthAsync(int apartmentNumber); //param of apartment
        Task<double> GetDebtAsync(int apartmentNumber); //param of apartment
        Task<Bill> GetBillById(int id);
        
    }
}
