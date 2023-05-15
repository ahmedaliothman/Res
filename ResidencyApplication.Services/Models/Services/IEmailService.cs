using ResidencyApplication.Services.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ResidencyApplication.Services.Models.Services
{
   public interface IEmailService
    {
        Task SendEmailAsync(EmailInfo emailInfo);
        Task sendWithStatus(int status, string email, string username, string comment,string appTypeName);
    }
}
