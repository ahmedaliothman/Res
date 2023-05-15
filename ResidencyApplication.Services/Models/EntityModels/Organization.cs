using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class Organization
    {
        public int Id { get; set; }
        public int SapId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
