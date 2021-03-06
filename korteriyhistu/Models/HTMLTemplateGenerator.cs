﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public static class HTMLTemplateGenerator
    {
        public static string GetBillAsHtmlString(BillsContext billsContext, ApartmentsContext apartmentsContext, int apartmentNumber)
        {
            var sb = new StringBuilder();

            var bill = billsContext.Bill.Where(b => b.MonthToPayFor == DateTime.Now.Month && b.Apartment == apartmentNumber).FirstOrDefault();
            var debt = billsContext.Bill.Where(b => b.Apartment == apartmentNumber && b.MonthToPayFor != DateTime.Now.Month).Sum(i => i.SumToPay);

            var apartment = apartmentsContext.Apartment.Where(ap => ap.number == apartmentNumber).FirstOrDefault();

            sb.Append(@"
                <html>
                    <head><meta charset='UTF-8'></head>
                    <body>
                        <h1>2019. " + (MonthInEstonian)bill.MonthToPayFor + @" arve</h1>
                        <div id='subtitle'>Jakobi 31 korteriühistu</div>
                        <div>Registrikood: 80483047</div>
                        <div>Aadress: Jakobi 31, Tartu, 51006</div>
                        <div id='bank-account-number'>Pangakonto: </div>
                        <div>
                            <span class='left-column'>Arve number:</span> 
                            <span>" + bill.Number + @"</span>
                        </div>
                        <div id='deadline'>
                            <span class='left-column'>Maksetähtaeg:</span>
                            <span>" + bill.PaymentDeadline.ToString("d", new CultureInfo("de-DE")) + @"</span>
                        </div>
                        <div>
                            <span class='left-column'>Korter:</span>
                            <span>" + apartment.number + @"</span>
                        </div>
                        <div>
                            <span class='left-column'>Maksja:</span>
                            <span>" + apartment.owners + @"</span>
                        </div>
                        <div id='area'>
                            <span class='left-column'>Korteri üldpinna ruumala:</span>
                            <span>" + apartment.surfaceArea + @"</span>
                        </div>
                        <div>
                            <span class='left-column'>Makse suurus ruutmeetri kohta:</span>
                            <span>1.25 eurot</span>
                        </div>");
            if(apartment.extraSurfaceArea > 0)
            {
                sb.Append(@" <div>
                            <span class='left-column'>Võetud lisakohustus ruutmeetrites:</span>
                            <span>" + apartment.extraSurfaceArea + @"</span></div>");
            }

            sb.Append(@"<div id='payment-for-period'>
                            <span class='left-column'>Remondifondi makse:</span>
                            <span>1.25 * (" + apartment.surfaceArea + @" + " + apartment.extraSurfaceArea + @") = " + bill.Total + @" eurot</span></div>
                        <div>
                            <span class='left-column'>Eelnevate perioodide võlgnevus:</span>
                            <span>" + debt + @" eurot</span></div>
                        <div id='total'>
                            <span class='left-column'>Kokku tasuda:</span>
                            <span>" + (bill.Total + debt) + @" eurot</span>
                        </div>
                        <div id='pdf-author'>
                            <span class='left-column'>Arve koostas:</span> 
                            <span>John Roe</span>
                        </div>
                    </body>
                </html>");                       
                   
            return sb.ToString();
        }
    }
}
