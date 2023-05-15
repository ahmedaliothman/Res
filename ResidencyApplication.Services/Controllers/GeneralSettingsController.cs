using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;

namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSettingsController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;
        public ResponseRequest ResponseRequest_;
        public ObjGeneralSettings ObjGeneralSettings_;
        public GeneralSettingsController(ResidencyApplicationContext context)
        {
            _context = context;
        }

        // GET: api/GeneralSettings
        [HttpGet]
        public async Task<ActionResult> GetGeneralSettings()
        {
            var res= await _context.GeneralSettings.ToListAsync();
            ObjGeneralSettings_ = new ObjGeneralSettings();
            ResponseRequest_ = new ResponseRequest();

            ObjGeneralSettings_.FeatureId1 =Convert.ToInt32(res.Where(r=>r.FeatureId==1).ToList().FirstOrDefault().Value);
            ObjGeneralSettings_.FeatureId2 = (bool)res.Where(r => r.FeatureId == 2).ToList().FirstOrDefault().IsActive; 
            ObjGeneralSettings_.FeatureId3 = (bool)res.Where(r => r.FeatureId == 3).ToList().FirstOrDefault().IsActive;
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.HasError = false;
            ResponseRequest_.Response = ObjGeneralSettings_;
            return Ok(ResponseRequest_);
        }

        // GET: api/GeneralSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralSetting>> GetGeneralSetting(int id)
        {
            var generalSetting = await _context.GeneralSettings.FindAsync(id);

            if (generalSetting == null)
            {
                return NotFound();
            }

            return generalSetting;
        }

        // PUT: api/GeneralSettings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutGeneralSetting(ObjGeneralSettings generalSetting)
        {
            ResponseRequest_ = new ResponseRequest();
            if (generalSetting==null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }

            var featureId1 = _context.GeneralSettings.Where(r => r.FeatureId == 1).ToList().FirstOrDefault();
            var featureId2 = _context.GeneralSettings.Where(r => r.FeatureId == 2).ToList().FirstOrDefault();
            var featureId3 = _context.GeneralSettings.Where(r => r.FeatureId == 3).ToList().FirstOrDefault();
            featureId1.Value = generalSetting.FeatureId1;
            featureId2.IsActive = generalSetting.FeatureId2;
            featureId3.IsActive = generalSetting.FeatureId3;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        // POST: api/GeneralSettings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GeneralSetting>> PostGeneralSetting(GeneralSetting generalSetting)
        {
            generalSetting.IsActive = true;
            _context.GeneralSettings.Add(generalSetting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeneralSetting", new { id = generalSetting.FeatureId }, generalSetting);
        }

        // DELETE: api/GeneralSettings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeneralSetting>> DeleteGeneralSetting(int id)
        {
            var generalSetting = await _context.GeneralSettings.FindAsync(id);
            if (generalSetting == null)
            {
                return NotFound();
            }

            _context.GeneralSettings.Remove(generalSetting);
            await _context.SaveChangesAsync();

            return generalSetting;
        }

        private bool GeneralSettingExists(int id)
        {
            return _context.GeneralSettings.Any(e => e.FeatureId == id);
        }
    }

    public class ObjGeneralSettings {
        public int FeatureId1 { get; set; }
        public bool FeatureId2 { get; set; }
        public bool FeatureId3 { get; set; }
    }
}
