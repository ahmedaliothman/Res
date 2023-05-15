using System;

public class PersonalInformationDTO
{
    public int id { get; set; }
    public long EmployeeNumber { get; set; }
    public string EmployeeNameArabic { get; set; }
    public string EmployeeNameEnglish { get; set; }
    public DateTime? BirthDate { get; set; }
    public string MobileNumber { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
    public DateTime? HireDate { get; set; }
    public int? ApplicationNumber { get; set; }
    public int? UserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
