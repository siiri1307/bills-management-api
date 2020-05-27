using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using korteriyhistu.Models;
using System.IO;
using CsvHelper;
using System.Text;
using CsvHelper.Configuration;
using System.Globalization;

namespace korteriyhistu.Controllers
{
    //[Produces("application/json")] //content type of output
    [Route("api/Bills")] //tells the server to use this controller when the route starts with api/Bills
    public class BillsController : Controller
    {
        private readonly BillsContext billsContext;
        private readonly ApartmentsContext apartmentsContext;

        public BillsController(BillsContext context, ApartmentsContext apartmentsContext)
        {
            billsContext = context;
            this.apartmentsContext = apartmentsContext;
        }

        // GET: api/Bills
        [HttpGet] //method annotation
        public IEnumerable<Bill> GetBill()
        {
            return billsContext.Bill.Include(bill => bill.Logs);
        }

        [Produces("text/csv")]
        [HttpGet("download")]
        public IActionResult DownloadLogs()
        {
            var bills = billsContext.Bill.Include(bill => bill.Logs);

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter))
            {
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.Configuration.RegisterClassMap<BillsMap>();
                csvWriter.Configuration.RegisterClassMap<LogEntriesMap>();
                //csvWriter.Configuration.HasHeaderRecord = true;
                //csvWriter.Configuration.AutoMap<Bill>();

                csvWriter.WriteHeader<Bill>();
                csvWriter.WriteHeader<LogEntry>();
                
                csvWriter.NextRecord(); //to move records to the line after header

                //csvWriter.WriteRecords(bills);

                foreach (Bill bill in bills)
                {
                    csvWriter.WriteField(bill.Number);
                    csvWriter.WriteField(bill.Apartment);
                    csvWriter.WriteField(bill.MonthToPayFor);
                    csvWriter.WriteField(bill.Total);
                    csvWriter.WriteField(bill.SumToPay);
                    csvWriter.WriteField(bill.PaymentDeadline);

                    for (int i = 0; i < bill.Logs.Count; i++)
                    {
                        if(i == 0)
                        {
                            csvWriter.WriteField(bill.Logs.ElementAt(i).Message);
                            csvWriter.WriteField(bill.Logs.ElementAt(i).Comment);
                            csvWriter.NextRecord();
                        }
                        else
                        {
                            csvWriter.WriteField("");
                            csvWriter.WriteField("");
                            csvWriter.WriteField("");
                            csvWriter.WriteField("");
                            csvWriter.WriteField("");
                            csvWriter.WriteField("");
                            csvWriter.WriteField(bill.Logs.ElementAt(i).Message);
                            csvWriter.WriteField(bill.Logs.ElementAt(i).Comment);
                            if (i != bill.Logs.Count - 1)
                            {
                                csvWriter.NextRecord();
                            }
                        }
                    }
                    csvWriter.NextRecord();
                }
                streamWriter.Flush();

                return File(new MemoryStream(memoryStream.ToArray()), "text/csv", "download.csv");
            }
        }

        [HttpGet("unpaid")]
        public IEnumerable<Bill> GetUnpaidBills()
        {
           var bills = billsContext.Bill.ToList();

            foreach(Bill bill in bills){
                Console.WriteLine(bill.PaymentDeadline.Day);
                Console.WriteLine(DateTime.Now.Day);
            
                Console.WriteLine(DateTime.Now.CompareTo(bill.PaymentDeadline) > 0);
            }

           return billsContext.Bill.Where(bill => bill.Status == 3 && DateTime.Now.CompareTo(bill.PaymentDeadline) > 0);
        }

        // GET: api/Bills/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBill([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bill = await billsContext.Bill.SingleOrDefaultAsync(m => m.BillId == id);

            if (bill == null)
            {
                return NotFound();
            }

            return Ok(bill);
        }

        // PUT: api/Bills/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill([FromRoute] int id, [FromBody] Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bill.BillId)
            {
                return BadRequest();
            }
            
            //https://stackoverflow.com/questions/42735368/updating-related-data-with-entity-framework-core
            var billDBState = billsContext.Bill.Include(b => b.Logs).FirstOrDefault(b => b.BillId == id);
            billsContext.Entry(billDBState).CurrentValues.SetValues(bill);

            var logsDB = billDBState.Logs.ToList();
            
            foreach(var log in logsDB) //iterate over logs in the DB
            {
                var incomingLog = bill.Logs.SingleOrDefault(l => l.LogEntryId == log.LogEntryId);
                if(incomingLog != null)
                {
                    billsContext.Entry(log).CurrentValues.SetValues(incomingLog);
                }
                else
                {
                    billsContext.Remove(log);
                }
            }

            foreach(var log in bill.Logs)
            {
                if(logsDB.All(i => i.LogEntryId != log.LogEntryId))
                {
                    billDBState.Logs.Add(log);
                }
            }

            //billsContext.Entry(bill).State = EntityState.Modified; 
            //result in ALL properties being included in the SQL update (whether they've actually changed or not), 
            //EXCEPT for the reference properties for related entities

            //billsContext.Update(bill); //even if log entry ID is the same, log is inserted multiple times.
            //will do the same as above, but will include the reference properties, 
            //also include ALL properties on those entities in the update, whether they've changed or not
            //https://stackoverflow.com/questions/51046340/ef-core-2-0-trouble-cascading-inserts-for-related-entities-when-updating-princ

            try
            {
                await billsContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /*
        // POST: api/Bills
        //generate bills in DB for the running month
        [HttpPost]
        public async Task<IActionResult> PostBills()
        {
            var billsForRunningMonth = this.billsContext.Bill.Where(bill => bill.MonthToPayFor == (DateTime.Now.Month)).ToList();

            if(billsForRunningMonth.Count() > 0)
            {
                return StatusCode(422); 
            }

            var apartments = this.apartmentsContext.Apartment.ToArray();

            for (var i = 0; i < apartments.Length; i++)
            {
                double sum = apartments.ElementAtOrDefault(i).surfaceArea * 1.25 + apartments.ElementAtOrDefault(i).extraSurfaceArea * 1.25;
                DateTime deadline = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 10);

                Bill bill = new Bill(sum, this.getMaxBillNumberValue() + 1, apartments.ElementAtOrDefault(i).number, sum, DateTime.Now.Month, deadline, new List<LogEntry>());
                this.billsContext.Add(bill);
                await this.billsContext.SaveChangesAsync();
            }

            var bills = GetBill();

            return Ok(bills);
        }*/
        
        [HttpPost]
        public async Task<IActionResult> PostBills([FromBody] string monthName)
        {
            int monthToPayFor = Int32.Parse(monthName.Split(" ")[0]);
            int yearToPayFor = Int32.Parse(monthName.Split(" ")[1]);

            //int monthNumber = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
            //missing input from frontend about year
         
            //if now is January and the input month is December, assume you want to make bills for the December of previous year
            //int year = (DateTime.Now.Month.Equals(1) && monthName.Equals("December")) ? (DateTime.Now.Year - 1) : DateTime.Now.Year;

            DateTime forMonthOf = new DateTime(yearToPayFor, monthToPayFor, 10);

            var billsForRunningMonth = this.billsContext.Bill.Where(bill => bill.MonthToPayFor == monthToPayFor && bill.YearToPayFor == yearToPayFor).ToList();

            if (billsForRunningMonth.Count() > 0)
            {
                return StatusCode(422);
            }

            var apartments = this.apartmentsContext.Apartment.ToArray();

            for (var i = 0; i < apartments.Length; i++)
            {
                double sum = apartments.ElementAtOrDefault(i).surfaceArea * 1.25 + apartments.ElementAtOrDefault(i).extraSurfaceArea * 1.25;
                DateTime deadline = forMonthOf.AddMonths(1);

                Bill bill = new Bill(sum, this.getMaxBillNumberValue() + 1, apartments.ElementAtOrDefault(i).number, sum, monthToPayFor, yearToPayFor, deadline, new List<LogEntry>(), "Not paid");
                this.billsContext.Add(bill);
                await this.billsContext.SaveChangesAsync();
            }

            var bills = GetBill();

            return Ok(bills);
        }

        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bill = await billsContext.Bill.SingleOrDefaultAsync(m => m.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }

            billsContext.Bill.Remove(bill);
            await billsContext.SaveChangesAsync();

            return Ok(bill);
        }

        private bool BillExists(int id)
        {
            return billsContext.Bill.Any(e => e.BillId == id);
        }
        
        public void addBillsForRunningMonth()
        {
            var apartments = this.apartmentsContext.Apartment.ToArray();

            for (var i = 0; i < apartments.Length; i++)
            {
                double sum = apartments.ElementAtOrDefault(i).surfaceArea * 1.25 + apartments.ElementAtOrDefault(i).extraSurfaceArea * 1.25;
                DateTime deadline = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 10);

                Bill bill = new Bill(sum, this.getMaxBillNumberValue() + 1, apartments.ElementAtOrDefault(i).number, sum, DateTime.Now.Month, DateTime.Now.Year, deadline, new List<LogEntry>(), "");
                this.billsContext.Add(bill);
                this.billsContext.SaveChanges();
            }
        }

        private int getMaxBillNumberValue()
        {
            return this.billsContext.Bill.Max(b => b.Number);
        }
    }
}

public sealed class BillsMap: ClassMap<Bill>
{
    public BillsMap()
    {
        Map(b => b.Number).Name("Bill number");
        Map(b => b.Apartment);
        Map(b => b.MonthToPayFor).Name("Month");
        Map(b => b.Total);
        Map(b => b.SumToPay).Name("Debt");
        Map(b => b.PaymentDeadline).TypeConverterOption.Format("dd.MM.yyyy").Name("Due date");
    }
}

public sealed class LogEntriesMap: ClassMap<LogEntry>
{
    public LogEntriesMap()
    {
        Map(log => log.Message).Name("Paid");
        Map(log => log.Comment).Name("Comment");
    }
}