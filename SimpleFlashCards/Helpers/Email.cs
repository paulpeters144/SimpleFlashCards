using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SimpleFlashCards.Helpers
{
    public enum EmailType
    {
        Register,
        Reset
    }
    public class Email
    {
        public EmailType Type { get; set; }
        public string ToEmail { get; }
        public string EmailAccount { get; set; }
        public string ClientSKey { get; set; }
        public string ClientId { get; set; }
        public string BaseUrl { get; set; }
        public string MainEmail { get; set; }
        public string Cred { get; set; }

        public Email(EmailType type, string toEmail)
        {
            Type = type;
            ToEmail = toEmail;
            var appsettings = new AppSettings();
            MainEmail = appsettings.GetSetting("Email:MainEmail");
            Cred = appsettings.GetSetting("Email:Cred");
        }
        public bool Send()
        {
            bool result = false;
            try
            {
                switch (Type)
                {
                    case EmailType.Register:
                        if (BaseUrl != null)
                        {
                            sendRegisterEmail();
                            result = true;
                        }
                        break;
                    case EmailType.Reset:
                        if (BaseUrl != null)
                        {
                            sendResetEmail();
                            result = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        private void sendResetEmail()
        {
            string resetTemplate = getEmailTemplate("Reset");

            string url = BaseUrl + "/reset?token=" + new Secure().Encrypt(ToEmail);
            resetTemplate = resetTemplate.Replace("XX_REGLINK_XX", url);

            sendEmail(resetTemplate, "Reset");
        }
        private void sendRegisterEmail()
        {
            string regTemplate = getEmailTemplate("Register");

            string url = BaseUrl + "/registered?token=" + new Secure().Encrypt(ToEmail);
            regTemplate.Replace("XX_REGLINK_XX", url);

            sendEmail(regTemplate, "Register");
        }
        private string getEmailTemplate(string name)
        {
            string path = Directory.GetCurrentDirectory() + @$"/Data/{name}.txt";

            if (!File.Exists(path))
                throw new Exception("Path to email template does not exist.");

            string emailTemplate = File.ReadAllText(path);
            return emailTemplate;
        }
        private void sendEmail(string emailTemplate, string subject)
        {
            var SmtpServer = new SmtpClient()
            {
                Host = "smtp.zoho.com",
                EnableSsl = true,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(MainEmail, Cred)
            };

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(MainEmail);
            mail.To.Add(ToEmail);
            mail.Subject = subject;
            mail.Body = emailTemplate;
            mail.IsBodyHtml = true;

            SmtpServer.Send(mail);
        }
    }
}
