using System;
using System.Net;
using System.Net.Mail;

namespace SmartReservationCinema.Services
{
    public class MailSender
    {
        public async void SendMessage(string email,string subject, string text)
        {
            string fromEmail = "smartrevervation12@outlook.com";
            string yourPassword = "sMrtResr1";
            string fromName = "Smart Reservation Cinema :3";
            string toEmail = email;        

            string msgTheme = subject;

            MailAddress from = new MailAddress(fromEmail, fromName);
            MailAddress to = new MailAddress(toEmail);
            MailMessage m = new MailMessage(from, to);
            m.Subject = msgTheme;
            m.Body = text;
            m.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);//465
            smtp.Credentials = new NetworkCredential(fromEmail, yourPassword);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(m);
            

        }
    }
}
