using System;

public class ApplicationAttachmentsLogDTO
{
    public int AttachmentLogId { get; set; }
    public int AttachmentId { get; set; }
    public string AttachmentName { get; set; }
    public string AttachmentPath { get; set; }
    public int AttachmentTypeId { get; set; }
    public DateTime?  ApplicationNumber { get; set; }
    public string Action { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool? IsActive { get; set; }
}
