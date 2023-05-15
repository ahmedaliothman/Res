﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class ApplicationStatus
    {
        public ApplicationStatus()
        {
            UserApplications = new HashSet<UserApplication>();
        }

        public int ApplicationStatusId { get; set; }
        public string ApplicationStatusName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserApplication> UserApplications { get; set; }
    }
}
