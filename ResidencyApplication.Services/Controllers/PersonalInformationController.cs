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
    public class PersonalInformationController : ControllerBase
    {

        private readonly ResidencyApplicationContext _context;
        public PersonalInformationController(ResidencyApplicationContext context)
        {
            _context = context;
        }

        // GET: api/PersonalInformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalInformation>>> GetPersonalInformations()
        {
            return await _context.PersonalInformations.ToListAsync();
        }

        // GET: api/PersonalInformation/id
        [HttpGet("{ApplicationNumber}")]
        public async Task<ActionResult<PersonalInformation>> GetPersonalInformation(int ApplicationNumber)
        
        {
            var PersonalInformation = await _context.PersonalInformations.Where(r => r.ApplicationNumber == ApplicationNumber).ToListAsync();

            if (PersonalInformation == null)
            {
                return NotFound();
            }

            return PersonalInformation.FirstOrDefault();
        }

        // PUT: api/PersonalInformation/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonalInformation(int id, PersonalInformation personalInformation)
        {
         
           

            try
            {
                personalInformation.UpdatedDate = DateTime.Now;
                _context.Entry(personalInformation).State = EntityState.Modified;
              //  _context.PersonalInformations.Add(personalInformation);

                await _context.SaveChangesAsync();

                var res = _context.Users.Where(r => r.UserId == personalInformation.UserId).FirstOrDefault();
                res.EmployeeNumber = (int)personalInformation.EmployeeNumber;
                res.SectionId = personalInformation.SectionId;
                res.JobtypeId = personalInformation.JobtypeId;
                res.UserTypeId = personalInformation.UserTypeId;
                await _context.SaveChangesAsync();

                if (res.IsSapUser)
                {
                    var sapuser = _context.SapUsers.Where(r => r.UserId == personalInformation.UserId).FirstOrDefault();
                    sapuser.EmployeeNumber = personalInformation.EmployeeNumber.ToString();
                    await _context.SaveChangesAsync();

                }
                else
                {
                    var nonsapuser = _context.NonSapUsers.Where(r => r.UserId == personalInformation.UserId).FirstOrDefault();
                    nonsapuser.EmployeeNumber = personalInformation.EmployeeNumber.ToString();
                    await _context.SaveChangesAsync();

                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalInformationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return CreatedAtAction("GetPersonalInformation", new { ApplicationNumber = personalInformation.ApplicationNumber }, personalInformation);
        }

        // POST: api/PersonalInformation
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PersonalInformation>> PostPersonalInformation(PersonalInformation personalInformation)
        {
            personalInformation.CreatedDate = DateTime.Now;
            _context.PersonalInformations.Add(personalInformation);
            var res = _context.Users.Where(r=>r.UserId==personalInformation.UserId).FirstOrDefault();
            res.EmployeeNumber =(int) personalInformation.EmployeeNumber;
            res.SectionId = personalInformation.SectionId;
            res.JobtypeId = personalInformation.JobtypeId;
            res.UserTypeId = personalInformation.UserTypeId;
            if (res.IsSapUser)
            {
                var sapuser = _context.SapUsers.Where(r => r.UserId == personalInformation.UserId).FirstOrDefault();
                sapuser.EmployeeNumber = personalInformation.EmployeeNumber.ToString();
            }
            else
            {
                var nonsapuser = _context.NonSapUsers.Where(r => r.UserId == personalInformation.UserId).FirstOrDefault();
                nonsapuser.EmployeeNumber = personalInformation.EmployeeNumber.ToString();
            }


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonalInformation", new { ApplicationNumber = personalInformation.ApplicationNumber }, personalInformation);
        }

        // DELETE: api/PersonalInformations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonalInformation>> DeletePersonalInformation(int id)
        {
            var PersonalInformation = await _context.PersonalInformations.FindAsync(id);
            if (PersonalInformation == null)
            {
                return NotFound();
            }

            _context.PersonalInformations.Remove(PersonalInformation);
            await _context.SaveChangesAsync();

            return PersonalInformation;
        }


        private bool PersonalInformationExists(int id)
        {
            return _context.PersonalInformations.Any(e => e.ApplicationNumber == id);
        }




    }
}
