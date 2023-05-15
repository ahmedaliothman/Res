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
    public class SapUsersController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;

        public SapUsersController(ResidencyApplicationContext context)
        {
            _context = context;
        }

        // GET: api/SapUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SapUser>>> GetSapUsers()
        {
            return await _context.SapUsers.ToListAsync();
        }

        // GET: api/SapUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SapUser>> GetSapUser(int id)
        {
            var sapUser = await _context.SapUsers.FindAsync(id);

            if (sapUser == null)
            {
                return NotFound();
            }

            return sapUser;
        }

        // PUT: api/SapUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSapUser(int id, SapUser sapUser)
        {
            if (id != sapUser.UserId)
            {
                return BadRequest();
            }

            _context.Entry(sapUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SapUserExists(id))
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

        // POST: api/SapUsers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SapUser>> PostSapUser(SapUser sapUser)
        {
            _context.SapUsers.Add(sapUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SapUserExists(sapUser.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSapUser", new { id = sapUser.UserId }, sapUser);
        }

        // DELETE: api/SapUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SapUser>> DeleteSapUser(int id)
        {
            var sapUser = await _context.SapUsers.FindAsync(id);
            if (sapUser == null)
            {
                return NotFound();
            }

            _context.SapUsers.Remove(sapUser);
            await _context.SaveChangesAsync();

            return sapUser;
        }

        private bool SapUserExists(int id)
        {
            return _context.SapUsers.Any(e => e.UserId == id);
        }
    }
}
