using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Models.Settings
{
    public class EmailSettings
    {
        public string EMail { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
    public class EmailInfo
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string username { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
