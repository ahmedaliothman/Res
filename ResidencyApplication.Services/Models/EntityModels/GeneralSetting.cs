using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class GeneralSetting
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int? PreventTypeDays { get; set; }
        public bool? EditReturnActivation { get; set; }
        public bool? ElectronicPaymentActivation { get; set; }
        public int? Value { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
