using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.IO;

namespace FaculdadeSI.Models
{
    public static class Email
    {
      
        public static bool SendedEmail(List<string> mails)
        {
            bool response = true;

            StreamReader sr = new StreamReader(@"C:\Faculdade\SenhaEmail.txt");//caminho do seu arquivo txt
            string senha;

            senha = sr.ReadLine(); //vai ler a segunda linha onde está a senha

            foreach (var item in mails)
            {
                try
                {
                    MailMessage email = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";

                    // set up the Gmail server
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("sasestacio@gmail.com", senha);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    // draft the email
                    MailAddress fromAddress = new MailAddress("sasestacio@gmail.com");
                    email.From = fromAddress;
                    email.To.Add(item);
                    email.Subject = "Título";
                    email.Body = "Corpo";

                    smtp.Send(email);
                }

                catch (Exception ex)
                {
                    response = false;
                }
            }

            return response;
        }
    }
}