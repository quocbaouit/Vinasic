using System;
using System.Net;
using System.IO;
using System.Net.Mail;
namespace Dynamic.Framework
{
   public class SendMailSMTP
    {

        public void SendMail(String mailFrom,string mailTo,string subject, string body)
        {
            MailMessage mail = new MailMessage(mailFrom, mailTo);
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(mailFrom.Trim(), "tranquocbaouit@@@");
            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
        }
    }
}
