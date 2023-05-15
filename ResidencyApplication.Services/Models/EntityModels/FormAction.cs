using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class FormAction
    {
        public int? ActionId { get; set; }
        public string FormAction1 { get; set; }
        public int? PageId { get; set; }
        public string AccessType { get; set; }
        public int? UserId { get; set; }
    }
}
