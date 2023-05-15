using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;


namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassportInformationController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;
        public PassportInformationController(ResidencyApplicationContext context)
        {
            _context = context;
        }

        // GET: api/PassportInformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassportInformation>>> GetPassportInformations()
        {
            return await _context.PassportInformations.ToListAsync();
        }

        // GET: api/PassportInformation/id
        [HttpGet("{ApplicationNumber}")]
        public async Task<ActionResult<PassportInformation>> GetPassportInformation(int ApplicationNumber)
        {
            var passportInformation = await _context.PassportInformations.Where(r => r.ApplicationNumber == ApplicationNumber).ToListAsync();

            if (passportInformation == null)
            {
                return NotFound();
            }

            return passportInformation.FirstOrDefault();
        }

        // PUT: api/PassportInformation/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassportInformation(int id, PassportInformation passportInformation)
        {
            passportInformation.UpdatedDate = DateTime.Now;
            _context.Entry(passportInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                //Insert data into log table
                var lastinsertedId = passportInformation.UserId;
                PassportInformationLog PI = new PassportInformationLog();
                PI.PassportInformationLogId = 0;
                PI.Id  = passportInformation.Id;
                PI.CivilId = passportInformation.CivilId;
                PI.NationalityId = passportInformation.NationalityId;
                PI.PassportNumber = passportInformation.PassportNumber;
                PI.IssueCountry = passportInformation.IssueCountry;
                PI.IssueDate = passportInformation.IssueDate;
                PI.ExpiryDate = passportInformation.ExpiryDate;
                PI.Address = passportInformation.Address;
                PI.ResidencyExpiryDate = passportInformation.ResidencyExpiryDate;
                PI.ApplicationNumber = passportInformation.ApplicationNumber;
                PI.UserId = passportInformation.UserId;
                PI.Action = "Updated";
                PI.CreatedDate = DateTime.Now;
                PI.UpdatedDate = DateTime.Now;
                PI.IsActive = passportInformation.IsActive;

                _context.PassportInformationLogs.Add(PI);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassportInformationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetPassportInformation", new { ApplicationNumber = passportInformation.ApplicationNumber }, passportInformation);
        }

        // POST: api/PassportInformation
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PassportInformation>> PostPassportInformation(PassportInformation passportInformation)
        {
            passportInformation.CreatedDate = DateTime.Now;
            _context.PassportInformations.Add(passportInformation);
            await _context.SaveChangesAsync();

            //Insert data into log table
            var lastinsertedId = passportInformation.UserId;
            PassportInformationLog PI = new PassportInformationLog();
            PI.PassportInformationLogId = 0;
            PI.Id = passportInformation.Id;
            PI.CivilId = passportInformation.CivilId;
            PI.NationalityId = passportInformation.NationalityId;
            PI.PassportNumber = passportInformation.PassportNumber;
            PI.IssueCountry = passportInformation.IssueCountry;
            PI.IssueDate = passportInformation.IssueDate;
            PI.ExpiryDate = passportInformation.ExpiryDate;
            PI.Address = passportInformation.Address;
            PI.ResidencyExpiryDate = passportInformation.ResidencyExpiryDate;
            PI.ApplicationNumber = passportInformation.ApplicationNumber;
            PI.UserId = passportInformation.UserId;
            PI.Action = "Inserted";
            PI.CreatedDate = DateTime.Now;
            PI.UpdatedDate = DateTime.Now;
            PI.IsActive = passportInformation.IsActive;
            _context.PassportInformationLogs.Add(PI);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassportInformation", new { ApplicationNumber = passportInformation.ApplicationNumber }, passportInformation);
        }

        // DELETE: api/PassportInformations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PassportInformation>> DeletePassportInformation(int id)
        {
            var PassportInformation = await _context.PassportInformations.FindAsync(id);
            if (PassportInformation == null)
            {
                return NotFound();
            }

            _context.PassportInformations.Remove(PassportInformation);
            await _context.SaveChangesAsync();

            return PassportInformation;
        }

        private bool PassportInformationExists(int id)
        {
            return _context.PassportInformations.Any(e => e.ApplicationNumber == id);
        }
    }
}
