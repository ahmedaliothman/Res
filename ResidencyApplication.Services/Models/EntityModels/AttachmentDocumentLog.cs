using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class AttachmentDocumentLog
    {
        public int Id { get; set; }
        public int? AttachmentDocumentId { get; set; }
        public int? UserId { get; set; }
        public int? ApplicationNumber { get; set; }
        public string ApprovedLetterForResidencyRenewal { get; set; }
        public string SalaryCertification { get; set; }
        public string CivilIdCopy { get; set; }
        public string PassportCopy { get; set; }
        public string OtherRelatedDocuments { get; set; }
        public string Action { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
