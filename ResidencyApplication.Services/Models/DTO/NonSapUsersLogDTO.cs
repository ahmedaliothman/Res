using System;

public class NonSapUsersLogDTO
{
    public int UserLogId { get; set; }
    public int UserId { get; set; }
    public string CivilId { get; set; }
    public string Password { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeNumber { get; set; }
    public string Organization { get; set; }
    public int UserTypeId { get; set; }
    public int RegistrationStatusId { get; set; }
    public DateTime RegistrationDate { get; set; }
}
