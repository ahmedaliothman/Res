using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResidencyApplication.Services.Models.CustomInputTypes;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;

namespace ResidencyApplication.Services.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [EnableCors("all")]
    [ApiController]
    [Route("[controller]")]
    public class JwtController : ControllerBase
    {

        private IJwtServices _jwtServices;
        private commonUserInfo commonUserInfo_;
        private readonly ResidencyApplicationContext _context;
        public ResponseRequest ResponseRequest_;
        public ResponseRequest<AuthenticateResponse> AuthenticateResponse_;
        // private jwtServices _jwtServices1 = new jwtServices();

        public JwtController(IJwtServices jwtServices, ResidencyApplicationContext context)
        {
            _context = context;
            _jwtServices = jwtServices;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        async public Task<IActionResult> register([FromBody] RegisterRequest model)
        {
            ResponseRequest_ = new ResponseRequest();
            if (model.Email.Contains("awqaf.gov.kw"))
            {
                ResponseRequest_.Status = 0;
                ResponseRequest_.Message = "لايمكنك طلب حساب باميل الوزارة";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);

            }
            var ExistSap = _context.SapUsers.Where(r => r.CivilId == model.CivilId);
            var ExistNonSap = _context.NonSapUsers.Where(r => r.CivilId == model.CivilId);
            var ExistUser = _context.Users.Where(r => r.Email == model.Email);
            //var Exists = _context.Users.Where(r => r.Email == model.Email&&r.CivilIdSerialNumber==model.CivilId);
            if (ExistSap.Count() > 0|| ExistUser.Count()>0 )
            {
                ResponseRequest_.Status = 1;
                ResponseRequest_.Message = "هذا الاميل موجود من قبل بالفعل او الرقم المدنى  ";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }

            SpeakerInfoModels user_sap = _jwtServices.GetSpeakerInfoByCivilId_SAP(model.CivilId.ToString());
            if (!string.IsNullOrEmpty( user_sap?.civilid) && user_sap?.civilid!="0")
            {
                ResponseRequest_.Status = 1;
                ResponseRequest_.Message = "هذا الرقم المدنى يوجد لديه حساب على الوزارة يمكنك الدخول وعدم الحاجه لانشاء حساب ";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }

            commonUserInfo user_ = _jwtServices.Register(model);
            var response = user_;
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم انشاء الحساب بنجاح برجاء الانتظار حتى الموافقه علية";
            ResponseRequest_.HasError = false;
            ResponseRequest_.Response = response;
            return Ok(ResponseRequest_);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        async public Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            ResponseRequest_ = new ResponseRequest();
            AuthenticateResponse_ = new ResponseRequest<AuthenticateResponse>();
            AuthenticateResponse_ = await _jwtServices.Authenticate(model, ipAddress());
            if (AuthenticateResponse_.HasError)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.Message = AuthenticateResponse_.Message;
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            setTokenCookie(AuthenticateResponse_.Response.RefreshToken);

            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم التسجيل بنجاح";
            ResponseRequest_.HasError = false;
            ResponseRequest_.Response = AuthenticateResponse_.Response;
            return Ok(ResponseRequest_);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        async public Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _jwtServices.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }


        [HttpPost("revoke-token")]
        async public Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _jwtServices.RevokeToken(token, ipAddress());

            if (response == null)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        // Password update Api
        [AllowAnonymous]
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody]ChangePassword changePasswordObj)
        {
            int UserId = changePasswordObj.UserId;
            string OldPassword = changePasswordObj.OldPassword;
            string NewPassword = changePasswordObj.NewPassword;

            var UserExist = _context.NonSapUsers.Where(r1 => r1.UserId == UserId);
            var ValidOldPassword = _context.NonSapUsers.Where(r => r.Password == OldPassword && r.UserId == UserId);
            ResponseRequest_ = new ResponseRequest();
            if (UserExist.Count() == 0)
            {
                ResponseRequest_.Status = 204;
                ResponseRequest_.Message = "User not found";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }
            else if (ValidOldPassword.Count() == 0)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.Message = "Old password not correct";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }
            else
            {

                var result = (from a in _context.NonSapUsers
                              where a.UserId == changePasswordObj.UserId && a.Password == OldPassword
                              select a).FirstOrDefault();
                if (result == null)
                {
                    ResponseRequest_.Status = 204;
                    ResponseRequest_.Message = "User not found";
                    ResponseRequest_.HasError = true;
                    return BadRequest(ResponseRequest_);
                }
                result.Password = NewPassword;
                await _context.SaveChangesAsync();
            }
            ResponseRequest_.Status =200;
            ResponseRequest_.Message = "Password updated successfully!";
            return Ok(ResponseRequest_);
        }

    }
}
