using System;

public class UserApplicationsLogDTO
{
    public int ApplicationNumberLogId { get; set; }
    public int ApplicationNumber { get; set; }
    public int UserId { get; set; }
    public int ApplicationStatusId { get; set; }
    public int ApplicationTypeId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public bool IsActive { get; set; }
    public string Remark { get; set; }
    public int? StepNo { get; set; }
}


