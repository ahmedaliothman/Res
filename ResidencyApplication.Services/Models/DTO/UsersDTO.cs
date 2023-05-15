using System;

public class UsersDTO
{
    public int UserId { get; set; }
    public string CivilIdSerialNumber { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public bool ResidencyByMoa { get; set; }
    public string NationalityId { get; set; }
    public bool IsSapUser { get; set; }
    public bool? IsActive { get; set; }
}
