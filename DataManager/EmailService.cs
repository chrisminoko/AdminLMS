using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;

namespace BackEnd.DataManager
{
    public class EmailService
    {
        private SmtpClient smtpClient;
        private const string Host = "Host", Port = "Port", EmailFrom = "EmailFrom", PassKey = "PassKey";
        protected MailAddress mailFrom { get; set; }
        public EmailService()
        {
            smtpClient = new SmtpClient(ConfigurationManager.AppSettings[Host], int.Parse(ConfigurationManager.AppSettings[Port]));
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings[EmailFrom], ConfigurationManager.AppSettings[PassKey]);
            mailFrom = new MailAddress(ConfigurationManager.AppSettings[EmailFrom], "Homelink");
        }
        public void SendEmail(EmailContent emailContent)
        {
            String line = "";
            using (StreamReader sr = new StreamReader(HostingEnvironment.MapPath("~/EmailTemplate/Email.html")))
            {
                line = sr.ReadToEnd();

            }

            MailMessage message = new MailMessage();
            message.From = mailFrom;
            foreach (MailAddress address in emailContent.mailTo)
                message.To.Add(address);
            foreach (MailAddress address in emailContent.mailCc)
                message.CC.Add(address);
            message.Subject = emailContent.mailSubject;
            message.Priority = emailContent.mailPriority;
           
            message.Body = emailContent.mailBody + "<br/<br/>" + emailContent.mailFooter;
            line.Replace("change this link here", message.Body);
            message.Body = line;
            message.IsBodyHtml = true;
            foreach (Attachment attachment in emailContent.mailAttachments)
                message.Attachments.Add(attachment);

            try
            {
                smtpClient.Send(message);
            }
            catch { }
        }
    }

    public class EmailContent
    {
        public List<MailAddress> mailTo { get; set; }
        public List<MailAddress> mailCc { get; set; }
        public string mailSubject { get; set; }
        public string mailBody { get; set; }
        public string mailFooter { get; set; }
        public MailPriority mailPriority { get; set; }
        public List<Attachment> mailAttachments { get; set; }
    }
}