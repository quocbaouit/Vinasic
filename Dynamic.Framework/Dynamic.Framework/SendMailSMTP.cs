using System;
using System.Net;
using System.IO;
using System.Net.Mail;
namespace Dynamic.Framework
{
    public class SendMailSMTP
    {

        public void SendMail(String mailFrom,string passWord, string mailTo, string sujb, string bd)
        {
            var fromAddress = new MailAddress(mailFrom);
           
            var fromPassword = passWord;
            var toAddress = new MailAddress(mailTo);

            string subject = sujb;
            string body = bd;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml=true
            })


                smtp.Send(message);
        }
    }
}
