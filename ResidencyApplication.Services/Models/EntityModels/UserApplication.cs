using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class UserApplication
    {
        public UserApplication()
        {
            ApplicationAttachments = new HashSet<ApplicationAttachment>();
        }

        public int ApplicationNumber { get; set; }
        public int? UserId { get; set; }
        public int? ApplicationStatusId { get; set; }
        public int? ApplicationTypeId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string Remark { get; set; }
        public int? StepNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? SubmittedBy { get; set; }
        public int? AssignedTo { get; set; }

        public virtual ApplicationStatus ApplicationStatus { get; set; }
        public virtual ApplicationType ApplicationType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ApplicationAttachment> ApplicationAttachments { get; set; }
    }
}
