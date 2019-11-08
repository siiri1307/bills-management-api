using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using korteriyhistu.Data;
using korteriyhistu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace korteriyhistu.Controllers
{
    [Route("api/PDFs")]
    public class PDFGeneratorController : Controller
    {
        private readonly ISupervisor supervisor;
        private readonly IEmailService emailService;
        private readonly BillsContext billsContext;
        private readonly ApartmentsContext apartmentsContext;
        byte[][] billAsBinaryData = new byte[3][]; //jagged array, or array of arrays. Declares an array which has 3 elements, each of which is an array of bytes

        private IConverter converter;

        //for DinkToPdf
        GlobalSettings globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 35 },
            DocumentTitle = "PDF Bill",
            //Out = @"C:\Users\siiri\source\repos\korteriyhistu\korteriyhistu\Out\Test_PDF_Gen.pdf"
        };

        public PDFGeneratorController(IConverter converter, BillsContext context, ApartmentsContext apartmentsContext, ISupervisor supervisor, IEmailService emailService)
        {
            this.converter = converter;
            this.billsContext = context;
            this.apartmentsContext = apartmentsContext;
            this.supervisor = supervisor;
            this.emailService = emailService;
        }

        private int getMaxBillNumberValue()
        {
            return this.billsContext.Bill.Max(b => b.Number);
        }

        // GET: api/PDFs
        [Produces("application/pdf")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {/*
            //should DB connection be in controller or Model?
            var billsForRunningMonth = this.billsContext.Bill.Where(bill => bill.MonthToPayFor == (DateTime.Now.Month)).ToList();

            if (billsForRunningMonth.Count() == 0)
            {
                return StatusCode(404);
            }

            var apartments = this.apartmentsContext.Apartment.ToArray();

            for(var i = 0; i < apartments.Length; i++)
            {
                //set settings for DinkToPdf
                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = HTMLTemplateGenerator.GetBillAsHtmlString(this.billsContext, this.apartmentsContext, apartments.ElementAtOrDefault(i).number),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "bills.scss") },
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                //store contents of the PDF as a binary data to the jagged array
                billAsBinaryData[i] = this.converter.Convert(pdf);
            }*/

            byte[][] billsBinary = await supervisor.GetBillsBinary();

            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < billsBinary.Length; i++) {
                    
                        var zipArchiveEntry = archive.CreateEntry("bill" + (i+1) + ".pdf", CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(billsBinary[i], 0, billsBinary[i].Length);
                    }
                } 
                return File(ms.ToArray(), "application/zip", "Archive.zip");
            }
        }

        // GET: api/PDFs/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/PDFs
        /*
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        */
        
        // PUT: api/PDFs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //just trial. Refactor this part.
        [HttpPost]
        public async Task SendPDFBillsToEmail()
        {
            var billsAsBinary = await supervisor.GetBillsBinary();

            EmailMessage email = new EmailMessage(new EmailAddress { Name = "Jakobi korteriühistu", Address = "" },
                new List<EmailAddress> { new EmailAddress { Name = "Siiri", Address = "" } },
                "Novembri arved", "Saadan teile novembri arved", billsAsBinary);

            await emailService.Send(email);
        }

            /*
            var billsForRunningMonth = this.billsContext.Bill.Where(bill => bill.MonthToPayFor == (DateTime.Now.Month)).ToList();

            if (billsForRunningMonth.Count() == 0)
            {
                return StatusCode(404);
            }

            var apartments = this.apartmentsContext.Apartment.ToArray();

            for (var i = 0; i < apartments.Length; i++)
            {
                //set settings for DinkToPdf
                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = HTMLTemplateGenerator.GetBillAsHtmlString(this.billsContext, this.apartmentsContext, apartments.ElementAtOrDefault(i).number),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "bills.scss") },
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                //store contents of the PDF as a binary data to the jagged array
                billAsBinaryData[i] = this.converter.Convert(pdf);
            }*/


            /*
            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("", "");
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("");
                    mail.To.Add("");
                    mail.Subject = "Novembri arved";
                    mail.Body = "Saadan teile novembri arved";
                    mail.Attachments.Add(new Attachment(new MemoryStream(billsAsBinary[0]), "bill1.pdf"));
                    mail.Attachments.Add(new Attachment(new MemoryStream(billsAsBinary[1]), "bill2.pdf"));
                    mail.Attachments.Add(new Attachment(new MemoryStream(billsAsBinary[2]), "bill3.pdf"));

                    //Oops: from/recipients switched around here...
                    //smtpClient.Send("targetemail@targetdomain.xyz", "myemail@gmail.com", "Account verification", body);
                    smtpClient.Send(mail);
                 
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: {1}", e.ToString(), e.Message);
            }
            
            return Ok();*/

        

    }
}
