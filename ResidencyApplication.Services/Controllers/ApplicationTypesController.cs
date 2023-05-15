using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;

namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationTypesController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;
        private readonly IMapper _mapper;
        public ResponseRequest ResponseRequest_;
        
        public ApplicationTypesController(ResidencyApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            ResponseRequest_ = new ResponseRequest();
        }

        // GET: api/ApplicationTypes
        [HttpGet]
        public async Task<ActionResult<ResponseRequest<IEnumerable<ApplicationTypesDTO>>>> GetApplicationTypes()
        {
            var res = await _context.ApplicationTypes.ToListAsync();
            var resDTO = _mapper.Map<List<ApplicationTypesDTO>>(res);

            var rtnObj = new ResponseRequest
            {
                HasError = false,
                Message = "Successfull",
                Response = resDTO,
                Status = (int)HttpStatusCode.OK
            };
            return Ok(rtnObj);

        }

        // GET: api/ApplicationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationType>> GetApplicationType(int id)
        {
            var applicationType = await _context.ApplicationTypes.FindAsync(id);

            if (applicationType == null)
            {
                return NotFound();
            }

            return applicationType;
        }

        // PUT: api/ApplicationTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationType(int id, ApplicationType applicationType)
        {
            if (id != applicationType.ApplicationTypeId)
            {
                return BadRequest();
            }

            _context.Entry(applicationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApplicationTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ApplicationType>> PostApplicationType(ApplicationType applicationType)
        {
            _context.ApplicationTypes.Add(applicationType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationTypeExists(applicationType.ApplicationTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationType", new { id = applicationType.ApplicationTypeId }, applicationType);
        }

        // DELETE: api/ApplicationTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationType>> DeleteApplicationType(int id)
        {
            var applicationType = await _context.ApplicationTypes.FindAsync(id);
            if (applicationType == null)
            {
                return NotFound();
            }

            _context.ApplicationTypes.Remove(applicationType);
            await _context.SaveChangesAsync();

            return applicationType;
        }
        [HttpPut("updateIsActive/{id}")]
        public async Task<ActionResult<ResponseRequest<ApplicationTypesDTO>>> updateIsActive(int id,[FromBody] bool isActive)
        {
            var applicationType = await _context.ApplicationTypes.FindAsync(id);
            var rtnObj = new ResponseRequest
            {
                Status = (int)HttpStatusCode.NotFound,
                HasError = true,
                Response = null,
                Message = "No Such Application Type"
            };
            if (applicationType == null)
            {           
                return NotFound(rtnObj);
            }
            else
            {
                applicationType.IsActive = isActive;
                applicationType.UpdatedDate = DateTime.Now;
                _context.Entry(applicationType).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationTypeExists(id))
                    {
                        return NotFound(rtnObj);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            var resDTO = _mapper.Map<ApplicationTypesDTO>(applicationType);
            rtnObj = new ResponseRequest
            {
                Status = (int)HttpStatusCode.OK,
                HasError = false,
                Response = resDTO,
                Message = "تم الحفظ بنجاح"
            };
            return Ok(rtnObj);
        }




        private bool ApplicationTypeExists(int id)
        {
            return _context.ApplicationTypes.Any(e => e.ApplicationTypeId == id);
        }
    }
}
