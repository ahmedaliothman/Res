using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class UserRole
    {
        public int UserRoleId { get; set; }
        public string UserRoleNameEn { get; set; }
        public string UserRoleNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
