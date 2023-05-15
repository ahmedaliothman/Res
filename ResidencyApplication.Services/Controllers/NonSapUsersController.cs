using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;
using ResidencyApplication.Services.Models.Services;

namespace ResidencyApplication.Services.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NonSapUsersController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;
        private ResponseRequest ResponseRequest_ = new ResponseRequest();
        private IEmailService EmailService_;
        private ISMSHelper _SMSHelper;
        public NonSapUsersController(ResidencyApplicationContext context, IEmailService EmailService,ISMSHelper SMSHelper)
        {
            _context = context;
            EmailService_ = EmailService;
            _SMSHelper = SMSHelper;
        }

        [HttpGet("GetNonSapUser")]
        public async Task<ActionResult> GetNonSapUser(string civilID="",string name="", DateTime fromDate =new DateTime(), DateTime toDate= new DateTime())
        { 
            ResponseRequest_ = new ResponseRequest();
            //Query for a specific customer.
            var App =
                (from a in _context.NonSapUsers
                 join s in _context.Users on a.UserId equals s.UserId
                 where ((civilID!=""&&civilID!=null)? a.CivilId==civilID: a.CivilId!= null)&&((name!=""&&name!= "undefined") ?a.EmployeeName.Contains(name): a.EmployeeName!=null) 
                 select new  {userId=a.UserId, civilID = a.CivilId,phone=s.MobileNumber,email=s.Email,organization=a.Organization,usertypeid=a.UserTypeId }).FirstOrDefault();
            if (App == null)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.Message = "لا يوجد مستخدم بهذه البيانات";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }
            //Return Response
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.HasError = false;
            ResponseRequest_.Response = App;
            return Ok(ResponseRequest_);
        }

        [HttpPost("UpdateNonSapUser")]
        public async Task<ActionResult> UpdateNonSapUser(UserInfoInputObj input)
        {

            ResponseRequest_ = new ResponseRequest();
            //Query for a specific customer.
            var App =
                (from a in _context.NonSapUsers
                 where a.UserId ==Convert.ToInt32(input.userid)
                 select a).FirstOrDefault();
            var App2 =
               (from a in _context.Users
                where a.UserId ==Convert.ToInt32(input.userid)
                select a).FirstOrDefault();
            if (App == null)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.Message = "Bad Request";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }
            App2.Email = input.email!=""?input.email:App2.Email;
            App2.MobileNumber = input.phone!=""?input.phone:App2.MobileNumber;
            App.Organization =Convert.ToInt32(input.organization.ToString());
            App.UserTypeId =Convert.ToInt32(input.usertypeid);

              await _context.SaveChangesAsync();
            //Return Response
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }


        // GET: api/SapUsers
        [HttpGet]
        public async Task<ActionResult> GetNonSapUsersRegistered()
        {
            //var NonSapUsersRegistered = _context.NonSapUsers.Where(r => r.RegistrationStatusId == 1 ).OrderBy(r => r.RegistrationDate).ToList();
            var NonSapUsersRegistered = (from ru in _context.Users 
                                         join r in _context.NonSapUsers on ru.UserId equals r.UserId
                                         join rT in _context.UserTypes on ru.UserTypeId equals rT.UserTypeId
                                         join rO in _context.Organizations on ru.SectionId  equals rO.Id
                                         join rJ in _context.JobTypes on ru.JobtypeId  equals rJ.IdSergate
                                         join rs in _context.RegistrationStatuses on r.RegistrationStatusId equals rs.RegistrationStatusId
                                         where  ru.IsSapUser==false
                                         select new  {ru.UserId,r.RegistrationDate,r.CivilId,r.EmployeeName, RegistrationStatusNameAr=rs.RegistrationStatusNameAr, Organization=rO.Name,r.RegistrationStatusId,rT.UserTypeName,JobName=rJ.Name }).OrderBy(r=>r.RegistrationDate).ToList().Distinct();
            if (NonSapUsersRegistered == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "لا يوجد طلبات استخراج حساب";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = NonSapUsersRegistered;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }


        [HttpPut("{UserId}/{StatusId}")]
        public async Task<IActionResult> UpdateNonSapUserRegistrationStatus(int UserId, int StatusId)
        {
            ResponseRequest_ = new ResponseRequest();
            //Query for a specific customer.
            var App =
                (from a in _context.NonSapUsers
                 where a.UserId == UserId 
                 select a).FirstOrDefault();
            if (App == null)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.Message = "Bad Request";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }
            App.RegistrationStatusId = StatusId;
            var App_ =
               (from a in _context.Users
                where a.UserId == UserId
                select a).FirstOrDefault();
            App_.IsActive = true;
            await _context.SaveChangesAsync();
            //Return Response
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.HasError = false;
            string msg = "";
            if (StatusId == 2)
            { msg = "تم الموافقة على المستخدم الخاص بك فى برنامج الاقامات برجاء تسجيل دخول "; 
            await  EmailService_.sendWithStatus(1, App_.Email, App_.Email, msg, "");
                _SMSHelper.sendWithStatus(1, App_.MobileNumber, App_.Email, msg, "");
            }

            if (StatusId == 3)
            { msg = "تم عدم الموافقة على المستخدم الخاص بك فى برنامج الاقامات برجاء تسجيل دخول "; 
                 await  EmailService_.sendWithStatus(1, App_.Email, App_.Email, msg, "");
                _SMSHelper.sendWithStatus(1, App_.MobileNumber, App_.Email, msg, "");

            }


            return Ok(ResponseRequest_);
        }






    }

    public class UserInfoInputObj {
        public int userid { get; set; }
        public string email { get; set; }
        public string usertypeid { get; set; }
        public string organization { get; set; }
        public string phone { get; set; }

    }
}
