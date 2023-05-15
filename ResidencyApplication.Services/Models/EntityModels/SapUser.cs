using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class SapUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CivilId { get; set; }
        public string Username { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public string JobTitle { get; set; }
        public string EmployeeType { get; set; }
        public string Sector { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public bool? IsActive { get; set; }
        public int? Organization { get; set; }

        public virtual User User { get; set; }
    }
}
