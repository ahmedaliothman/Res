using E_SahelService_Service.Extensions;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_SahelService_Service.Exceptions
{
    public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            

            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}
