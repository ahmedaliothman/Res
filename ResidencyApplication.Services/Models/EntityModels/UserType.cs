using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class UserType
    {
        public UserType()
        {
            NonSapUsers = new HashSet<NonSapUser>();
        }

        public int UserTypeId { get; set; }
        public string SapId { get; set; }
        public string UserTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<NonSapUser> NonSapUsers { get; set; }
    }
}
