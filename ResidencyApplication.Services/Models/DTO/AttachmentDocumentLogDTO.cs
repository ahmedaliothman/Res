using System;

public class AttachmentDocumentLogDTO
{
    public int id { get; set; }
    public int AttachmentDocumentId { get; set; }
    public string ApprovedLetterForResidencyRenewal { get; set; }
    public string SalaryCertification { get; set; }
    public string CivilIdCopy { get; set; }
    public string PassportCopy { get; set; }
    public string OtherRelatedDocuments { get; set; }
    public int? ApplicationNumber { get; set; }
    public int? UserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
