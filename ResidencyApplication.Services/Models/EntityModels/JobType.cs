using System;
using System.Collections.Generic;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class JobType
    {
        public int Id { get; set; }
        public int? Sapid { get; set; }
        public string Name { get; set; }
        public int IdSergate { get; set; }
    }
}
