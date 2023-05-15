using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class User
    {
        public User()
        {
            SapUsers = new HashSet<SapUser>();
            UserApplications = new HashSet<UserApplication>();
        }

        public int UserId { get; set; }
        public string CivilIdSerialNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool ResidencyByMoa { get; set; }
        public bool IsSapUser { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
        public string NationalityId { get; set; }
        public int? SectionId { get; set; }
        public int? UserTypeId { get; set; }
        public int? JobtypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? EmployeeNumber { get; set; }
        public int? UserRoleId { get; set; }
        public int? GenderId { get; set; }

        public virtual NonSapUser NonSapUser { get; set; }
        public virtual ICollection<SapUser> SapUsers { get; set; }
        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
