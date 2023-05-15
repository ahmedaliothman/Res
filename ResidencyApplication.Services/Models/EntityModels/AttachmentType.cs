using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class AttachmentType
    {
        public AttachmentType()
        {
            ApplicationAttachments = new HashSet<ApplicationAttachment>();
        }

        public int AttachmentTypeId { get; set; }
        public string AttachmentTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ApplicationAttachment> ApplicationAttachments { get; set; }
    }
}
