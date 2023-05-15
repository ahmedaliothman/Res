using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using ResidencyApplication.Services.Models.Settings;
using MimeKit;
using System;
using System.Globalization;
using ResidencyApplication.Services.Models.EntityModels;
using System.Collections;
using System.Linq;
namespace ResidencyApplication.Services.Models.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _mailSettings;
        private readonly ResidencyApplicationContext _context;

        public EmailService(IOptions<EmailSettings> mailSettings, ResidencyApplicationContext context)
        {
            _mailSettings = mailSettings.Value;
            _context = context;
        }
        public async Task sendWithStatus(int status,string email,string username,string comment ,string appTypeName) {
            var rules = _context.NotificationSettings.FirstOrDefault();
            EmailInfo inputEmail = new EmailInfo();
            inputEmail.EmailTo = email;
            inputEmail.Subject = "نظام تجديد الاقامات";
            inputEmail.username = username;
            if (status == 1&& rules.AcceptEmailNotification==true)
                inputEmail.Body = comment;

            if (status == 2&& rules.RejectEmailNotification==true)
                inputEmail.Body = "تم رفض معاملة "+ appTypeName + "  التعليق :"+comment;
            
            if (status == 3&& rules.ReturnEmailNotification==true)
                inputEmail.Body = "تم إرجاع معاملة " + appTypeName + "التعليق:" + comment; ;
            if (status == 5)
                inputEmail.Body = "جاري العمل على المعاملة";
            
            SendEmailAsync(inputEmail);
        }
        public async Task SendEmailAsync(EmailInfo emailInfo)
        {
            //Fetching Email Body Text from EmailTemplate File.  
            string FilePath = @"D:\C#\React+C#API\workMOA\Residency\FRS-Residency_Renewal-100121\ResidencyApplication.Services\EmailTemplates\CustomTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            //Repalce [newusername] = signup user name   
            MailText = MailText.Replace("[username]", emailInfo.username);
            MailText = MailText.Replace("[body]", emailInfo.Body);
            MailText = MailText.Replace("[date]", DateTime.Now.ToString("dd dddd , MMMM, yyyy", new CultureInfo("ar-AE")));
            MailText = MailText.Replace("[url]", "http://localhost:25004/");
            var email = new MailMessage(_mailSettings.EMail, emailInfo.EmailTo);
            email.Subject = emailInfo.Subject;
            email.IsBodyHtml = true;
            var builder = new BodyBuilder();
            if (emailInfo.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in emailInfo.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = emailInfo.Body;
            email.Body = MailText;
            using var smtp = new SmtpClient();
            smtp.Port = _mailSettings.Port;
            smtp.Host = _mailSettings.Host;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = false;
            smtp.Credentials = new System.Net.NetworkCredential(_mailSettings.EMail, _mailSettings.Password);
             smtp.Send(email);
            
        }

        public Task sendWithStatus(int status, string email, string username)
        {
            throw new NotImplementedException();
        }
    }
}
