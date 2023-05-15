using System;

public class UserApplicationsDTO
{
    public int ApplicationNumber { get; set; }
    public int UserId { get; set; }
    public int ApplicationStatusId { get; set; }
    public int ApplicationTypeId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public bool IsActive { get; set; }
    public string Remark { get; set; }
    public int? StepNo { get; set; }
}

public class UserApplicationsDetailedDTO
{
    public int ApplicationNumber { get; set; }
    public string EmployeeName { get; set; }
    public string CivilId { get; set; }
    public int UserId { get; set; }
    public int ApplicationStatusId { get; set; }
    public string ApplicationStatusName { get; set; }
    public int ApplicationTypeId { get; set; }
    public string ApplicationTypeName { get; set; }
    public DateTime ApplicationDate { get; set; }
    public bool IsActive { get; set; }
    public string Remark { get; set; }
    public int StepNo { get; set; }
    public string StepName { get; set; }
      
}
public class UserApplicationsListDTO
{
    public int ApplicationNumber { get; set; }
    public string EmployeeName { get; set; }
    public string CivilId { get; set; }
    public int UserId { get; set; }
    public int ApplicationStatusId { get; set; }
    public string ApplicationStatusName { get; set; }
    public int ApplicationTypeId { get; set; }
    public string ApplicationTypeName { get; set; }
    public DateTime ApplicationDate { get; set; }
    public bool IsActive { get; set; }
    public string Remark { get; set; }
    public int StepNo { get; set; }
    public string StepName { get; set; }
    public string UserName { get; set; }
}

