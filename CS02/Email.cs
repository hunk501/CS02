using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CS02
{
    class Email
    {
        public static string HOST = "smtp.gmail.com";
        public static string FROM = "lddsoftware501@gmail.com";
        public static string TO = "lddsoftware501@gmail.com";
        public static int PORT = 587;
        public static string USER = "lddsoftware501@gmail.com";
        public static string PASSWORD = "89@l24%D01d?501";


        public static void sendEmail(string _body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient(HOST);

                mail.From = new MailAddress(FROM);
                mail.To.Add(TO);
                mail.Subject = "[BG] Annapolis Status";
                mail.Body = _body;

                smtp.Port = PORT;
                smtp.Credentials = new System.Net.NetworkCredential(USER, PASSWORD);
                smtp.EnableSsl = true;

                smtp.Send(mail);
                Console.WriteLine("Email has been sent.!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
