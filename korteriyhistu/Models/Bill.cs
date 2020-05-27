using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Value generated on add
        public int BillId { get; set; }
        public int Number { get; set; }
        public double Total { get; set; }
        public int Apartment { get; set; }
        public double SumToPay { get; set; }
        public int MonthToPayFor { get; set; }
        public int YearToPayFor { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public int Status { get; set; }
        public List<LogEntry> Logs { get; set; }
        public string Comment { get; set; }

        public Bill(double total, int number, int apartment, double sumToPay, int monthToPayFor, int yearToPayFor, DateTime deadline, List<LogEntry> logs, string comment)
        {
            this.Total = total;
            this.Number = number;
            this.Apartment = apartment;
            this.SumToPay = sumToPay;
            this.MonthToPayFor = monthToPayFor;
            this.YearToPayFor = yearToPayFor;
            this.PaymentDeadline = deadline;
            this.Status = (int)BillStatus.UNPAID;
            this.Logs = logs;
            this.Comment = comment;
        }

       public Bill() { }
    }
}
