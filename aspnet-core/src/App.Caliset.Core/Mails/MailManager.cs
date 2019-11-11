using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace App.Caliset.Mails
{
    public class MailManager : DomainService, IMailManager
    {
        public MailManager(){ }
        static void Main(string to, string subject, string body)
        {
            string from = "caliset.notification@gmail.com";
            string Password = "caliset123";

            MailMessage mailMessage = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new System.Net.NetworkCredential(from, Password)
            };

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

        }

        public void SendMail(string to, string subject, string body)
        {
            Main(to, subject, body);
        }
    }
}
