using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ResidencyApplication.Services.Models.CustomInputTypes;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;
namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ResidencyApplicationContext _context;
        public ResponseRequest ResponseRequest_ = new ResponseRequest();
        private IJwtServices _jwtServices;


        public UsersController(IJwtServices jwtServices, ResidencyApplicationContext context)
        {
            _context = context;
            _jwtServices = jwtServices;

        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("LdapUsers")]

        async public Task<ActionResult> ListLDAPUsers(string key)
        {
            
            List<EmployeeView> users = new List<EmployeeView>();
            if (string.IsNullOrEmpty(key))
            {


                ResponseRequest_.HasError = true;
                ResponseRequest_.Message = "برجاء ادخال اسم المستخدم";
                ResponseRequest_.Status = 404;
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);

            }
            EmployeeView usr = new EmployeeView();
            dynamic list = _context.EmployeeViews.Where(r => r.CivilId == key).ToList();

            if (list.Count == 0)
            {
                list = await FindEmployeeFromSAP(key);
                if (list.Response[0].CivilId == null)
                {
                    ResponseRequest_.HasError = true;
                    ResponseRequest_.Message = "لا يوجد بيانات لهذا الاسم ";
                    ResponseRequest_.Status = 404;
                    ResponseRequest_.Response = null;
                    return NotFound(ResponseRequest_);


                }
                usr.EmployeeName = list.Response[0].EmployeeName;
                usr.UserName = list.Response[0].DomainUsername;
                usr.CivilId = list.Response[0].CivilId;
                usr.Exsists = false;
                if (list.Response[0].employeetypeid == "ش1" || list.Response[0].nationalityid.ToString().ToLower() == "kw")
                {
                    usr.Valid = "NotValid";
                    usr.StatusMessage = list.Response[0].employeetypeid == "ش1" ? "نوع التوظيف الخاص به شركات عام غير متاح التجديد" : "لايمكن تجديد الاقامة للجنسية الكويتية";
                }
                else
                { 
                    usr.Valid = "valid";
                }

            }
            else
            {

                // var listAD =  ListADUsers(key);
                usr = list[0];

            }
            users.Add(usr);
            ResponseRequest_.HasError = false;
            ResponseRequest_.Message = "";
            ResponseRequest_.Status = 200;
            ResponseRequest_.Response = users;
            return Ok(ResponseRequest_);
       
        }

        private static List<UserAccount> ListADUsers(string searchText)
        {
            if (searchText == null || searchText == "")
            {
                return null;
            }
            searchText = "*" + searchText + "*";
            List<UserAccount> lstADUsers = new List<UserAccount>();

            string DomainPath = "LDAP://awqaf.gov.kw:389";

            string u = "", p = "";
            try
            {
                u = "msalam338";
                p = "me7me7";
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            DirectoryEntry searchRoot = new DirectoryEntry(DomainPath, u, p);

            DirectorySearcher search = new DirectorySearcher(searchRoot);
            //search.Filter = "((|(cn=" + searchText + ")(sAMAccountName=" + searchText + ")(description=" + searchText + ")))";
            search.Filter = "((|(description=" + searchText + ")))";
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("description");
            search.PropertiesToLoad.Add("SAMAccountName");
            search.PropertiesToLoad.Add("displayname");
            search.PropertiesToLoad.Add("physicalDeliveryOfficeName");

            SearchResult result;
            SearchResultCollection resultCol = search.FindAll();

            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    result = resultCol[counter];
                    UserAccount usr = new UserAccount();

                    if (result.Properties.Contains("samaccountname") &&
                             (String)result.Properties["SAMAccountName"][0] != "" &&
                             result.Properties.Contains("mail") &&
                             result.Properties.Contains("displayname"))
                        try
                        {
                            usr.Name = (String)result.Properties["cn"][0];
                            usr.Email = (String)result.Properties["mail"][0];
                            usr.CivilID = (String)result.Properties["description"][0];
                            usr.UserName = (String)result.Properties["SAMAccountName"][0];
                            if (result.Properties.Contains("physicalDeliveryOfficeName"))
                            {
                                usr.Jobtitle = (String)result.Properties["physicalDeliveryOfficeName"][0];//title
                            }

                            lstADUsers.Add(usr);
                        }
                        catch (Exception ex)
                        {
                            //lstADUsers.Add(usr);
                        }
                }
            }
            return lstADUsers;
        }

        private async Task<ResponseRequest<List<NominatedEmployee>>> FindEmployeeFromSAP(string term)
        {

            if (string.IsNullOrEmpty(term))
            {
                return new ResponseRequest<List<NominatedEmployee>>
                {
                    Message = "not_allow_empty",
                    HasError = true
                };
            }
            string url = "http://10.31.65.115:8100/awqaf_host/pa/pa0006/";
            string auth = "Basic a3phZGVoMzU3OjIyQEp1bmVAMTk3Mw==";
            try
            {
                using (var client = new HttpClient())
                {
                    //Send HTTP requests from here.
                    SapResult result = new SapResult();
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", auth);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(term);
                    if (response.IsSuccessStatusCode)
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        var data = SapResult.FromJson(res);
                        List<NominatedEmployee> nominees = new List<NominatedEmployee>();
                        var e = data;
                        NominatedEmployee n = new NominatedEmployee()
                        {
                            CivilId = e.Civilid,
                            DepartmentName = e.Department,
                            EmployeeName = e.Employeename,
                            JobTitle = e.Jobtitle,
                            RemoteEmployeeID = e.Filenumber,
                            SectionName = e.Section,
                            SectorName = e.Sector,
                            DomainUsername = e.Domainusername,
                            employeetypeid = e.Employeetypeid,
                            nationalityid=e.Nationalityid
                           
                            
                        };

                        if (!string.IsNullOrWhiteSpace(e.Workcenter))
                        {
                            n.RemoteID = e.Workcenterid.ToString();
                            n.RemoteValue = e.Workcenter;
                            n.RemoteType = "Work Center";
                        }
                        else if (!string.IsNullOrWhiteSpace(e.Subsection))
                        {
                            n.RemoteID = e.Subsectionid.ToString();
                            n.RemoteValue = e.Subsection;
                            n.RemoteType = "Sub Section";
                        }
                        else if (!string.IsNullOrWhiteSpace(e.Section))
                        {
                            n.RemoteID = e.Sectionid;
                            n.RemoteValue = e.Section;
                            n.RemoteType = "Section";
                        }
                        else if (!string.IsNullOrWhiteSpace(e.Subdepartment))
                        {
                            n.RemoteID = e.Subdepartmentid;
                            n.RemoteValue = e.Subdepartment;
                            n.RemoteType = "Sub Department";
                        }
                        else if (!string.IsNullOrWhiteSpace(e.Department))
                        {
                            n.RemoteID = e.Departmentid.ToString();
                            n.RemoteValue = e.Department;
                            n.RemoteType = "Department";
                        }
                        else if (!string.IsNullOrWhiteSpace(e.Sector))
                        {
                            n.RemoteID = e.Sectorid.ToString();
                            n.RemoteValue = e.Sector;
                            n.RemoteType = "Sector";
                        }
                        //Duty Time..
                        if (!string.IsNullOrWhiteSpace(e.Dutytime))
                        {
                            n.DutyTime = e.Dutytime;
                        }
                        else
                        {
                            n.DutyTime = "-";
                        }
                        nominees.Add(n);

                        return new ResponseRequest<List<NominatedEmployee>>
                        {
                            Response = nominees,
                            HasError = false
                        };

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseRequest<List<NominatedEmployee>>
                {
                    Message = ex.Message,
                    HasError = true
                };
            }

        }

        //        GET : getting admin users
        [HttpGet("Admins")]
        public async Task<ActionResult> GetAdminUsers()
        {
            try
            {
                var res = await _context.Users.Where(r => r.UserRoleId == 1 || r.UserRoleId == 3).ToListAsync();
                var res_ = from ru in _context.Users
                           join ur in _context.UserRoles on ru.UserRoleId equals ur.UserRoleId
                           join rs in _context.SapUsers on ru.UserId equals rs.UserId into us
                           from rs in us.DefaultIfEmpty()
                           join rns in _context.NonSapUsers on ru.UserId equals rns.UserId into uns
                           from rns in uns.DefaultIfEmpty()
                           where ru.UserRoleId == 1 || ru.UserRoleId == 3
                           select new UserAccountInfo
                           {
                               UserId = ru.UserId,
                               EmployeeNumber = ru.EmployeeNumber.ToString(),
                               Email = ru.Email,
                               EmployeeName = ru.IsSapUser == true ? rs.EmployeeName : rns.EmployeeName,
                               CivilId = ru.IsSapUser == true ? rs.CivilId : rns.CivilId,
                               UserRoleId = (int)ru.UserRoleId,
                               UserRoleName = ur.UserRoleNameAr,
                           };

                if (res_ == null)
                {
                    ResponseRequest_.HasError = false;
                    ResponseRequest_.Message = "لايوجد بيانات";
                    ResponseRequest_.Status = 404;
                    ResponseRequest_.Response = null;
                    return Ok(ResponseRequest_);
                }
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "";
                ResponseRequest_.Status = 200;
                ResponseRequest_.Response = res_;
                return Ok(ResponseRequest_);
            }
            catch (Exception)
            {
                ResponseRequest_.HasError = true;
                ResponseRequest_.Message = "يوجد خطاء";
                ResponseRequest_.Status = 400;
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);
            }
        }


        //        GET : getting admin users
        [HttpGet("Admins/{level}")]
        public async Task<ActionResult> GetAdminUsers(int level)
        {
            try
            {
                var res = await _context.Users.Where(r => r.UserRoleId == 1 || r.UserRoleId == 3).ToListAsync();
                var res_ = from ru in _context.Users
                           join ur in _context.UserRoles on ru.UserRoleId equals ur.UserRoleId
                           join rs in _context.SapUsers on ru.UserId equals rs.UserId into us
                           from rs in us.DefaultIfEmpty()
                           join rns in _context.NonSapUsers on ru.UserId equals rns.UserId into uns
                           from rns in uns.DefaultIfEmpty()
                           where ru.UserRoleId == level
                           select new UserAccountInfo
                           {
                               UserId = ru.UserId,
                               EmployeeNumber = ru.EmployeeNumber.ToString(),
                               Email = ru.Email,
                               EmployeeName = ru.IsSapUser == true ? rs.EmployeeName : rns.EmployeeName,
                               CivilId = ru.IsSapUser == true ? rs.CivilId : rns.CivilId,
                               UserRoleId = (int)ru.UserRoleId,
                               UserRoleName = ur.UserRoleNameAr,
                           };

                if (res_ == null)
                {
                    ResponseRequest_.HasError = false;
                    ResponseRequest_.Message = "لايوجد بيانات";
                    ResponseRequest_.Status = 404;
                    ResponseRequest_.Response = null;
                    return Ok(ResponseRequest_);
                }
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "";
                ResponseRequest_.Status = 200;
                ResponseRequest_.Response = res_;
                return Ok(ResponseRequest_);
            }
            catch (Exception)
            {
                ResponseRequest_.HasError = true;
                ResponseRequest_.Message = "يوجد خطاء";
                ResponseRequest_.Status = 400;
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);
            }
        }




        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/5
        [HttpGet("GetUpdatedUser/{id}")]
        public async Task<ActionResult<User>> GetUpdatedUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            UpdatedUserObject returnObj = new UpdatedUserObject();
            if (user == null)
            {
                ResponseRequest_.Response = null;
                ResponseRequest_.HasError = true;
                ResponseRequest_.Message = "";
                ResponseRequest_.Status = 404;
                return NotFound(ResponseRequest_);
            }
            returnObj.UserId = user.UserId;
            returnObj.CivilIdSerial = user.CivilIdSerialNumber;
            returnObj.UserType = (int)user.UserTypeId;
            returnObj.JobTypeId = (int)user.JobtypeId;
            returnObj.SectionId = (int)user.SectionId;
            returnObj.MobileNumber = user.MobileNumber;
            if (user.IsSapUser)
            {
                var user_ = _context.SapUsers.Where(r => r.UserId == id).FirstOrDefault();
                returnObj.EmployeeNumber = user_.EmployeeNumber;
            }

            if (!user.IsSapUser)
            {
                var user_ = _context.NonSapUsers.Where(r => r.UserId == id).FirstOrDefault();
                returnObj.EmployeeNumber = user_.EmployeeNumber;

            }
            ResponseRequest_.Response = returnObj;
            ResponseRequest_.HasError = false;
            ResponseRequest_.Message = "";
            ResponseRequest_.Status = 200;
            return Ok(ResponseRequest_);
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                //Insert data into log table
                var lastinsertedId = user.UserId;
                UsersLog userlog = new UsersLog();
                userlog.UserLogId = 0;
                userlog.UserId = lastinsertedId;
                userlog.CivilIdSerialNumber = user.CivilIdSerialNumber;
                userlog.MobileNumber = user.MobileNumber;
                userlog.Email = user.Email;
                userlog.ResidencyByMoa = user.ResidencyByMoa;
                userlog.NationalityId = user.NationalityId;
                userlog.IsSapUser = user.IsSapUser;
                userlog.CreatedDate = user.CreatedDate;
                userlog.UpdatedDate = user.UpdatedDate;
                userlog.IsActive = user.IsActive;
                userlog.IsAdmin = user.IsAdmin;
                userlog.Action = "Updated";
                _context.UsersLogs.Add(userlog);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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


        [HttpPut("PutUserRoleId/{id}/{role}")]
        public async Task<IActionResult> PutUserRoleId(int id, int role)
        {

            try
            {
                var res = _context.Users.Find(id);
                res.UserRoleId = role;
                _context.SaveChanges();
                ResponseRequest_.HasError = false;
                ResponseRequest_.Message = "تم الحفظ بنجاح";
                ResponseRequest_.Status = 200;
                ResponseRequest_.Response = null;
                return Ok(ResponseRequest_);
            }
            catch (Exception ex)
            {
                ResponseRequest_.HasError = true;
                ResponseRequest_.Message = "حدث خطاء عند الحفظ";
                ResponseRequest_.Status = 400;
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);

            }

        }


        [HttpPost("AddUserToDbWithUserRoleId")]
        public async Task<IActionResult> AddUserToDbWithUserRoleId(UserAccountInfo input)
        {
            try
            {
                SpeakerInfoModels SpeakerInfoModelsession = null;
                _jwtServices.GetSpeakerInfoByUserName_SAP("");
                SpeakerInfoModelsession = _jwtServices.GetSpeakerInfoByUserName_SAP(input.UserName);
                if (SpeakerInfoModelsession.civilid != null)
                {
                    var Exists = _context.SapUsers.Where(r => r.CivilId == SpeakerInfoModelsession.civilid);
                    if (Exists.Count() > 0)
                    {
                        var res_ = _context.Users.Find(Exists.FirstOrDefault().UserId);
                        res_.UserRoleId = input.UserRoleId;
                        _context.SaveChanges();
                        var user = _context.EmployeeViews.Where(r => r.UserId == res_.UserId).FirstOrDefault();
                        ResponseRequest_.HasError = false;
                        ResponseRequest_.Message = "تم الحفظ بنجاح";
                        ResponseRequest_.Status = 200;
                        ResponseRequest_.Response = user;
                        return Ok(ResponseRequest_);

                    }
                    else
                    {
                        int SectionId = 0;
                        if (SpeakerInfoModelsession.sectionid != null || SpeakerInfoModelsession.sectionid != 0)
                        {
                            var check = _context.Organizations.Where(r => r.SapId == SpeakerInfoModelsession.sectionid).Select(r => r.Id).ToList().FirstOrDefault();
                            if (check == null)
                            {
                                Organization org = new Organization { Name = "", SapId = SpeakerInfoModelsession.sectionid, ParentId = SpeakerInfoModelsession.subdepartmentid };
                                _context.Organizations.Add(org);
                                _context.SaveChanges();
                                SectionId = org.Id;
                            }
                            else
                            {
                                SectionId = check;
                            }
                        }
                        ///Validate and migrate UserType 
                        int UserTypeId = 0;
                        if (SpeakerInfoModelsession.employeetypeid != null || SpeakerInfoModelsession.employeetypeid != "")
                        {
                            var check = _context.UserTypes.Where(r => r.SapId == SpeakerInfoModelsession.employeetypeid).Select(r => r.UserTypeId).ToList().FirstOrDefault();
                            if (check == 0 || check == null)
                            {
                                UserType UserType_ = new UserType { UserTypeName = SpeakerInfoModelsession.employeetype, SapId = SpeakerInfoModelsession.employeetypeid };
                                _context.UserTypes.Add(UserType_);
                                _context.SaveChanges();
                                UserTypeId = UserType_.UserTypeId;
                            }
                            else
                            {
                                UserTypeId = check;
                            }
                        }
                        ///Validate and migrate UserType 
                        int JobtypeId = 0;
                        if (SpeakerInfoModelsession.jobtitleid != null || SpeakerInfoModelsession.jobtitleid != 0)
                        {
                            var checkSapId = _context.JobTypes.Where(r => r.Sapid == SpeakerInfoModelsession.jobtitleid).Select(r => r.Id).ToList().FirstOrDefault();
                            var checkSapName = _context.JobTypes.Where(r => r.Sapid == SpeakerInfoModelsession.jobtitleid).Select(r => r.Id).ToList().FirstOrDefault();
                            if (checkSapId == 0 || checkSapId == null)
                            {
                                if (checkSapName == 0 || checkSapName == null)
                                {
                                    int maxId = (int)_context.JobTypes.Max(r => r.Id);
                                    JobType JobType_ = new JobType { Id = maxId + 1, Name = SpeakerInfoModelsession.jobtitle, Sapid = SpeakerInfoModelsession.jobtitleid };
                                    _context.JobTypes.Add(JobType_);
                                    _context.SaveChanges();
                                    JobtypeId = (int)JobType_.Id;
                                }
                                else
                                {
                                    JobType JobType_ = new JobType { Id = checkSapName, Name = SpeakerInfoModelsession.jobtitle, Sapid = SpeakerInfoModelsession.jobtitleid };
                                    _context.JobTypes.Add(JobType_);
                                    _context.SaveChanges();
                                    JobtypeId = (int)JobType_.Id;
                                }
                            }
                            else
                            {
                                JobtypeId = (int)checkSapId;
                            }
                        }
                        User user_ = new User();
                        SapUser Sapuser_ = new SapUser();
                        user_.IsActive = true;
                        user_.IsSapUser = true;
                        user_.Email = input.UserName;
                        user_.NationalityId = SpeakerInfoModelsession.nationalityid;
                        user_.SectionId = SectionId;
                        user_.UserTypeId = UserTypeId;
                        user_.JobtypeId = JobtypeId;
                        user_.UserRoleId = input.UserRoleId;
                        user_.GenderId = SpeakerInfoModelsession.genderid;
                        //user_.ResidencyByMoa = false;
                        try
                        {
                            user_.EmployeeNumber = Convert.ToInt32(SpeakerInfoModelsession.personelno);
                        }
                        catch (Exception)
                        {
                        }
                        _context.Users.Add(user_);
                        _context.SaveChanges();
                        //// saving user to sap users table 
                        Sapuser_.CivilId = SpeakerInfoModelsession.civilid;
                        Sapuser_.EmployeeName = SpeakerInfoModelsession.employeename;
                        Sapuser_.EmployeeType = SpeakerInfoModelsession.employeetype;
                        Sapuser_.Department = SpeakerInfoModelsession.department;
                        Sapuser_.UserId = user_.UserId;
                        Sapuser_.HireDate = Convert.ToDateTime(DateTime.ParseExact(SpeakerInfoModelsession.hireddate.Replace('.', '-'), "dd-MM-yyyy", CultureInfo.InvariantCulture));
                        Sapuser_.BirthDate = Convert.ToDateTime(DateTime.ParseExact(SpeakerInfoModelsession.birthdate.Replace('.', '-'), "dd-MM-yyyy", CultureInfo.InvariantCulture));
                        Sapuser_.Section = SpeakerInfoModelsession.section;
                        Sapuser_.Sector = SpeakerInfoModelsession.sector;
                        Sapuser_.EmployeeNumber = SpeakerInfoModelsession.personelno;
                        Sapuser_.Organization = SpeakerInfoModelsession.departmentid;

                        _context.SapUsers.Add(Sapuser_);
                        _context.SaveChanges();
                        var user = _context.EmployeeViews.Where(r => r.UserId == Sapuser_.UserId).FirstOrDefault(); 
                        ResponseRequest_.HasError = false;
                        ResponseRequest_.Message = "تم الحفظ بنجاح";
                        ResponseRequest_.Status = 200;
                        ResponseRequest_.Response = user;
                        return Ok(ResponseRequest_);
                    }
                  
                }
                else
                {
                    var Exists = _context.Users.Where(r => r.Email == input.UserName);
                    if (Exists.Count() > 0)
                    {
                        var res_ = _context.Users.Find(Exists.FirstOrDefault().UserId);
                        res_.UserRoleId = input.UserRoleId;
                        var user = _context.EmployeeViews.Where(r => r.UserId == res_.UserId).FirstOrDefault();
                        _context.SaveChanges();
                        ResponseRequest_.HasError = false;
                        ResponseRequest_.Message = "تم الحفظ بنجاح";
                        ResponseRequest_.Status = 200;
                        ResponseRequest_.Response = res_;
                        return Ok(ResponseRequest_);
                    }
                    else
                    {
                        User user_ = new User();
                        SapUser Sapuser_ = new SapUser();
                        user_.IsActive = true;
                        user_.IsSapUser = true;
                        user_.Email = input.UserName;
                        user_.UserRoleId = input.UserRoleId;
                        user_.NationalityId = "";
                        _context.Users.Add(user_);
                        _context.SaveChanges();
                        //// saving user to sap users table 
                        Sapuser_.EmployeeName = input.EmployeeName;
                        Sapuser_.CivilId = input.CivilId;
                        Sapuser_.UserId = user_.UserId;
                        _context.SapUsers.Add(Sapuser_);
                        _context.SaveChanges();
                        var user = _context.EmployeeViews.Where(r => r.UserId == Sapuser_.UserId).FirstOrDefault();
                        ResponseRequest_.HasError = false;
                        ResponseRequest_.Message = "تم الحفظ بنجاح";
                        ResponseRequest_.Status = 200;
                        ResponseRequest_.Response = user;
                        return Ok(ResponseRequest_);
                    }
                   
                }

            }
            catch (Exception ex)
            {
                ResponseRequest_.HasError = true;
                ResponseRequest_.Message = "حدث خطاء";
                ResponseRequest_.Status = 400;
                ResponseRequest_.Response = null;
                return BadRequest(ResponseRequest_);
            }
        }


        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //Insert data into log table
            var lastinsertedId = user.UserId;
            UsersLog userlog = new UsersLog();
            userlog.UserLogId = 0;
            userlog.UserId = lastinsertedId;
            userlog.CivilIdSerialNumber = user.CivilIdSerialNumber;
            userlog.MobileNumber = user.MobileNumber;
            userlog.Email = user.Email;
            userlog.ResidencyByMoa = user.ResidencyByMoa;
            userlog.NationalityId = user.NationalityId;
            userlog.IsSapUser = user.IsSapUser;
            userlog.CreatedDate = user.CreatedDate;
            userlog.UpdatedDate = user.UpdatedDate;
            userlog.IsActive = user.IsActive;
            userlog.IsAdmin = user.IsAdmin;
            userlog.Action = "Inserted";
            _context.UsersLogs.Add(userlog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        [HttpPost("changeProfile")]
        async public Task<IActionResult> changeProfile([FromBody] UserProfile model)
        {
            var user = await _context.Users.FindAsync(model.UserId);

            try
            {
                user.Email = model.Email != null ? model.Email : user.Email;
                user.CivilIdSerialNumber = model.CivilIdSerialNumber;
                user.MobileNumber = model.MobileNumber;
                //   user.EmployeeNumber = Convert.ToInt32(model.EmployeeNumber);
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                if (user.IsSapUser)
                {
                    SapUser usersap = _context.SapUsers.Where(r => r.UserId == model.UserId).First();
                    usersap.EmployeeNumber = model.EmployeeNumber;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    NonSapUser usernonsap = _context.NonSapUsers.Where(r => r.UserId == model.UserId).FirstOrDefault();
                    usernonsap.EmployeeNumber = model.EmployeeNumber;
                    _context.Entry(usernonsap).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                ResponseRequest_.HasError = true;
                ResponseRequest_.Message = "حدث خطا";
                ResponseRequest_.Status = 404;
                return NotFound(ResponseRequest_);
            }
            ResponseRequest_.HasError = false;
            ResponseRequest_.Message = "تم الحفظ بنجاح";
            ResponseRequest_.Status = 200;
            ResponseRequest_.Response = user;
            return Ok(ResponseRequest_);
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


    }

    public class LdapMessage
    {
        public String userName { get; set; }
        public String fullName { get; set; }
        public String email { get; set; }
        public String description { get; set; }
        public String officeName { get; set; }
    }
    public class UserAccountInfo
    {
        public String UserName { get; set; }
        public String EmployeeName { get; set; }
        public String Email { get; set; }
        public String CivilId { get; set; }
        public int UserRoleId { get; set; }
        public string EmployeeNumber { get; set; }
        public int UserId { get; set; }
        public string UserRoleName { get; set; }




    }

    
    public class UserAccount
    {

        public UserAccount()
        {

        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public string Jobtitle { get; set; }
        public string CivilID { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserIP { get; set; }
        public string SessionGUID { get; set; }

        public bool IsAdmin { get; set; }
        public string UserPassword { get; set; }
        public bool IsBeneficiary { get; set; }

    }


    public class NominatedEmployee
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string RemoteID { get; set; }
        public string DomainUsername { get; set; }
        public string RemoteType { get; set; }
        public string RemoteValue { get; set; }
        public string RemoteEmployeeID { get; set; }
        public string SectorName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? RegistrationDateTime { get; set; }
        public string Status { get; set; }
        public int? AcademicYear { get; set; }
        public string CivilId { get; set; }
        public int? DecisionBasedOnIndex { get; set; }
        public string Mobile { get; set; }
        public string DutyTime { get; set; }

        public string employeetypeid { get; set; }

        public string nationalityid { get; set;  }
        [NotMapped]
        public string NomineeHistoryCount { get; set; }

    }


    public class UpdatedUserObject
    {
        public int? UserId { get; set; }
        public string? CivilIdSerial { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? MobileNumber { get; set; }
        public int? SectionId { get; set; }
        public int? UserType { get; set; }
        public int? JobTypeId { get; set; }
    }


    public partial class SapResult
    {
        [JsonProperty("birthdate")]
        public string Birthdate { get; set; }

        [JsonProperty("cardholder")]
        public string Cardholder { get; set; }

        [JsonProperty("civilid")]
        public string Civilid { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("departmentid")]
        public string Departmentid { get; set; }

        [JsonProperty("domainusername")]
        public string Domainusername { get; set; }

        [JsonProperty("dutytime")]
        public string Dutytime { get; set; }

        [JsonProperty("dutytimeid")]
        public string Dutytimeid { get; set; }

        [JsonProperty("employeename")]
        public string Employeename { get; set; }

        [JsonProperty("employeestatus")]
        public string Employeestatus { get; set; }

        [JsonProperty("employeestatusid")]
        public string Employeestatusid { get; set; }

        [JsonProperty("employeetype")]
        public string Employeetype { get; set; }

        [JsonProperty("employeetypeid")]
        public string Employeetypeid { get; set; }

        [JsonProperty("filenumber")]
        public string Filenumber { get; set; }

        [JsonProperty("financialgrade")]
        public string Financialgrade { get; set; }

        [JsonProperty("financialgradearea")]
        public string Financialgradearea { get; set; }

        [JsonProperty("financialgradetype")]
        public string Financialgradetype { get; set; }

        [JsonProperty("fingerprintid")]
        public string Fingerprintid { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("genderid")]
        public string Genderid { get; set; }

        [JsonProperty("hireddate")]
        public string Hireddate { get; set; }

        [JsonProperty("islinesupervisorof")]
        public string Islinesupervisorof { get; set; }

        [JsonProperty("ismanager")]
        public string Ismanager { get; set; }

        [JsonProperty("jobtitle")]
        public string Jobtitle { get; set; }

        [JsonProperty("jobtitleid")]
        public string Jobtitleid { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("nationalityid")]
        public string Nationalityid { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("organizationid")]
        public string Organizationid { get; set; }

        [JsonProperty("organizationunitid")]
        public string Organizationunitid { get; set; }

        [JsonProperty("organizationunitlevel")]
        public string Organizationunitlevel { get; set; }

        [JsonProperty("personelno")]
        public string Personelno { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("sectionid")]
        public string Sectionid { get; set; }

        [JsonProperty("sector")]
        public string Sector { get; set; }

        [JsonProperty("sectorid")]
        public string Sectorid { get; set; }

        [JsonProperty("subdepartment")]
        public string Subdepartment { get; set; }

        [JsonProperty("subdepartmentid")]
        public string Subdepartmentid { get; set; }

        [JsonProperty("subsection")]
        public string Subsection { get; set; }

        [JsonProperty("subsectionid")]
        public string Subsectionid { get; set; }

        [JsonProperty("workcenter")]
        public string Workcenter { get; set; }

        [JsonProperty("workcenterid")]
        public string Workcenterid { get; set; }

        [JsonProperty("workschedulerule")]
        public string Workschedulerule { get; set; }

        [JsonProperty("workscheduletime")]
        public string Workscheduletime { get; set; }
    }

    public partial class SapResult
    {
        public static SapResult FromJson(string json) => JsonConvert.DeserializeObject<SapResult>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this SapResult self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
