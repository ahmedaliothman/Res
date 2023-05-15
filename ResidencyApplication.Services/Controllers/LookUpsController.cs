using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.CustomReturnTypes;
using AutoMapper;
using ResidencyApplication.Services.Models.jwt;

namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpsController : ControllerBase
    {

        private readonly ResidencyApplicationContext _context;
        private readonly IMapper _mapper;
        public ResponseRequest ResponseRequest_ = new ResponseRequest();

        public LookUpsController(ResidencyApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Nationality
        [HttpGet("Nationality")]
        public async Task<ActionResult> GetNationalities()
        {
            var res = await _context.Nationalities.ToListAsync();
            var resDTO = _mapper.Map<List<LookUpsReturnObj>>(res);
            return Ok(resDTO);
        }

        #endregion
        #region UserRoles
        [HttpGet("UserRoles")]
        public async Task<ActionResult> GetUserRoles()
        {
            var res = await _context.UserRoles.Where(r=>r.UserRoleId!=2).ToListAsync();
            var resDTO = _mapper.Map<List<LookUpsReturnObj>>(res);
            return Ok(resDTO);
        }

        #endregion
        #region UserType
        [HttpGet("UserType")]
        public async Task<ActionResult> GetUserType()
        {
            var res= await _context.UserTypes.ToListAsync();
            var resDTO = _mapper.Map<List<LookUpsReturnObj>>(res);
            return Ok(resDTO);
        }
        #endregion

        [HttpPost("UserType")]
        public  ActionResult PostUserType(LookUpsReturnObj input)
        {
            UserType obj = new UserType();
            try
            {
                obj.UserTypeName = input.label;
                _context.UserTypes.Add(obj);
                _context.SaveChanges();
                ResponseRequest_.Status = 200;
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "تم الحفظ بنجاح";
                ResponseRequest_.Response = null;
                return Ok(ResponseRequest_);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("UNIQUE"))
                {
                    ResponseRequest_.Message = "حدث خطاء هذا النوع موجود بالفعل ";
                }
                else
                {
                    ResponseRequest_.Message = "حدث خطاء فى الحفظ";
                }
                ResponseRequest_.Status = 400;
                ResponseRequest_.HasError = true;
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);
            }
         

        }

        [HttpPut("UserType")]
        public ActionResult PutUserType(LookUpsReturnObj input)
        {
            var res = _context.UserTypes.Where(r=>r.UserTypeId==Convert.ToInt32(input.value)).FirstOrDefault();
            try
            {
                res.UserTypeName = input.label;
                _context.SaveChanges();
                ResponseRequest_.Status = 200;
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "تم الحفظ بنجاح";
                ResponseRequest_.Response = null;
                return Ok(ResponseRequest_);
            }
            catch (Exception ex)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "حدث خطاء فى الحفظ";
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);
            }


        }

        [HttpDelete("UserType")]
        public ActionResult DeleteUserType(int input)
        {
            UserType res = _context.UserTypes.Where(r => r.UserTypeId == Convert.ToInt32(input)).FirstOrDefault();
            try
            {

                _context.UserTypes.Remove(res);
                _context.SaveChanges();
                ResponseRequest_.Status = 200;
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "تم الحذف بنجاح";
                ResponseRequest_.Response = null;
                return Ok(ResponseRequest_);
            }
            catch (Exception ex)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "حدث خطاء فى الحذف";
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);
            }


        }




        #region ApplicationTypes
        [HttpGet("ApplicationTypes")]
        public async Task<ActionResult<IEnumerable<LookUpsReturnObj>>> ApplicationTypes()
        {
            var res = await _context.ApplicationTypes.Where(r=>r.IsActive==true).ToListAsync();
            var resDTO = _mapper.Map<List<LookUpsReturnObj>>(res);
            return Ok(resDTO);
        }

        #endregion


        #region Organization
        [HttpGet("Organization")]
        public async Task<ActionResult<IEnumerable<LookUpsReturnObj>>> OrganizationTypes()
        {
            var res = await _context.Organizations.Where(r => r.Name.Contains("قسم")).ToListAsync();
            var resDTO = _mapper.Map<List<LookUpsReturnObj>>(res);
            return Ok(resDTO);
        }
        #endregion


        #region JobTypes
        [HttpGet("JobTypes")]
        public async Task<ActionResult<IEnumerable<object>>> JobTypes()
        {
            var res = await _context.JobTypes.Select(r=>new {value=r.Id,label=r.Name }).Distinct().ToListAsync();
            return Ok(res);
        }
        #endregion

    }
}
