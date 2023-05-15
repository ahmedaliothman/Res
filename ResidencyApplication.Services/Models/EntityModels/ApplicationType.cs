using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class ApplicationType
    {
        public ApplicationType()
        {
            UserApplications = new HashSet<UserApplication>();
        }

        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
