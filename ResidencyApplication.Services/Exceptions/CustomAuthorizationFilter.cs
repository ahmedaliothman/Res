using Hangfire.Dashboard;
using ResidencyApplication.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Exceptions
{
    public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            

            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}
