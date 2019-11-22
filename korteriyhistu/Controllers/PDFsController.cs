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
        private readonly ISupervisor billsSupervisor;
        private readonly IEmailService emailService;

        public PDFGeneratorController(ISupervisor supervisor, IEmailService emailService)
        {
            this.billsSupervisor = supervisor;
            this.emailService = emailService;
        }
        
        
        // GET: api/PDFs
        [Produces("application/pdf")]
        [HttpPost("getZip")]
        public async Task<IActionResult> GetBillsZip([FromBody] BillViewModel model)
        {
            try
            {
                Dictionary<string, byte[]> billsFileNameWithBinary = await billsSupervisor.GetBillsBinary(model.billsList);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach(KeyValuePair<string, byte[]> kvp in billsFileNameWithBinary)
                        {
                            var zipArchiveEntry = archive.CreateEntry(kvp.Key+ ".pdf", CompressionLevel.Fastest);
                            using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(kvp.Value, 0, kvp.Value.Length);
                        }
                    }
                    return File(ms.ToArray(), "application/zip", "Archive.zip");
                }
            }
            catch (NullReferenceException)
            {
                return StatusCode(404);
            }
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

        [HttpPost]
        public async Task<IActionResult> SendPDFBillsToEmail([FromBody] BillViewModel model)
        {
       
            try
            {
                var billsAsBinary = await billsSupervisor.GetBillsBinary(model.billsList);
                //think what could be posted, like mail title, body, selected bills. Use defaults, if not provided.
                EmailMessage email = new EmailMessage("Jakobi 31 korteriühistu arved", "Saadan teile eelneva kuu arved.\nTervitades\nVootele", billsAsBinary);

                await emailService.SendAsync(email);
                return Ok();
            }
            catch (NullReferenceException) {
                return StatusCode(404);
            }
         }
    }
}
