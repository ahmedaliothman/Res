using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class RegistrationStatus
    {
        public RegistrationStatus()
        {
            NonSapUsers = new HashSet<NonSapUser>();
        }

        public int RegistrationStatusId { get; set; }
        public string RegistrationStatusName { get; set; }
        public string RegistrationStatusNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<NonSapUser> NonSapUsers { get; set; }
    }
}
