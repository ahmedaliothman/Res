using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class Nationality
    {
        public string NationalityId { get; set; }
        public string NationalityName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
