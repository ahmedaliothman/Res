using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class UserApplicationsLog
    {
        public int ApplicationNumberLogId { get; set; }
        public int ApplicationNumber { get; set; }
        public int UserId { get; set; }
        public int ApplicationStatusId { get; set; }
        public int ApplicationTypeId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Remark { get; set; }
        public int? StepNo { get; set; }
        public string Action { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
