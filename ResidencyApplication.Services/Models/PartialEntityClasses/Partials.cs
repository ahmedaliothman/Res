using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Models.EntityModels
{
   

    public partial class RefreshToken
    {
        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsActive => Revoked == null && !IsExpired;


    }
    public partial class EmployeeView
    {
        public Boolean? SapUser => IsSapUser;
        public Boolean? Exsists = true;
    }
}
