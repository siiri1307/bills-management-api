using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace korteriyhistu.Models
{
    public class DataGenerator
    {
        public static void Initialize(BillsContext billContext, BudgetContext budgetContext, ApartmentsContext apartmentsContext)
        {
            billContext.Bill.AddRange(
                new Bill
                {
                    BillId = 1000,
                    Number = 1,
                    Total = 110,
                    Apartment = 1,
                    SumToPay = 110,
                    MonthToPayFor = 7,
                    PaymentDeadline = new DateTime(2019, 8, 10),
                    Status = 3,
                    Logs = new List<LogEntry>()
                },
                new Bill
                {
                    BillId = 1001,
                    Number = 2,
                    Total = 95,
                    Apartment = 2,
                    SumToPay = 95,
                    MonthToPayFor = 7,
                    PaymentDeadline = new DateTime(2019, 8, 10),
                    Status = 3,
                    Logs = new List<LogEntry>()
                },
                new Bill
                {
                    BillId = 1002,
                    Number = 3,
                    Total = 100,
                    Apartment = 3,
                    SumToPay = 100,
                    MonthToPayFor = 7,
                    PaymentDeadline = new DateTime(2019, 8, 10),
                    Status = 3,
                    Logs = new List<LogEntry>()
                });
            apartmentsContext.Apartment.AddRange(
                new Apartment
                {
                    id = 1,
                    number = 1,
                    surfaceArea = 71,
                    extraSurfaceArea = 0,
                    owners = "Jane Doe"
                },
                 new Apartment
                 {
                     id = 2,
                     number = 2,
                     surfaceArea = 92,
                     extraSurfaceArea = 0,
                     owners = "Ordinary Joe"
                 },
                 new Apartment
                 {
                     id = 3,
                     number = 3,
                     surfaceArea = 65,
                     extraSurfaceArea = 10,
                     owners = "Mighty Albert"
                 }
                );
            apartmentsContext.SaveChanges();
            billContext.SaveChanges();
            
            }
        }

}
