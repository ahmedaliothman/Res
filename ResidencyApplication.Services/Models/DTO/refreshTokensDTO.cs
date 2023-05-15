using System;

public class refreshTokensDTO
{
    public int id { get; set; }
    public string token { get; set; }
    public DateTime? expires { get; set; }
    public DateTime? created { get; set; }
    public string createdByIp { get; set; }
    public DateTime? revoked { get; set; }
    public string revokedByIp { get; set; }
    public string replacedByToken { get; set; }
    public int? userId { get; set; }
}
