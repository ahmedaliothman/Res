using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class VUserApplication
    {
        public int ApplicationNumber { get; set; }
        public int? UserId { get; set; }
        public string Email { get; set; }
        public long EmployeeNumber { get; set; }
        public string EmployeeNameArabic { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string EmployeeNameEnglish { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string Remark { get; set; }
        public int? ApplicationStatusId { get; set; }
        public int? ApplicationTypeId { get; set; }
    }
}
