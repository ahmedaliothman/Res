using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class UsersLog
    {
        public int UserLogId { get; set; }
        public int UserId { get; set; }
        public string CivilIdSerialNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool ResidencyByMoa { get; set; }
        public string NationalityId { get; set; }
        public bool IsSapUser { get; set; }
        public string Action { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
