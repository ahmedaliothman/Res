using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;

namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {

        private readonly ResidencyApplicationContext _context;
        public ResponseRequest ResponseRequest_ = new ResponseRequest();
        private IJwtServices _jwtServices;


        public UserRoleController(IJwtServices jwtServices, ResidencyApplicationContext context)
        {
            _context = context;
            _jwtServices = jwtServices;

        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                var res = _context.UserRoles.ToList();
                if (res == null)
                {
                    ResponseRequest_.Message = "لايوج بيانات";
                    ResponseRequest_.HasError = false;
                    ResponseRequest_.Status = 404;
                    ResponseRequest_.Response = null;
                    return NotFound(ResponseRequest_);

                }

                ResponseRequest_.Message = "";
                ResponseRequest_.HasError = false;
                ResponseRequest_.Status = 200;
                ResponseRequest_.Response = res;
                return Ok(ResponseRequest_);

            }
            catch (Exception ex)
            {
                ResponseRequest_.Message = "يوجد خطاء";
                ResponseRequest_.HasError = true;
                ResponseRequest_.Status = 400;
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);
            }
        }


    }
}