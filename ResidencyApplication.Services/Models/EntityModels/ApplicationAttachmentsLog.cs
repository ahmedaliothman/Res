using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class ApplicationAttachmentsLog
    {
        public int AttachmentLogId { get; set; }
        public int AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentPath { get; set; }
        public int AttachmentTypeId { get; set; }
        public int ApplicationNumber { get; set; }
        public string Action { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
