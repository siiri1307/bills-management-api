using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Google.Apis.Auth;
using korteriyhistu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace korteriyhistu.Controllers
{
    [Produces("application/json")]
    [Route("api/Authenticate")]
    public class AuthenticateController : Controller
    {
        string appClientId = "";
        string appClientSecret = "";
        string userEmail = "";
        string gmailScope = "https://mail.google.com/";
        private const string GoogleApiTokenInfoUrl = "https://oauth2.googleapis.com/tokeninfo?id_token={0}";
        
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateAccessToken([FromBody] string accessToken)
        {
           
            try
            {
                Payload payload = await GoogleJsonWebSignature.ValidateAsync(accessToken); //verify the integrity of Google ID token
                string email = payload.Email;
                return Ok();
            }
            catch(InvalidJwtException e)
            {
                return StatusCode(401);
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendBillsViaEmail()
        {
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
                    mail.Attachments.Add(new Attachment(@"C:\Users\siiri\Downloads\arved\bill1.pdf"));
                    mail.Attachments.Add(new Attachment(@"C:\Users\siiri\Downloads\arved\bill2.pdf"));
                    mail.Attachments.Add(new Attachment(@"C:\Users\siiri\Downloads\arved\bill3.pdf"));

                    //Oops: from/recipients switched around here...
                    //smtpClient.Send("targetemail@targetdomain.xyz", "myemail@gmail.com", "Account verification", body);
                    smtpClient.Send(mail);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: {1}", e.ToString(), e.Message);
            }
            //EmailService emailService = new EmailService();
            //await emailService.Send();
            return Ok();
        }


    }
}