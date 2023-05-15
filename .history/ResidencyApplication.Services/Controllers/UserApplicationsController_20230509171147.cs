using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;
using ResidencyApplication.Services.Models.Services;

namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApplicationsController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ISMSHelper _SMSHelper;
        public ResponseRequest ResponseRequest_ = new ResponseRequest();
        private IEmailService EmailService_;
        public UserApplicationsController(ResidencyApplicationContext context, IMapper mapper, IEmailService EmailService,ISMSHelper SMSHelper)
        {
            _context = context;
            //_context.ChangeTracker.LazyLoadingEnabled = false;
            _mapper = mapper;
            EmailService_ = EmailService;
            _SMSHelper = SMSHelper;

        }

        // GET: api/UserApplications
        [HttpGet]
        public async Task<ActionResult> GetUserApplication()
        {
            ResponseRequest_ = new ResponseRequest();
            var res = await _context.UserApplications.ToListAsync();
            var resDTO = _mapper.Map<List<UserApplicationsDTO>>(res);
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = resDTO;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }



        [HttpGet("GetV_CountAssignedApplications")]
        public async Task<ActionResult> GetV_CountAssignedApplications() {

            ResponseRequest_ = new ResponseRequest();
            var res = await _context.VCountAssignedApplications.ToListAsync();
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = res;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }
        // GET: api/UserApplication/id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserApplication(int id)
        {
            ResponseRequest_ = new ResponseRequest();
            var UserApplication = await _context.UserApplications.FindAsync(id);

            if (UserApplication == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "Not found";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }

            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = UserApplication;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }
        // GET: api/UserId/id
        [HttpGet("UserId/{UserId}")]
        public ActionResult GetUserApplicationsByUserId(int UserId)
        {
            ResponseRequest_ = new ResponseRequest();
            var UserApplication = from uapp in _context.UserApplications
                                  join appt in _context.ApplicationTypes
                                  on uapp.ApplicationTypeId equals appt.ApplicationTypeId
                                  join apps in _context.ApplicationStatuses
                                  on uapp.ApplicationStatusId equals apps.ApplicationStatusId
                                  join uapps in _context.UserApplicationSteps
                                  on uapp.StepNo equals uapps.StepNo
                                  where uapp.UserId == UserId
                                  select new
                                  {
                                      uapp.ApplicationNumber,
                                      uapp.ApplicationDate,
                                      uapp.Remark,
                                      uapp.ApplicationStatusId,
                                      uapp.ApplicationTypeId,
                                      uapp.StepNo,
                                      uapp.IsActive,
                                      uapp.UserId,
                                      appt.ApplicationTypeName,
                                      apps.ApplicationStatusName,
                                      uapps.StepName
                                  };



            if (UserApplication == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "Not found";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = UserApplication;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        [HttpGet("Admin")]
        public ActionResult GetUserApplicationsAdmin()
        {
            ResponseRequest_ = new ResponseRequest();
            var UserApplication = from uapp in _context.UserApplications
                                  join appt in _context.ApplicationTypes on uapp.ApplicationTypeId equals appt.ApplicationTypeId
                                  join apps in _context.ApplicationStatuses on uapp.ApplicationStatusId equals apps.ApplicationStatusId
                                  join uapps in _context.UserApplicationSteps on uapp.StepNo equals uapps.StepNo

                                 select new
                                  {
                                      uapp.ApplicationNumber,
                                      uapp.ApplicationDate,
                                      uapp.Remark,
                                      uapp.ApplicationStatusId,
                                      uapp.ApplicationTypeId,
                                      uapp.StepNo,
                                      uapp.IsActive,
                                      uapp.UserId,
                                      appt.ApplicationTypeName,
                                      apps.ApplicationStatusName,
                                      uapps.StepName,
                                      CivilId ="",
                                      UserName = ""
                                  };



            if (UserApplication == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "Not found";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = UserApplication;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        [HttpGet("List")]
        public async Task<ActionResult> GetApplicationList(String Type,int UserId)
        {
            var UserApplication =new object();
            UserApplication = null;
            var currentUser = _context.Users.Find(UserId);
            if (Type == "inward")
            {
                 UserApplication = (from a in _context.UserApplications
                                      join b in _context.ApplicationTypes on a.ApplicationTypeId equals b.ApplicationTypeId into ab
                                      from b in ab.DefaultIfEmpty() // <== makes join left join
                                      join c in _context.ApplicationStatuses on a.ApplicationStatusId equals c.ApplicationStatusId into ac
                                      from c in ac.DefaultIfEmpty()
                                      join d in _context.UserApplicationSteps on a.StepNo equals d.StepNo into ad
                                      from d in ad.DefaultIfEmpty()
                                      join e in _context.SapUsers on a.UserId equals e.UserId into ae
                                      from e in ae.DefaultIfEmpty()
                                      join f in _context.NonSapUsers on a.UserId equals f.UserId into af
                                      from f in af.DefaultIfEmpty()
                                      join g in _context.Users on a.UserId equals g.UserId into ag
                                      from g in ag.DefaultIfEmpty()
                                   join p in _context.PersonalInformations on a.ApplicationNumber equals p.ApplicationNumber into pa
                                   from p in pa.DefaultIfEmpty()
                                   join pass in _context.PassportInformations on a.ApplicationNumber equals pass.ApplicationNumber into passa
                                   from pass in passa.DefaultIfEmpty()
                                   join n in _context.Nationalities on g.NationalityId equals n.NationalityId into ng
                                   from n in ng.DefaultIfEmpty()
                                   join k in _context.Users on a.AssignedTo equals k.UserId into ka 
                                   from k in ka.DefaultIfEmpty()
                                    join k1 in _context.SapUsers on k.UserId equals k1.UserId into kk1
                                    from k1 in kk1.DefaultIfEmpty()
                                    join k2 in _context.NonSapUsers on k.UserId equals k2.UserId into kk2
                                    from k2 in kk2.DefaultIfEmpty()
                                    join gg in _context.Genders on g.GenderId equals gg.Id into ggg
                                    from gg in ggg.DefaultIfEmpty()
                                    where a.ApplicationStatusId == 5 && (currentUser.UserRoleId != 1 ? a.AssignedTo == Convert.ToInt32(UserId) : a.AssignedTo == a.AssignedTo)  // <== where applications is pending status
                                    select new
                                      {
                                         
                                          a.ApplicationNumber,
                                          a.ApplicationDate,
                                          a.Remark,
                                          a.ApplicationStatusId,
                                          a.ApplicationTypeId,
                                          a.StepNo,
                                          a.IsActive,
                                          a.UserId,
                                          b.ApplicationTypeName,
                                          c.ApplicationStatusName,
                                          d.StepName,
                                          n.NationalityName,
                                          pass.PassportNumber,
                                          pass.IssueDate,
                                          pass.ExpiryDate,
                                          p.JobTitle,
                                          p.BirthDate,
                                          CivilId = g.IsSapUser == true ? e.CivilId : f.CivilId,
                                          UserName = g.IsSapUser == true ? e.EmployeeName : f.EmployeeName,
                                        EnglishName = p.EmployeeNameEnglish,
                                        assignTo =  k.IsSapUser==true ?k1.EmployeeName:k2.EmployeeName,
                                         keyas= a.AssignedTo,
                                         Gender=g.GenderId,
                                         GenderName=gg.Name
                                    }).Distinct().OrderBy(r=>r.keyas.HasValue).ThenBy(r=>r.keyas);
                
            }
            else if (Type == "outward")
            {
                UserApplication = (from a in _context.UserApplications
                                  join b in _context.ApplicationTypes on a.ApplicationTypeId equals b.ApplicationTypeId into ab
                                  from b in ab.DefaultIfEmpty() // <== makes join left join
                                  join c in _context.ApplicationStatuses on a.ApplicationStatusId equals c.ApplicationStatusId into ac
                                  from c in ac.DefaultIfEmpty()
                                  join d in _context.UserApplicationSteps on a.StepNo equals d.StepNo into ad
                                  from d in ad.DefaultIfEmpty()
                                  join e in _context.SapUsers on a.UserId equals e.UserId into ae
                                  from e in ae.DefaultIfEmpty()
                                  join f in _context.NonSapUsers on a.UserId equals f.UserId into af
                                  from f in af.DefaultIfEmpty()
                                  join g in _context.Users on a.UserId equals g.UserId into ag
                                  from g in ag.DefaultIfEmpty()
                                  join p in _context.PersonalInformations on a.ApplicationNumber equals p.ApplicationNumber into pa
                                  from p in pa.DefaultIfEmpty()
                                  join pass in _context.PassportInformations on a.ApplicationNumber equals pass.ApplicationNumber into passa
                                  from pass in passa.DefaultIfEmpty()
                                  join n in _context.Nationalities on g.NationalityId equals n.NationalityId into ng
                                  from n in ng.DefaultIfEmpty()
                                   join k in _context.Users on a.AssignedTo equals k.UserId into ka
                                   from k in ka.DefaultIfEmpty()
                                   join k1 in _context.SapUsers on k.UserId equals k1.UserId into kk1
                                   from k1 in kk1.DefaultIfEmpty()
                                   join k2 in _context.NonSapUsers on k.UserId equals k2.UserId into kk2
                                   from k2 in kk2.DefaultIfEmpty()
                                   join gg in _context.Genders on g.GenderId equals gg.Id into ggg
                                   from gg in ggg.DefaultIfEmpty()
                                   where a.ApplicationStatusId != 5 && a.ApplicationStatusId != 6 &&  (currentUser.UserRoleId != 1 ? a.AssignedTo == Convert.ToInt32(UserId) : a.AssignedTo == a.AssignedTo)  // <== where applications is pending status
                                   select new
                                  {
                                      a.ApplicationNumber,
                                      a.ApplicationDate,
                                      a.Remark,
                                      a.ApplicationStatusId,
                                      a.ApplicationTypeId,
                                      a.StepNo,
                                      a.IsActive,
                                      a.UserId,
                                      b.ApplicationTypeName,
                                      c.ApplicationStatusName,
                                      d.StepName,
                                      n.NationalityName,
                                      pass.PassportNumber,
                                      pass.IssueDate,
                                      pass.ExpiryDate,
                                      p.JobTitle,
                                      p.BirthDate,
                                      CivilId = g.IsSapUser == true ? e.CivilId : f.CivilId,
                                      UserName = g.IsSapUser == true ? e.EmployeeName : f.EmployeeName,
                                      EnglishName = p.EmployeeNameEnglish,
                                       assignTo = k.IsSapUser == true ? k1.EmployeeName : k2.EmployeeName,
                                       Gender = g.GenderId,
                                       GenderName = gg.Name

                                   }).Distinct();
            }
          

            if (UserApplication == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "Not found";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = UserApplication;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }


        [HttpGet("SearchList")]
        public ActionResult GetSearchApplicationList(int ApplicationNumber, int ApplicationTypeId, int UserTypeId, string CivilId, int ApplicationStatusId, string EmployeeName, string SubmittedBy, string EmployeeNumber,string AssignedTo, DateTime FromDate, DateTime ToDate)
        {
            var currentUser = _context.Users.Find(Convert.ToInt32(AssignedTo));

            ResponseRequest_ = new ResponseRequest();
            FromDate = FromDate == DateTime.MinValue ? new DateTime(1980, 1, 1) : FromDate;
            ToDate = ToDate == DateTime.MinValue ? new DateTime(2050, 1, 1) : ToDate;
            var UserApplication = (from a in _context.UserApplications
                                  join b in _context.ApplicationTypes on a.ApplicationTypeId equals b.ApplicationTypeId into ab
                                  from b in ab.DefaultIfEmpty() // <== makes join left join
                                  join c in _context.ApplicationStatuses on a.ApplicationStatusId equals c.ApplicationStatusId into ac
                                  from c in ac.DefaultIfEmpty()
                                  join d in _context.UserApplicationSteps on a.StepNo equals d.StepNo into ad
                                  from d in ad.DefaultIfEmpty()
                                  join e in _context.SapUsers on a.UserId equals e.UserId into ae
                                  from e in ae.DefaultIfEmpty()
                                  join f in _context.NonSapUsers on a.UserId equals f.UserId into af
                                  from f in af.DefaultIfEmpty()
                                  join g in _context.Users on a.UserId equals g.UserId into ag
                                  from g in ag.DefaultIfEmpty()
                                  join p in _context.PersonalInformations on  a.ApplicationNumber  equals    p.ApplicationNumber  into pa
                                  from p in pa.DefaultIfEmpty()
                                  join pass in _context.PassportInformations on  a.ApplicationNumber  equals  pass.ApplicationNumber  into passa
                                  from pass in passa.DefaultIfEmpty()
                                  join n in _context.Nationalities on g.NationalityId equals n.NationalityId into ng
                                  from n in ng.DefaultIfEmpty()
                                  join o in _context.SapUsers on a.SubmittedBy equals o.UserId into ao
                                  from o in ao.DefaultIfEmpty()
                                  join q in _context.NonSapUsers on a.SubmittedBy equals q.UserId into aq
                                  from q in aq.DefaultIfEmpty()
                                  join m in _context.Users on a.SubmittedBy equals m.UserId into am
                                  from m in am.DefaultIfEmpty()
                                  join y in _context.UserTypes on g.UserTypeId equals y.UserTypeId into gy
                                  from y in gy.DefaultIfEmpty()
                                  join gg in _context.Genders on g.GenderId equals gg.Id into ggg
                                  from gg in ggg.DefaultIfEmpty()
                                  join oo in _context.Organizations on g.SectionId equals oo.Id into oog
                                  from oo in oog.DefaultIfEmpty()
                                  join jj in _context.JobTypes on g.JobtypeId equals jj.Id into jjg
                                  from jj in jjg.DefaultIfEmpty()
                                   
                                   where (ApplicationNumber!=0 ? a.ApplicationNumber == ApplicationNumber: a.ApplicationNumber!=0)  &&
                                  (ApplicationTypeId!=0? a.ApplicationTypeId == ApplicationTypeId: a.ApplicationTypeId!=0) && 
                                  (UserTypeId!=0? g.UserTypeId == UserTypeId: g.UserTypeId!= 0) &&
                                  (ApplicationStatusId!=0? a.ApplicationStatusId == ApplicationStatusId : a.ApplicationStatusId!=0) && 
                                  (a.ApplicationDate >= FromDate && a.ApplicationDate <= ToDate )&&
                                   (g.IsSapUser == true ? ((CivilId != "0" && CivilId != null) ? e.CivilId == CivilId : e.CivilId != "") : (((CivilId != "0" && CivilId != null) ? f.CivilId == CivilId : f.CivilId != ""))) &&
                                  (g.IsSapUser == true ? (EmployeeName!= null?e.EmployeeName.Contains(EmployeeName): e.EmployeeName!="") : (EmployeeName!=null?f.EmployeeName.Contains(EmployeeName): f.EmployeeName!=""))&&
                                  (g.IsSapUser == true ? (EmployeeNumber!= null?e.EmployeeNumber== EmployeeNumber : e.EmployeeNumber == e.EmployeeNumber) : (EmployeeNumber != null?f.EmployeeNumber== EmployeeNumber : f.EmployeeNumber == f.EmployeeNumber))&&
                                  (m.IsSapUser == true ? (SubmittedBy != null?o.EmployeeName.Contains(SubmittedBy) : o.EmployeeName != "") : (SubmittedBy != null?q.EmployeeName.Contains(SubmittedBy) : q.EmployeeName != ""))&&
                                  (currentUser.UserRoleId!=1?a.AssignedTo==currentUser.UserId : a.AssignedTo== a.AssignedTo)
                                   select new 
                                  {
                                      a.ApplicationNumber,
                                      a.ApplicationDate,
                                      a.Remark,
                                      a.ApplicationStatusId,
                                      a.ApplicationTypeId,
                                      a.StepNo,
                                      a.IsActive,
                                      a.UserId,
                                      b.ApplicationTypeName,
                                      c.ApplicationStatusName,
                                      d.StepName,
                                      n.NationalityName,
                                      pass.PassportNumber,
                                      pass.IssueDate,
                                      pass.ExpiryDate,
                                      p.JobTitle,
                                      p.BirthDate,
                                      CivilId = g.IsSapUser == true ? e.CivilId : f.CivilId,
                                      UserName = g.IsSapUser == true ? e.EmployeeName : f.EmployeeName,
                                      EnglishName=p.EmployeeNameEnglish,
                                      submittedBy = m.IsSapUser == true ? o.EmployeeName : q.EmployeeName,
                                       Gender = g.GenderId,
                                       GenderName = gg.Name,
                                       SectionName=oo.Name,
                                       JobName=jj.Name,
                                       UserTypeName=y.UserTypeName
                                   }).Distinct();



            if (UserApplication == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "Not found";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = UserApplication;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        [HttpGet("GetActiveUserApplicationByUserID/{userId}")]
        public ActionResult GetActiveUserApplicationByUserID(int userId)
        {
            ResponseRequest_ = new ResponseRequest();
            var UserApplication = _context.UserApplications.Where(
                 m => m.UserId == userId && m.ApplicationStatusId == 6).OrderBy(r => r.ApplicationNumber).FirstOrDefault();

            if (UserApplication == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "";
                ResponseRequest_.HasError = false;
                return NotFound(ResponseRequest_);
            }

            var resDTO = _mapper.Map<UserApplicationsDTO>(UserApplication);
   
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = resDTO;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        //Get Application details
        [HttpGet("GetApplicationDetail/{ApplicationNumber}")]
        public ActionResult GetApplicationDetail(int ApplicationNumber)
        {
            ResponseRequest_ = new ResponseRequest();
            var AppUserType = (from UA in _context.UserApplications
                               join U in _context.Users on UA.UserId equals U.UserId
                               where UA.ApplicationNumber == ApplicationNumber
                               select new UsersDTO
                               {
                                
                                   IsSapUser = U.IsSapUser
                               }).FirstOrDefault();
            var IsSapUser = AppUserType.IsSapUser;

            if (IsSapUser == true)
            {
                var AppDetails = (from UA in _context.UserApplications
                                  join AS in _context.ApplicationStatuses on UA.ApplicationStatusId equals AS.ApplicationStatusId
                                  join AT in _context.ApplicationTypes on UA.ApplicationTypeId equals AT.ApplicationTypeId
                                  join U in _context.SapUsers on UA.UserId equals U.UserId
                                  join US in _context.UserApplicationSteps on UA.StepNo equals US.StepNo
                                  where UA.ApplicationNumber == ApplicationNumber
                                  select new UserApplicationsDetailedDTO
                                  {
                                      ApplicationNumber = UA.ApplicationNumber,
                                      EmployeeName = U.EmployeeName,
                                      CivilId = U.CivilId,
                                      ApplicationStatusId = AS.ApplicationStatusId,
                                      ApplicationStatusName = AS.ApplicationStatusName,
                                      ApplicationTypeId = AT.ApplicationTypeId,
                                      ApplicationTypeName = AT.ApplicationTypeName,
                                      StepNo = US.StepNo,
                                      StepName = US.StepName,
                                      Remark = UA.Remark,
                                      IsActive = UA.IsActive,
                                      ApplicationDate = (DateTime)UA.ApplicationDate
                                  }).FirstOrDefault();
                if (AppDetails == null)
                {
                    ResponseRequest_.Status = 404;
                    ResponseRequest_.Message = "Not found";
                    ResponseRequest_.HasError = true;
                    return NotFound(ResponseRequest_);
                }

                var resDTO = _mapper.Map<UserApplicationsDetailedDTO>(AppDetails);

                ResponseRequest_.Status = 200;
                ResponseRequest_.Message = "";
                ResponseRequest_.Response = resDTO;
                ResponseRequest_.HasError = false;
                return Ok(ResponseRequest_);
            }
            else
            {
                var AppDetails = (from UA in _context.UserApplications
                                  join AS in _context.ApplicationStatuses on UA.ApplicationStatusId equals AS.ApplicationStatusId
                                  join AT in _context.ApplicationTypes on UA.ApplicationTypeId equals AT.ApplicationTypeId
                                  join U in _context.NonSapUsers on UA.UserId equals U.UserId
                                  join US in _context.UserApplicationSteps on UA.StepNo equals US.StepNo
                                  where UA.ApplicationNumber == ApplicationNumber
                                  select new UserApplicationsDetailedDTO
                                  {
                                      ApplicationNumber = UA.ApplicationNumber,
                                      EmployeeName = U.EmployeeName,
                                      CivilId = U.CivilId,
                                      ApplicationStatusId = AS.ApplicationStatusId,
                                      ApplicationStatusName = AS.ApplicationStatusName,
                                      ApplicationTypeName = AT.ApplicationTypeName,
                                      StepNo = US.StepNo,
                                      StepName = US.StepName,
                                      Remark = UA.Remark,
                                      IsActive = UA.IsActive,
                                      ApplicationDate = (DateTime)UA.ApplicationDate
                                  }).FirstOrDefault();
                if (AppDetails == null)
                {
                    ResponseRequest_.Status = 404;
                    ResponseRequest_.Message = "Not found";
                    ResponseRequest_.HasError = true;
                    return NotFound(ResponseRequest_);
                }
                var resDTO = _mapper.Map<UserApplicationsDetailedDTO>(AppDetails);
                 // Return Data
                ResponseRequest_.Status = 200;
                ResponseRequest_.Message = "Application Details";
                ResponseRequest_.Response = resDTO;
                ResponseRequest_.HasError = false;
                return Ok(ResponseRequest_);
            }
        }

        // PUT: api/UserApplication/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserApplicationsDTO userapplication)
        {

            ResponseRequest_ = new ResponseRequest();
            userapplication.Remark = "جاري العمل على المعاملة عزيزى الموظف";
            _context.Entry(_mapper.Map<UserApplication>(userapplication)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                //sending email 
                // if (userapplication.ApplicationStatusId == 5&& userapplication.StepNo==5)
                // {
                //     //var res = _context.Users.Where(r => r.UserId == userapplication.UserId).FirstOrDefault();
                //     var resView = _context.VUserApplications.Where(r => r.ApplicationNumber == userapplication.ApplicationNumber).FirstOrDefault();
                //     var s = Request.Host.Value;
                //     string appName = _context.ApplicationTypes.Find(userapplication.ApplicationTypeId).ApplicationTypeName;
                //     await EmailService_.sendWithStatus(5, resView.Email, resView.EmployeeNameArabic, userapplication.Remark,appName);
                //     _SMSHelper.sendWithStatus(5, resView.MobileNumber, resView.EmployeeNameArabic, userapplication.Remark, appName);

                // }

                //Insert data into log table
                var lastinsertedId = userapplication.ApplicationNumber;
                UserApplicationsLog UserApplicationsLog = new UserApplicationsLog();
                UserApplicationsLog.ApplicationNumberLogId = 0;
                UserApplicationsLog.ApplicationNumber = lastinsertedId;
                UserApplicationsLog.UserId = userapplication.UserId;
                UserApplicationsLog.ApplicationStatusId = userapplication.ApplicationStatusId;
                UserApplicationsLog.ApplicationTypeId = userapplication.ApplicationTypeId;
                UserApplicationsLog.ApplicationDate = userapplication.ApplicationDate;
                UserApplicationsLog.Remark = userapplication.Remark;
                UserApplicationsLog.StepNo = userapplication.StepNo;
                UserApplicationsLog.CreatedDate = DateTime.Now;
                UserApplicationsLog.UpdatedDate = DateTime.Now;
                UserApplicationsLog.Action = "Updated";
                UserApplicationsLog.IsActive = userapplication.IsActive;
                _context.UserApplicationsLogs.Add(UserApplicationsLog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserApplicationExists(id))
                {
                    ResponseRequest_.Status = 404;
                    ResponseRequest_.Message = "Not found";
                    ResponseRequest_.HasError = true;
                    return NotFound(ResponseRequest_);
                }
                else
                {
                    throw;
                }
            }
            // Return Data
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.Response = CreatedAtAction("GetUserApplication", new { id = userapplication.ApplicationNumber }, userapplication);
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        // POST: api/UserApplications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserApplication>> PostUserApplication(UserApplicationsDTO userapplication)
        {
            ResponseRequest_ = new ResponseRequest();
            try
            {
                ResponseRequest_ = new ResponseRequest();
                var Days = _context.GeneralSettings.Where(r => r.FeatureId == 1).FirstOrDefault().Value;
                var OldUserApplication = _context.UserApplications.Where(
                 m => m.UserId == userapplication.UserId && m.ApplicationTypeId == userapplication.ApplicationTypeId).OrderByDescending(r => r.ApplicationNumber).FirstOrDefault();

                if (OldUserApplication != null)
                {
                    int UDayes = (DateTime.Now-OldUserApplication.ApplicationDate ).Value.Days;
                    if (UDayes <=Days)
                    {
                        ResponseRequest_.Status = 404;
                        ResponseRequest_.Message = "لا يمكن طلب نفس نوع الطلب مرتين ";
                        ResponseRequest_.HasError = true;
                        return NotFound(ResponseRequest_);

                    }
                    
                }

                
                UserApplication UserApplication_ = _mapper.Map<UserApplication>(userapplication);
                UserApplication_.Remark = "جاري العمل على المعاملة عزيزى الموظف";
                _context.UserApplications.Add(UserApplication_);
                await _context.SaveChangesAsync();

                ///Insert data into log table
                var lastinsertedId = UserApplication_.ApplicationNumber;
                UserApplicationsLog UserApplicationsLog = new UserApplicationsLog();
                UserApplicationsLog.ApplicationNumberLogId = 0;
                UserApplicationsLog.ApplicationNumber = lastinsertedId;
                UserApplicationsLog.UserId = userapplication.UserId;
                UserApplicationsLog.ApplicationStatusId = userapplication.ApplicationStatusId;
                UserApplicationsLog.ApplicationTypeId = userapplication.ApplicationTypeId;
                UserApplicationsLog.ApplicationDate = userapplication.ApplicationDate;
                UserApplicationsLog.Remark = userapplication.Remark;
                UserApplicationsLog.StepNo = userapplication.StepNo;
                UserApplicationsLog.CreatedDate = DateTime.Now;
                UserApplicationsLog.IsActive = userapplication.IsActive;
                UserApplicationsLog.Action = "Inserted";
                _context.UserApplicationsLogs.Add(UserApplicationsLog);
                await _context.SaveChangesAsync();
                // Return Data
                ResponseRequest_.Status = 200;
                ResponseRequest_.Message = "تم الحفظ بنجاح";
                ResponseRequest_.Response = CreatedAtAction("GetUserApplication", new { id = UserApplication_.ApplicationNumber }, UserApplication_);
                ResponseRequest_.HasError = false;
                return Ok(ResponseRequest_);
            }
            catch (DbUpdateConcurrencyException)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "Not found";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
        }

        // DELETE: api/UserApplications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserApplication>> DeleteUserApplication(int id)
        {
            ResponseRequest_ = new ResponseRequest();
            var userapplication = await _context.UserApplications.FindAsync(id);
            if (userapplication == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "Not found";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }

            _context.UserApplications.Remove(userapplication);
            await _context.SaveChangesAsync();
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "Deleted Successfully!";
            ResponseRequest_.Response = this.RedirectToAction("GetUserApplicationsByUserId/UserId", new { UserId = userapplication.UserId });
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }


        private bool UserApplicationExists(int id)
        {
            return _context.UserApplications.Any(e => e.ApplicationNumber == id);
        }

        // PUT: api/UserApplication/status update
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{UserId}/{ApplicationNumber}/{StatusId}")]
        public async Task<IActionResult> UpdateApplicationStatus(int UserId, int ApplicationNumber, int StatusId,string? Remark="", int? SubmittedBy = 0)
        {
            ResponseRequest_ = new ResponseRequest();
            //Query for a specific customer.
            var App =
                (from a in _context.UserApplications
                 where a.UserId == UserId && a.ApplicationNumber == ApplicationNumber
                 select a).FirstOrDefault();
            if (App == null)
            {
                ResponseRequest_.Status = 400;
                ResponseRequest_.Message = "Bad Request";
                ResponseRequest_.HasError = true;
                return BadRequest(ResponseRequest_);
            }
            App.ApplicationStatusId = StatusId;
           
            //email send 

            if (StatusId == 1 || StatusId == 2 || StatusId == 3)
            { 
                App.SubmittedBy = SubmittedBy;
                App.Remark = Remark + "-"+ DateTime.Now.ToString("dddd dd MMMM yyyy", new CultureInfo("ar-AE")) ;

            }
            if (App.AssignedTo == null || App.SubmittedBy != SubmittedBy)
            {
                App.AssignedTo = SubmittedBy;
            }
            await _context.SaveChangesAsync();

                //email send 
               // var res = _context.Users.Where(r => r.UserId == UserId).FirstOrDefault();
                var resView = _context.VUserApplications.Where(r => r.ApplicationNumber == App.ApplicationNumber).FirstOrDefault();
                string appName = _context.ApplicationTypes.Find(App.ApplicationTypeId).ApplicationTypeName;
                BackgroundJob.Enqueue(() => EmailService_.sendWithStatus(StatusId, resView.Email, resView.EmployeeNameArabic, App.Remark, appName));
                BackgroundJob.Enqueue(() =>_SMSHelper.sendWithStatus(StatusId, resView.MobileNumber, resView.EmployeeNameArabic, App.Remark, appName) );
            //Return Response
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        // GET: api/ApplicationType/ApplicationTypeId
        [HttpGet("GetApplicationType/{ApplicationTypeId}")]
        public async Task<ActionResult> GetApplicationType(int ApplicationTypeId)
        {
            ResponseRequest_ = new ResponseRequest();
            var ApplicationType = await _context.ApplicationTypes.FindAsync(ApplicationTypeId);

            if (ApplicationType == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "";
            ResponseRequest_.Response = ApplicationType;
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        // PUT: api/ApplicationType/status update
        [HttpPut("UpdateApplicationTypeStatus/{ApplicationTypeId}/{IsActive}")]
        public async Task<IActionResult> UpdateApplicationTypeStatus(int ApplicationTypeId, bool IsActive )
        {
            ResponseRequest_ = new ResponseRequest();
            var App =
            (from a in _context.ApplicationTypes
             where a.ApplicationTypeId == ApplicationTypeId
             select a).FirstOrDefault();
            App.IsActive = IsActive;
            await _context.SaveChangesAsync();

            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.Response = CreatedAtAction("GetApplicationType", new { ApplicationTypeId = App.ApplicationTypeId }, App);
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

        [HttpPut("UpdateApplicationAssignTo/{ApplicationNumber}/{UserID}")]
        public async Task<IActionResult> UpdateApplicationAssignTo(int ApplicationNumber, int UserID)
        {
            ResponseRequest_ = new ResponseRequest();
            var App =
            (from a in _context.UserApplications
             where a.ApplicationNumber == ApplicationNumber
             select a).FirstOrDefault();
            if (App == null)
            {
                ResponseRequest_.Status = 404;
                ResponseRequest_.Message = "";
                ResponseRequest_.HasError = true;
                return NotFound(ResponseRequest_);
            }
            App.AssignedTo = UserID;
            await _context.SaveChangesAsync();

            ResponseRequest_.Status = 200;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.Response = CreatedAtAction("GetApplicationType", new { ApplicationTypeId = App.ApplicationTypeId }, App);
            ResponseRequest_.HasError = false;
            return Ok(ResponseRequest_);
        }

    }
}
