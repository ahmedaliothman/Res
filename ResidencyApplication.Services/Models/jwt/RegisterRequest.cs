using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Models.jwt
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string CivilId { get; set; }
        [Required]
        public string EmployeeName { get; set; }

        public string CivilIdSerialNumber { get; set; }
        public string NationalityId { get; set; }
        public string JobTitle { get; set; }

        public string EmployeeType { get; set; }

        public string Organization { get; set; }

        public string GenderId { get; set; }

        public string MobileNumber { get; set; }

        public string EmployeeNumber { get; set; }


        public int RegistrationStatusId = 1;
        public int UserTypeId { get; set; }
        public int SectionId { get; set; }
        public int JobtypeId { get; set; }

        public DateTime RegistrationDate = DateTime.Now;
        public bool IsSapUser = false;
        public bool IsActive = false;
      
       
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }





    }
}
