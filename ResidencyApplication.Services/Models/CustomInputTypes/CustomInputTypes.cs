using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace ResidencyApplication.Services.Models.CustomInputTypes
{
    public class CustomInputTypes
    {
    }

    public class InputAttachmentDocumentObj
    {
        public IFormFile ApprovedLetterForResidencyRenewal { get; set; }
        public IFormFile SalaryCertification { get; set; }
        public IFormFile CivilIdCopy { get; set; }
        public IFormFile PassportCopy { get; set; }
        public List<IFormFile> OtherRelatedDocuments { get; set; }
        public int? ApplicationNumber { get; set; }
        public int? UserId { get; set; }
    }
}
