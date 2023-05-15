using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Models.CustomInputTypes
{
    public class UserProfile

    {
        public int UserId { get; set; }

        public string MobileNumber { get; set; }

        public string EmployeeNumber { get; set; }

        public string Email { get; set; }

        public string CivilIdSerialNumber { get; set; }
    }
}
