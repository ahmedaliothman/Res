using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class NotificationSetting
    {
        public int NotificationId { get; set; }
        public bool? AcceptEmailNotification { get; set; }
        public bool? RejectEmailNotification { get; set; }
        public bool? ReturnEmailNotification { get; set; }
        public bool? RegistEmailNotification { get; set; }
        public bool? PaymentEmailNotification { get; set; }
        public bool? AcceptSmsNotification { get; set; }
        public bool? RejectSmsNotification { get; set; }
        public bool? ReturnSmsNotification { get; set; }
        public bool? RegistSmsNotification { get; set; }
        public bool? PaymentSmsNotification { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
