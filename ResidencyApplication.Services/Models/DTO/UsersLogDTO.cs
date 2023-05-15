using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Models.DTO
{
    public class UsersLogDTO
    {
        public int UserLogId { get; set; }
        public int UserId { get; set; }
        public string CivilIdSerialNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool ResidencyByMoa { get; set; }
        public string NationalityId { get; set; }
        public bool IsSapUser { get; set; }
        public bool? IsActive { get; set; }
    }
}
