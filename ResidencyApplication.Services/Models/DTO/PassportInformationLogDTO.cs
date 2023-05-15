using System;

public class PassportInformationLogDTO
{
    public int PassportInformationLogId { get; set; }
    public int id { get; set; }
    public long? CivilID { get; set; }
    public string NationalityId { get; set; }
    public string PassportNumber { get; set; }
    public string IssueCountry { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Address { get; set; }
    public DateTime? ResidencyExpiryDate { get; set; }
    public int? ApplicationNumber { get; set; }
    public int? UserId { get; set; }
    public string Action { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
