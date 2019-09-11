using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using korteriyhistu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace korteriyhistu.Controllers
{
    [Route("api/PDFs")]
    public class PDFGeneratorController : Controller
    {
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

        public PDFGeneratorController(IConverter converter, BillsContext context, ApartmentsContext apartmentsContext)
        {
            this.converter = converter;
            this.billsContext = context;
            this.apartmentsContext = apartmentsContext;
        }

        private int getMaxBillNumberValue()
        {
            return this.billsContext.Bill.Max(b => b.Number);
        }

        // GET: api/PDFs
        [Produces("application/pdf")]
        [HttpGet]
        public IActionResult Get()
        {
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
            }
            
            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var zipArchiveEntry = archive.CreateEntry("bill1.pdf", CompressionLevel.Fastest);
                    using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(billAsBinaryData[0], 0, billAsBinaryData[0].Length);
                    zipArchiveEntry = archive.CreateEntry("bill2.pdf", CompressionLevel.Fastest);
                    using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(billAsBinaryData[1], 0, billAsBinaryData[1].Length);
                    zipArchiveEntry = archive.CreateEntry("bill3.pdf", CompressionLevel.Fastest);
                    using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(billAsBinaryData[2], 0, billAsBinaryData[2].Length);
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
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
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
    }
}
