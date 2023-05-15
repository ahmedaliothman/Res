using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class EmployeeView
    {
        public int UserId { get; set; }
        public string MobileNumber { get; set; }
        public string CivilIdSerialNumber { get; set; }
        public string Email { get; set; }
        public bool IsSapUser { get; set; }
        public bool? IsActive { get; set; }
        public string NationalityId { get; set; }
        public int? SectionId { get; set; }
        public int? JobtypeId { get; set; }
        public int? UserTypeId { get; set; }
        public int? EmployeeNumber { get; set; }
        public int? UserRoleId { get; set; }
        public string CivilId { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public string Valid { get; set; }
        public string StatusMessage { get; set; }
    }
}
