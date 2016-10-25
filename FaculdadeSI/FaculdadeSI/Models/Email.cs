using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace FaculdadeSI.Models
{
    public static class Email
    {
      
        public static bool SendedEmail(List<string> emails, int idAvaliação)
        {
            bool response = true;

            StreamReader sr = new StreamReader(@"C:\Faculdade\SenhaEmail.txt");//caminho do seu arquivo txt
            string senha;

            senha = sr.ReadLine(); //vai ler a segunda linha onde está a senha

            foreach (var item in emails)
            {
                //Verifica se o e-mail é válido
                if (emailIsValid(item))
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
                        email.Body = string.Format("http://localhost:34623/Avaliacao/Answer/{0}", idAvaliação);

                        smtp.Send(email);
                    }



                    catch (Exception ex)
                    {
                        response = false;
                    }
                }
            }

            return response;
        }

        public static bool emailIsValid(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}