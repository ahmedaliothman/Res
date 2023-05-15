using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Web;
using ResidencyApplication.Services.Models.EntityModels;
using Microsoft.Extensions.Options;
using ResidencyApplication.Services.Models.Settings;
using System.Globalization;

namespace ResidencyApplication.Services.Models.Services
{
    public interface ISMSHelper 
    {
        public bool sendWithStatus(int status, string MobileNumber, string username, string comment, string appTypeName);


    }
    public class SMSHelper:ISMSHelper
    {
        static string _host;
        static IWebHostEnvironment _env;
        private readonly ResidencyApplicationContext _context;
        public readonly SMSSettings _SMSSettings;

        public SMSHelper(IOptions<SMSSettings> smssettings, IWebHostEnvironment env, ResidencyApplicationContext context)
        {
            _env = env;
            _context = context;
            _SMSSettings = smssettings.Value;

        }
        public bool sendWithStatus(int status, string MobileNumber, string username, string comment, string appTypeName)
        {
            var rules = _context.NotificationSettings.FirstOrDefault();
            string msg = $"نظام تجديد الاقامات"
              + " -- "
              + "نوع المعاملة:"
              + "{0}"
              + " -- "
              + "الحالة:"
              + "{1}"
              + " -- "
              + "التعليق:"
              + "{2}";
            var culture = CultureInfo.CurrentCulture;
        //    culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational; // Always use native characters
            string message = "نظام تجديد الاقامات";
            

            if (status == 1 && rules.AcceptSmsNotification == true)
                message = " "+comment;


            if (status == 2 && rules.RejectSmsNotification == true)
            {
                message = string.Format(culture, msg, appTypeName, "تم رفض المعاملة ", comment);
            }

            if (status == 3 && rules.RejectSmsNotification == true)
            {
                message = string.Format(culture, msg, appTypeName, "تم إرجاع المعاملة", comment);

            }
            if (status == 5)
            {
                message = string.Format(culture, msg, appTypeName, "جاري العمل على المعاملة", comment);

            }
            try
            {
                SendSMS(MobileNumber, message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public  bool SendSMS(string mobile, string message)
        {

            _host = _SMSSettings.Host;
            string _URL = _host;
            string _senderid = HttpUtility.UrlEncode(_SMSSettings.Sender);   // here assigning sender id 
            string _user = HttpUtility.UrlEncode(_SMSSettings.UID); // API user name to send SMS
            string _pass = HttpUtility.UrlEncode(_SMSSettings.Password);    // API password to send SMS
            string _recipient = HttpUtility.UrlEncode(mobile);  // who will receive message
            string _messageText = HttpUtility.UrlEncode(message); // text message
            // Creating URL to send sms
            string _createURL = _URL +
            "?UID=" + _user +
               "&p=" + _pass +
               "&S=" + _senderid +
               "&G=965" + _recipient +
               "&M=" + _messageText +
               "&L=A";

            try
            {
                // creating web request to send sms 
                HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(_createURL);
                // getting response of sms
                HttpWebResponse myResp = (HttpWebResponse)_createRequest.GetResponse();
                System.IO.StreamReader _responseStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = _responseStreamReader.ReadToEnd();
                _responseStreamReader.Close();
                myResp.Close();
            }
            catch
            {
                //
            }
            return true;
        }
    }

}
