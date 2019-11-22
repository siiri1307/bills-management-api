using DinkToPdf;
using DinkToPdf.Contracts;
using korteriyhistu.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;


namespace korteriyhistu.Data
{
    public class BillSupervisor : ISupervisor
    {
        private readonly IBillRepository billRepository;
        private readonly IApartmentRepository apartmentRepository;

        //for DinkToPdf
        private IConverter converter;
        private GlobalSettings globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 35 },
            DocumentTitle = "PDF Bill",
        };

        public BillSupervisor(IBillRepository billRepository, IApartmentRepository apartmentRepository, IConverter converter)
        {
            this.billRepository = billRepository;
            this.apartmentRepository = apartmentRepository;
            this.converter = converter;
        }

        public async Task<byte[][]> GetBillsBinary()
        {
            byte[][] billAsBinaryData = new byte[3][]; //jagged array, or array of arrays. Declares an array which has 3 elements, each of which is an array of bytes
            var apartments = await this.apartmentRepository.GetAllAsync();

            for (int i = 0; i < apartments.Count(); i++)
            {
                Bill bill = await billRepository.GetBillCurrentMonthAsync(apartments.ElementAt(i).number);
       
                double debt = await billRepository.GetDebtAsync(apartments.ElementAt(i).number);

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = HTMLGenerator.GetBillAsHtmlString(apartments.ElementAt(i), bill, debt),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "bills.scss") },
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                billAsBinaryData[i] = this.converter.Convert(pdf);
            }

            return billAsBinaryData;
        }

        public async Task<Dictionary<string, byte[]>> GetBillsBinary(List<Bill> requestedBills)
        {
            var apartments = await this.apartmentRepository.GetAllAsync();

            Dictionary<string, byte[]> billsData = new Dictionary<string, byte[]>();
            
            for (int i = 0; i < requestedBills.Count(); i++)
            {
                var apartmentNo = requestedBills.ElementAt(i).Apartment;
                var apartment = await this.apartmentRepository.GetByNumber(apartmentNo);

                Bill bill = await billRepository.GetBillById(requestedBills.ElementAt(i).BillId);

                double debt = await billRepository.GetDebtAsync(apartmentNo); //change this to Apartment as well

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = HTMLGenerator.GetBillAsHtmlString(apartment, bill, debt),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "bills.scss") },
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                //billAsBinaryData[i] = this.converter.Convert(pdf);

                var bin = this.converter.Convert(pdf);
                var pdfFileName = "2019-" + bill.MonthToPayFor + "-korter" + bill.Apartment.ToString();

                billsData.Add(pdfFileName, bin);
            }

            return billsData;
        }
    }
}

