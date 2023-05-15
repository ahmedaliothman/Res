using System;

public class ApplicationTypesDTO
{
    public int ApplicationTypeId { get; set; }
    public string ApplicationTypeName { get; set; }
    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }


}
