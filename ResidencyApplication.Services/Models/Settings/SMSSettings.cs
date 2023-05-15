using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Models.Settings
{
    public class SMSSettings
    {
        public string Sender { get; set; }
        public string UID { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }

    }

}
