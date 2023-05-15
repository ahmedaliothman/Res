using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.CustomInputTypes;
using System.Net.Http;
using System.IO;
using System.Reflection;

namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentDocumentController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;
        private readonly string[] _extentions = new string[] { ".txt", ".docx", ".ppt", ".pptx", ".xls", " .xlsx", ".notebook", ".jpg", ".jpeg", ".png", ".cs", ".tiff",".pdf" };
        private readonly int _maxsize = 50000000;
        public AttachmentDocumentController(ResidencyApplicationContext context)
        {
            _context = context;
        }

        // GET: api/AttachmentDocuments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttachmentDocument>>> GetAttachmentDocuments()
        {
            return await _context.AttachmentDocuments.ToListAsync();
        }

        // GET: api/AttachmentDocument/id
        [HttpGet("{ApplicationNumberId}")]
        public async Task<ActionResult<AttachmentDocument>> GetAttachmentDocument(int ApplicationNumberId)
        {
            var AttachmentDocuments = await _context.AttachmentDocuments.Where(r => r.ApplicationNumber == ApplicationNumberId).ToListAsync();

            if (AttachmentDocuments.Count == 0)
            {
                return NotFound();
            }
            var AttachmentDocument = AttachmentDocuments.FirstOrDefault();
            AttachmentDocument.ApprovedLetterForResidencyRenewal= !string.IsNullOrEmpty(AttachmentDocument.ApprovedLetterForResidencyRenewal) ? String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.ApprovedLetterForResidencyRenewal):"";
            AttachmentDocument.CivilIdCopy=!string.IsNullOrEmpty(AttachmentDocument.CivilIdCopy)? String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.CivilIdCopy):"";
            AttachmentDocument.PassportCopy= !string.IsNullOrEmpty(AttachmentDocument.PassportCopy) ? String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.PassportCopy):"";
            AttachmentDocument.SalaryCertification= !string.IsNullOrEmpty(AttachmentDocument.SalaryCertification) ? String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.SalaryCertification):"";
            if (!string.IsNullOrEmpty(AttachmentDocument.OtherRelatedDocuments))
            {
                var OtherRelatedDocumentsArray = AttachmentDocument.OtherRelatedDocuments.Split(",");
                AttachmentDocument.OtherRelatedDocuments = null;
                if (OtherRelatedDocumentsArray.Length > 0)
                {
                    foreach (var item in OtherRelatedDocumentsArray)
                    {
                        string url = String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, item);
                        AttachmentDocument.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments == null ? url : AttachmentDocument.OtherRelatedDocuments + "," + url;

                    }
                }
            }
           // AttachmentDocument.OtherRelatedDocuments= !string.IsNullOrEmpty(AttachmentDocument.OtherRelatedDocuments) ? String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.OtherRelatedDocuments):"";
            
            
            return AttachmentDocument;
        }

        // PUT: api/AttachmentDocument/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]

        public async Task<IActionResult> PutAttachmentDocument(int id, [FromForm] InputAttachmentDocumentObj files)
        {

            var AttachmentDocument = _context.AttachmentDocuments.Where(r => r.ApplicationNumber == files.ApplicationNumber).FirstOrDefault();

            if (AttachmentDocument == null)
            {

                return NotFound();

            }

            string fileuploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", files.ApplicationNumber.ToString(), files.UserId.ToString());

            if (!System.IO.Directory.Exists(fileuploadDir))
            {

                System.IO.Directory.CreateDirectory(fileuploadDir);

            }

            string fileNameUnique = "";

            string guid = Guid.NewGuid().ToString();

            string extention = "";

            string path = "";

            foreach (PropertyInfo prop in files.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type == typeof(List<IFormFile>))
                {
                    try
                    {
                        List<IFormFile> files_ = (List<IFormFile>)prop.GetValue(files, null);
                        if (files_ != null)
                        { 
                        foreach (var file in files_)
                        {

                            if (file != null)
                            {
                                extention = Path.GetExtension(file.FileName).ToLower();

                                if (!_extentions.Contains(extention))
                                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this extention is not allowed");

                                if (file.Length > _maxsize)
                                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this file size is too big over 20MB");

                                fileNameUnique = Path.GetFileNameWithoutExtension(file.FileName) + "_" + id + extention;
                                path = Path.Combine(fileuploadDir, fileNameUnique);
                                using (Stream stream = new FileStream(path, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }
                                if (prop.Name == "OtherRelatedDocuments")
                                    AttachmentDocument.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments == null ? fileNameUnique : AttachmentDocument.OtherRelatedDocuments + ',' + fileNameUnique;


                            }

                        }
                        }
                    }
                    catch (Exception ex )
                    {

                    }
                
                }

                
                if (type == typeof(IFormFile))

                {
                    try
                    {
                        IFormFile file = (IFormFile)prop.GetValue(files, null);

                        if (file != null)
                        {
                            extention = Path.GetExtension(file.FileName).ToLower();

                            if (!_extentions.Contains(extention))

                                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this extention is not allowed");


                            if (file.Length > _maxsize)

                                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this file size is too big over 20MB");


                            fileNameUnique = Path.GetFileNameWithoutExtension(file.FileName) + "_" + guid + extention;

                            path = Path.Combine(fileuploadDir, fileNameUnique);


                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            if (prop.Name == "ApprovedLetterForResidencyRenewal")

                                AttachmentDocument.ApprovedLetterForResidencyRenewal = fileNameUnique;


                            if (prop.Name == "CivilIdCopy")

                                AttachmentDocument.CivilIdCopy = fileNameUnique;


                            if (prop.Name == "PassportCopy")

                                AttachmentDocument.PassportCopy = fileNameUnique;


                            if (prop.Name == "OtherRelatedDocuments")

                                AttachmentDocument.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments == null ? fileNameUnique : AttachmentDocument.OtherRelatedDocuments + ',' + fileNameUnique;



                            if (prop.Name == "SalaryCertification")

                                AttachmentDocument.SalaryCertification = fileNameUnique;
                        }
                    }
                    catch (Exception ex )
                    {

                    }
                  
                }

            }

            AttachmentDocument.ApplicationNumber = files.ApplicationNumber;

            AttachmentDocument.UserId = files.UserId;

            AttachmentDocument.UpdatedDate = DateTime.Now;

            _context.Entry(AttachmentDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                //Insert data into log table
                AttachmentDocumentLog AD = new AttachmentDocumentLog();
                AD.Id = 0;
                AD.AttachmentDocumentId = AttachmentDocument.Id;
                AD.UserId = AttachmentDocument.UserId;
                AD.ApplicationNumber = AttachmentDocument.ApplicationNumber;
                AD.ApprovedLetterForResidencyRenewal = AttachmentDocument.ApprovedLetterForResidencyRenewal;
                AD.SalaryCertification = AttachmentDocument.SalaryCertification;
                AD.CivilIdCopy = AttachmentDocument.CivilIdCopy;
                AD.PassportCopy = AttachmentDocument.PassportCopy;
                AD.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments;
                AD.Action = "Updated";
                AD.CreatedDate = DateTime.Now;
                AD.UpdatedDate = DateTime.Now;
                AD.IsActive = AttachmentDocument.IsActive;
                _context.AttachmentDocumentLogs.Add(AD);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!AttachmentDocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAttachmentDocument", new { ApplicationNumberId = AttachmentDocument.ApplicationNumber }, AttachmentDocument);

        }

        // POST: api/AttachmentDocument
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AttachmentDocument>> PostAttachmentDocument([FromForm] InputAttachmentDocumentObj files)
        {
            var AttachmentDocument = new AttachmentDocument();
            string fileuploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", files.ApplicationNumber.ToString(),files.UserId.ToString());
            if (!System.IO.Directory.Exists(fileuploadDir))
            {
                System.IO.Directory.CreateDirectory(fileuploadDir);
            }
            string fileNameUnique = "";
            string id = Guid.NewGuid().ToString();
            string extention = "";
            string path = "";
            foreach (PropertyInfo prop in files.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                try
                {
                    if (type == typeof(List<IFormFile>))
                    {
                        List<IFormFile> files_ = (List<IFormFile>)prop.GetValue(files, null);
                        if (files_ != null)
                        {
                            foreach (var file in files_)
                            {

                                if (file != null)
                                {
                                    extention = Path.GetExtension(file.FileName).ToLower();

                                    if (!_extentions.Contains(extention))
                                        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this extention is not allowed");

                                    if (file.Length > _maxsize)
                                        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this file size is too big over 20MB");

                                    fileNameUnique = Path.GetFileNameWithoutExtension(file.FileName) + "_" + id + extention;
                                    path = Path.Combine(fileuploadDir, fileNameUnique);
                                    using (Stream stream = new FileStream(path, FileMode.Create))
                                    {
                                        file.CopyTo(stream);
                                    }
                                    if (prop.Name == "OtherRelatedDocuments")
                                        AttachmentDocument.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments == null ? fileNameUnique : AttachmentDocument.OtherRelatedDocuments + ',' + fileNameUnique;


                                }

                            }
                        }
                        }

                }
                catch (Exception EX)
                {

                }
              
                try
                {
                    if (type == typeof(IFormFile))
                    {
                        IFormFile file = (IFormFile)prop.GetValue(files, null);
                        if (file != null)
                        {
                            extention = Path.GetExtension(file.FileName).ToLower();
                            if (!_extentions.Contains(extention.ToLower()))
                                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this extention is not allowed");

                            if (file.Length > _maxsize)
                                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "this file size is too big over 20MB");

                            fileNameUnique = Path.GetFileNameWithoutExtension(file.FileName) + "_" + id + extention;
                            path = Path.Combine(fileuploadDir, fileNameUnique);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            if (prop.Name == "ApprovedLetterForResidencyRenewal")
                                AttachmentDocument.ApprovedLetterForResidencyRenewal = fileNameUnique;

                            if (prop.Name == "CivilIdCopy")
                                AttachmentDocument.CivilIdCopy = fileNameUnique;

                            if (prop.Name == "PassportCopy")
                                AttachmentDocument.PassportCopy = fileNameUnique;

                            if (prop.Name == "OtherRelatedDocuments")
                                AttachmentDocument.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments == null ? fileNameUnique : AttachmentDocument.OtherRelatedDocuments + ',' + fileNameUnique;


                            if (prop.Name == "SalaryCertification")
                                AttachmentDocument.SalaryCertification = fileNameUnique;
                        }
                    }

                }
                catch (Exception EX )
                {

                }
              
            }

            AttachmentDocument.ApplicationNumber = files.ApplicationNumber;
            AttachmentDocument.UserId = files.UserId;
            AttachmentDocument.CreatedDate = DateTime.Now;
            _context.AttachmentDocuments.Add(AttachmentDocument);
            await _context.SaveChangesAsync();

            //Insert data into log table
            AttachmentDocumentLog AD = new AttachmentDocumentLog();
            AD.Id = 0;
            AD.AttachmentDocumentId = AttachmentDocument.Id;
            AD.UserId = AttachmentDocument.UserId;
            AD.ApplicationNumber = AttachmentDocument.ApplicationNumber;
            AD.ApprovedLetterForResidencyRenewal = AttachmentDocument.ApprovedLetterForResidencyRenewal;
            AD.SalaryCertification = AttachmentDocument.SalaryCertification;
            AD.CivilIdCopy = AttachmentDocument.CivilIdCopy;
            AD.PassportCopy = AttachmentDocument.PassportCopy;
            AD.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments;
            AD.Action = "Inserted";
            AD.CreatedDate = DateTime.Now;
            AD.UpdatedDate = DateTime.Now;
            AD.IsActive = AttachmentDocument.IsActive;
            _context.AttachmentDocumentLogs.Add(AD);
            await _context.SaveChangesAsync();
            //

            AttachmentDocument.ApprovedLetterForResidencyRenewal = String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.ApprovedLetterForResidencyRenewal);
            AttachmentDocument.CivilIdCopy = String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.CivilIdCopy);
            AttachmentDocument.PassportCopy = String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.PassportCopy);
            AttachmentDocument.SalaryCertification = String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, AttachmentDocument.SalaryCertification);
            if (!string.IsNullOrEmpty(AttachmentDocument.OtherRelatedDocuments))
            
                {
                var OtherRelatedDocumentsArray = AttachmentDocument.OtherRelatedDocuments.Split(",");
                AttachmentDocument.OtherRelatedDocuments = null;
                if (OtherRelatedDocumentsArray.Length > 0)
                {
                    foreach (var item in OtherRelatedDocumentsArray)
                    {
                        string url = String.Format("{0}://{1}{2}/Uploads/{3}/{4}/{5}", Request.Scheme, Request.Host, Request.PathBase, AttachmentDocument.ApplicationNumber, AttachmentDocument.UserId, item);
                        AttachmentDocument.OtherRelatedDocuments = AttachmentDocument.OtherRelatedDocuments == null ? url : AttachmentDocument.OtherRelatedDocuments + "," + url;

                    }
                }

            }
           

            return CreatedAtAction("GetAttachmentDocument", new { ApplicationNumberId = AttachmentDocument.ApplicationNumber }, AttachmentDocument);
        }

        // DELETE: api/AttachmentDocuments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AttachmentDocument>> DeleteAttachmentDocument(int id)
        {
            var AttachmentDocument = await _context.AttachmentDocuments.FindAsync(id);
            if (AttachmentDocument == null)
            {
                return NotFound();
            }

            _context.AttachmentDocuments.Remove(AttachmentDocument);
            await _context.SaveChangesAsync();

            return AttachmentDocument;
        }


        private bool AttachmentDocumentExists(int id)
        {
            return _context.AttachmentDocuments.Any(e => e.ApplicationNumber == id);
        }
    }
}
