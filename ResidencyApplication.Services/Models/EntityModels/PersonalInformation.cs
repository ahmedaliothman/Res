using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class PersonalInformation
    {
        public int Id { get; set; }
        public long EmployeeNumber { get; set; }
        public string EmployeeNameArabic { get; set; }
        public string EmployeeNameEnglish { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public DateTime? HireDate { get; set; }
        public int? ApplicationNumber { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }
        public int? SectionId { get; set; }
        public int? UserTypeId { get; set; }
        public int? JobtypeId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
