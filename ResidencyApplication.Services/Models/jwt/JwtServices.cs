using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ResidencyApplication.Services.Models.DomainModels;
using ResidencyApplication.Services.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace ResidencyApplication.Services.Models.jwt
{
    public interface IJwtServices
    {
        Task<ResponseRequest<AuthenticateResponse>> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
        public commonUserInfo Register(RegisterRequest model);
        public SpeakerInfoModels GetSpeakerInfoByCivilId_SAP(string civilid);
        public SpeakerInfoModels GetSpeakerInfoByUserName_SAP(string UserName) ;
        public SpeakerInfoModels ReturnSapUserFromDb(string UserName);
        Task<object> RevokeToken(string token, string ipAddress);
    }
    public class LdapMessage
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string DomainUserFullName { get; set; }
    }
    public class SpeakerInfoModels
    {
        public string employeename { get; set; }
        public string civilid { get; set; }
        public string nationality { get; set; }
        public string employeetype { get; set; }
        public string jobtitle { get; set; }
        public string department { get; set; }
        public int departmentid { get; set; }
        public string fingerprintid { get; set; }
        public string filenumber { get; set; }
        public string hireddate { get; set; }
        public string birthdate { get; set; }
        public string section { get; set; }
        public string sector { get; set; }
        public string personelno { get; set; }
        public string nationalityid { get; set; }
        public string employeetypeid { get; set; }
        public int jobtitleid { get; set; }
        public int genderid { get; set; }
        public int sectionid { get; set; }
        public int subdepartmentid { get; set; }
        
    }
    public class JwtServices : IJwtServices
    {
        private readonly ResidencyApplicationContext _context;
        private readonly AppSettings _appSettings;
        public commonUserInfo commonUserInfo_;
        public ResponseRequest<commonUserInfo> response;
        public ResponseRequest<AuthenticateResponse> AuthenticateResponse_;
        public JwtServices(IOptions<AppSettings> appSettings, ResidencyApplicationContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;

        }
        private LdapMessage AuthenticateLDAPUser(string name, string pwd)
        {
            try
            {
                string result = "";
                var request = WebRequest.Create(new Uri("http://10.31.90.60:5020/ldap/" + name + "/" + pwd + "/")) as HttpWebRequest;
                request.Accept = "application/json";
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                }
                return JsonConvert.DeserializeObject<LdapMessage>(result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public SpeakerInfoModels GetSpeakerInfoByUserName_SAP(string UserName)
        {
            try
            {
                SpeakerInfoModels currentUser = new SpeakerInfoModels();

                string result = "";
                var request = WebRequest.Create(new Uri("http://sapprd02-hq:8100/awqaf_host/pa/pa0005/" + UserName)) as HttpWebRequest;
                request.Accept = "application/json";
                request.Credentials = new System.Net.NetworkCredential("kzadeh357", "22@June@1973");
                //Authorization: Basic a3phZGVoMzU3OjIyQEp1bmVAMTk3Mw==
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    currentUser = (SpeakerInfoModels)JsonConvert.DeserializeObject(result, typeof(SpeakerInfoModels));
                }
                return currentUser;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public SpeakerInfoModels GetSpeakerInfoByCivilId_SAP(string civilid)
        {
            try
            {
                SpeakerInfoModels currentUser = new SpeakerInfoModels();

                string result = "";
                var request = WebRequest.Create(new Uri("http://sapprd02-hq:8100/awqaf_host/pa/pa0006/" + civilid)) as HttpWebRequest;
                request.Accept = "application/json";
                request.Credentials = new System.Net.NetworkCredential("kzadeh357", "22@June@1973");
                //Authorization: Basic a3phZGVoMzU3OjIyQEp1bmVAMTk3Mw==
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    currentUser = (SpeakerInfoModels)JsonConvert.DeserializeObject(result, typeof(SpeakerInfoModels));
                }
                return currentUser;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public commonUserInfo Register(RegisterRequest model)
        {


            User user_ = new User();
            NonSapUser NonSapUser_ = new NonSapUser();
            commonUserInfo_ = new commonUserInfo();
            user_.CivilIdSerialNumber = model.CivilIdSerialNumber;
            user_.IsActive = false;
            user_.IsSapUser = false;
            user_.Email = model.Email;
            user_.MobileNumber = model.MobileNumber;
            user_.NationalityId = model.NationalityId;
            user_.EmployeeNumber = Convert.ToInt32(model.EmployeeNumber);
            user_.SectionId = Convert.ToInt32(model.Organization);
            user_.UserTypeId = Convert.ToInt32(model.UserTypeId);
            user_.JobtypeId = Convert.ToInt32(model.JobtypeId);
            user_.UserRoleId = 2;
            user_.GenderId= Convert.ToInt32(model.GenderId);
            _context.Users.Add(user_);
            _context.SaveChanges();
            NonSapUser_.CivilId = model.CivilId;
         //   NonSapUser_.BirthDate = model.BirthDate;
            NonSapUser_.EmployeeName = model.EmployeeName;
            NonSapUser_.UserTypeId = Convert.ToInt32(model.UserTypeId);
            NonSapUser_.Organization =Convert.ToInt32( model.Organization);
            NonSapUser_.UserId = user_.UserId;
            NonSapUser_.Password = model.Password;
            NonSapUser_.EmployeeNumber = model.EmployeeNumber;
            NonSapUser_.RegistrationStatusId = 1;
            NonSapUser_.RegistrationDate = DateTime.Now;
            _context.NonSapUsers.Add(NonSapUser_);
            _context.SaveChanges();
            commonUserInfo_.UserId = NonSapUser_.UserId;
            commonUserInfo_.MobileNumber = user_.MobileNumber;
            commonUserInfo_.Email = user_.Email;
            commonUserInfo_.ResidencyByMoa = user_.ResidencyByMoa;
            commonUserInfo_.IsSapUser = user_.IsSapUser;
            commonUserInfo_.IsActive = (bool)user_.IsActive;
            commonUserInfo_.CivilId = NonSapUser_.CivilId;
            commonUserInfo_.EmployeeNumber = NonSapUser_.EmployeeNumber;
            commonUserInfo_.EmployeeName = NonSapUser_.EmployeeName;
            commonUserInfo_.Nationality = _context.Nationalities.Where(r => r.NationalityId == user_.NationalityId).Select(r => r.NationalityName).FirstOrDefault().ToString();
            commonUserInfo_.RegistrationDate = NonSapUser_.RegistrationDate;
            commonUserInfo_.Organization = "";
            commonUserInfo_.UserTypeId =(int) user_.UserTypeId;
            commonUserInfo_.SectionId= (int)user_.SectionId;
            commonUserInfo_.JobtypeId= (int)user_.JobtypeId;
            commonUserInfo_.UserRoleId = (int)user_.UserRoleId;
            commonUserInfo_.RegistrationStatusId = 0;
            commonUserInfo_.BirthDate = NonSapUser_.BirthDate;
            return commonUserInfo_;


        }
        public commonUserInfo ReturnSapUserFromDb(string username)
        {

            return (from ru in _context.Users
                    join rsap in _context.SapUsers on ru.UserId equals rsap.UserId
                    join rna in _context.Nationalities on ru.NationalityId equals rna.NationalityId  //left join 

                    where ru.Email == username
                    select (new commonUserInfo
                    {
                        UserId = ru.UserId,
                        MobileNumber = ru.MobileNumber,
                        Email = ru.Email,
                        ResidencyByMoa = ru.ResidencyByMoa,
                        IsSapUser = ru.IsSapUser,
                        IsActive = ru.IsActive != null ? (bool)ru.IsActive : false,
                        CivilId = rsap.CivilId,
                        CivilIdSerialNumber = ru.CivilIdSerialNumber,
                        EmployeeNumber = rsap.EmployeeNumber,
                        EmployeeName = rsap.EmployeeName,
                        EmployeeType = rsap.EmployeeType,
                        Sector = rsap.Sector,
                        Department = rsap.Department,
                        Section = rsap.Section,
                        SectionId=(int)ru.SectionId,
                        NationalityId = ru.NationalityId,
                        UserTypeId = (int)ru.UserTypeId,
                        JobtypeId = (int)ru.JobtypeId,
                        UserRoleId=(int)ru.UserRoleId,
                        Nationality = rna.NationalityName,
                        BirthDate = rsap.BirthDate,
                        JobTitle = rsap.JobTitle,
                        HireDate = rsap.HireDate,
                        Organization = rsap.Department,
                        RegistrationStatusId = 0,
                        IsAdmin = (bool)ru.IsAdmin
                    })).FirstOrDefault();
        }
        public commonUserInfo ReturnNonSapUserFromDb(string username)
        {
            return (from ru in _context.Users
                    join rn in _context.NonSapUsers on ru.UserId equals rn.UserId
                    join rt in _context.UserTypes on rn.UserTypeId equals rt.UserTypeId
                    join rna in _context.Nationalities on ru.NationalityId equals rna.NationalityId
                    join ro in _context.Organizations on ru.SectionId equals ro.Id
                    where ru.Email == username && rn.RegistrationStatusId == 2
                    select new commonUserInfo
                    {
                        UserId = ru.UserId,
                        MobileNumber = ru.MobileNumber,
                        Email = ru.Email,
                        ResidencyByMoa = ru.ResidencyByMoa,
                        RegistrationDate = rn.RegistrationDate,
                        IsSapUser = ru.IsSapUser,
                        IsActive = (bool)ru.IsActive,
                        CivilId = rn.CivilId,
                        CivilIdSerialNumber = ru.CivilIdSerialNumber,
                        EmployeeNumber = rn.EmployeeNumber,
                        EmployeeName = rn.EmployeeName,
                        EmployeeType = rt.UserTypeName,
                        Organization = ro.Name,
                        Department = "",
                        Section = "",
                        SectionId = (int)ru.SectionId,
                        NationalityId = ru.NationalityId,
                        UserTypeId = (int)ru.UserTypeId,
                        JobtypeId = (int)ru.JobtypeId,
                        UserRoleId=(int)ru.UserRoleId,
                        Nationality = rna.NationalityName,
                        BirthDate = rn.BirthDate,
                        JobTitle = rt.UserTypeName,
                        HireDate = null,
                        RegistrationStatusId = rn.RegistrationStatusId,
                        IsAdmin = (bool)ru.IsAdmin

                    }).FirstOrDefault();
        }
        public ResponseRequest<commonUserInfo> AuthenticateUser(AuthenticateRequest model)
        {
             response = new ResponseRequest<commonUserInfo>();
            LdapMessage LDAPsession = null;
            SpeakerInfoModels SpeakerInfoModelsession = null;
            if (!model.Username.Trim().ToLower().Contains("@"))
            {
                var name = model.Username;
                LDAPsession = AuthenticateLDAPUser(name, model.Password);
                if (LDAPsession.IsAuthenticated)
                {

                    SpeakerInfoModelsession = GetSpeakerInfoByUserName_SAP(name);
                    if (SpeakerInfoModelsession?.civilid != null)
                    {
                        //var Exists = _context.Users.Where(r => r.CivilIdSerialNumber == SpeakerInfoModelsession.civilid);
                        var Exists = _context.SapUsers.Where(r => r.CivilId == SpeakerInfoModelsession.civilid);
                         
                        if (Exists.Count() > 0)
                        {
                            //check if user have no res on minstry but can access as an admin         0020
                            var user_ = _context.Users.Find(Exists.FirstOrDefault().UserId);
                            if (user_.NationalityId.ToLower() == "kw" && (user_.UserRoleId != 1 && user_.UserRoleId != 3))
                            {
                                response.HasError = true;
                                response.Message = "لايمكنك استخدام هذة الخاصيه لتجديد الاقامة ";
                                response.Status = 400;
                                response.Response = null;
                                /// return message that youhave no permission to access this system   0020
                                return null;
                            }
                            if (user_.UserTypeId == 26 && (user_.UserRoleId != 1 && user_.UserRoleId != 3) )
                            {
                                response.HasError = true;
                                response.Message = "لايمكنك الدخول على النظام لان اقامتك ليست على الوزارة";
                                response.Status = 400;
                                response.Response = null;
                                /// return message that youhave no permission to access this system   0020
                                return response;
                            }
                            else
                            {
                                // return this user if admin or usual user                             0020
                                response.HasError = false;
                                response.Message = "";
                                response.Status = 200;
                                response.Response = ReturnSapUserFromDb(model.Username);
                                return response;
                            }
                        }
                        else
                        {
                            if (SpeakerInfoModelsession.employeetypeid == "ش1" || SpeakerInfoModelsession.nationalityid.ToLower()=="kw")
                            {
                                response.HasError = true;
                                response.Message = "لايمكنك الدخول على النظام لان اقامتك ليست على الوزارة";
                                response.Status = 400;
                                response.Response = null;
                                // return message you have no permission to access this system  0020
                                return response; 
                            }
                            //// save Sapuser to db 
                            ///Validate and migrate  
                            int SectionId = 0; 
                            if (SpeakerInfoModelsession.sectionid != null || SpeakerInfoModelsession.sectionid != 0)
                        {
                                var check = _context.Organizations.Where(r => r.SapId == SpeakerInfoModelsession.sectionid).Select(r => r.Id).ToList().FirstOrDefault();
                                if (check == null)
                                {
                                    Organization org= new Organization { Name = "", SapId = SpeakerInfoModelsession.sectionid, ParentId = SpeakerInfoModelsession.subdepartmentid };
                                    _context.Organizations.Add(org);
                                    _context.SaveChanges();
                                    SectionId = org.Id;
                                }else
                                {
                                    SectionId = check;
                                }
                            }

                            ///Validate and migrate UserType 
                            int UserTypeId = 0;
                            if (SpeakerInfoModelsession.employeetypeid != null || SpeakerInfoModelsession.employeetypeid != "")
                            {
                                var check = _context.UserTypes.Where(r => r.SapId == SpeakerInfoModelsession.employeetypeid).Select(r => r.UserTypeId).ToList().FirstOrDefault();
                                if (check == 0 ||check ==null)
                                {
                                    UserType UserType_ = new UserType { UserTypeName = SpeakerInfoModelsession.employeetype, SapId = SpeakerInfoModelsession.employeetypeid};
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
                                        int maxId = (int)_context.JobTypes.Max(r=>r.Id);
                                        JobType JobType_ = new JobType {Id=maxId+1, Name = SpeakerInfoModelsession.jobtitle, Sapid = SpeakerInfoModelsession.jobtitleid };
                                        _context.JobTypes.Add(JobType_);
                                        _context.SaveChanges();
                                        JobtypeId = (int)JobType_.Id;
                                    }
                                    else
                                    {
                                        JobType JobType_ = new JobType {Id= checkSapName, Name = SpeakerInfoModelsession.jobtitle, Sapid = SpeakerInfoModelsession.jobtitleid };
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
                            commonUserInfo_ = new commonUserInfo();
                            user_.IsActive = true;
                            user_.IsSapUser = true;
                            user_.Email = model.Username;
                            user_.NationalityId = SpeakerInfoModelsession.nationalityid;
                            user_.SectionId = SectionId;
                            user_.UserTypeId = UserTypeId;
                            user_.JobtypeId = JobtypeId;
                            user_.GenderId = SpeakerInfoModelsession.genderid;
                            user_.UserRoleId = 2;
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
                            commonUserInfo_.UserId = Sapuser_.UserId;
                            commonUserInfo_.MobileNumber = user_.MobileNumber;
                            commonUserInfo_.Email = user_.Email;
                            commonUserInfo_.ResidencyByMoa = user_.ResidencyByMoa;
                            commonUserInfo_.IsSapUser = user_.IsSapUser;
                            commonUserInfo_.IsActive = (bool)user_.IsActive;
                            commonUserInfo_.CivilId = Sapuser_.CivilId;
                            commonUserInfo_.EmployeeNumber = Sapuser_.EmployeeNumber;
                            commonUserInfo_.EmployeeName = Sapuser_.EmployeeName;
                            commonUserInfo_.EmployeeType = Sapuser_.EmployeeType;
                            commonUserInfo_.Sector = Sapuser_.Sector;
                            commonUserInfo_.Department = Sapuser_.Department;
                            commonUserInfo_.Section = Sapuser_.Section;
                            commonUserInfo_.SectionId = (int)user_.SectionId;
                            commonUserInfo_.NationalityId = user_.NationalityId;
                            commonUserInfo_.UserTypeId = (int)user_.UserTypeId;
                            commonUserInfo_.JobtypeId = (int)user_.JobtypeId;
                            commonUserInfo_.Nationality = _context.Nationalities.Where(r => r.NationalityId == SpeakerInfoModelsession.nationalityid).Select(r => r.NationalityName).FirstOrDefault().ToString();
                            commonUserInfo_.BirthDate = Sapuser_.BirthDate;
                            commonUserInfo_.JobTitle = Sapuser_.JobTitle;
                            commonUserInfo_.HireDate = Sapuser_.HireDate;
                            commonUserInfo_.UserRoleId =(int) user_.UserRoleId;
                            commonUserInfo_.Organization = SpeakerInfoModelsession.department;
                            commonUserInfo_.RegistrationStatusId = 0;
                            response.HasError = false;
                            response.Message = "";
                            response.Status = 200;
                            response.Response = commonUserInfo_;
                            return response;
                        }
                    }
                    else
                    {
                        var Exists = _context.Users.Where(r => r.Email == model.Username);
                        if (Exists.Count() > 0)
                        {
                            response.HasError = false;
                            response.Message = "";
                            response.Status = 200;
                            response.Response = ReturnSapUserFromDb(model.Username); ;
                            return response;
                        }
                        else
                        {

                            User user_ = new User();
                            SapUser Sapuser_ = new SapUser();
                            commonUserInfo_ = new commonUserInfo();
                            user_.IsActive = true;
                            user_.IsSapUser = true;
                            user_.Email = model.Username;
                            user_.UserRoleId = 2;
                            _context.Users.Add(user_);
                            _context.SaveChanges();
                            //// saving user to sap users table 
                            Sapuser_.EmployeeName = LDAPsession.DomainUserFullName;
                            Sapuser_.UserId = user_.UserId;
                            _context.SapUsers.Add(Sapuser_);
                            _context.SaveChanges();
                            commonUserInfo_.UserId = Sapuser_.UserId;
                            commonUserInfo_.MobileNumber = user_.MobileNumber;
                            commonUserInfo_.Email = user_.Email;
                            commonUserInfo_.ResidencyByMoa = user_.ResidencyByMoa;
                            commonUserInfo_.IsSapUser = user_.IsSapUser;
                            commonUserInfo_.IsActive = (bool)user_.IsActive;
                            commonUserInfo_.CivilId = Sapuser_.CivilId;
                            commonUserInfo_.EmployeeNumber = Sapuser_.EmployeeNumber;
                            commonUserInfo_.EmployeeName = Sapuser_.EmployeeName;
                            commonUserInfo_.EmployeeType = Sapuser_.EmployeeType;
                            commonUserInfo_.Sector = Sapuser_.Sector;
                            commonUserInfo_.Department = Sapuser_.Department;
                            commonUserInfo_.Section = Sapuser_.Section;
                            commonUserInfo_.BirthDate = Sapuser_.BirthDate;
                            commonUserInfo_.JobTitle = Sapuser_.JobTitle;
                            commonUserInfo_.HireDate = Sapuser_.HireDate;
                            commonUserInfo_.UserRoleId =(int) user_.UserRoleId;
                            commonUserInfo_.Organization = "";
                            commonUserInfo_.RegistrationStatusId = 0;
                            commonUserInfo_.IsAdmin = (bool)user_.IsAdmin;
                            response.HasError = false;
                            response.Message = "";
                            response.Status = 200;
                            response.Response = commonUserInfo_;
                            return response;
                        }
                    }
                }
                response.HasError = true;
                response.Message = "كلمة السر او المستخدم غير صحيحة";
                response.Status = 400;
                response.Response = null;
                return response;

            }
            else
            {
                var user_ = _context.Users.Where(r => r.Email == model.Username && r.IsActive == true).FirstOrDefault();
                if (user_ != null)
                {
                    var user__ = _context.NonSapUsers.Where(r => r.UserId == user_.UserId && r.Password == model.Password && r.RegistrationStatusId == 2).FirstOrDefault();
                    if (user__ != null)
                    {
                        response.HasError = false;
                        response.Message = "";
                        response.Status = 200;
                        response.Response = ReturnNonSapUserFromDb(model.Username);
                        return response;
                         

                    }
                    else
                    {
                        response.HasError = true;
                        response.Message = "كلمة السر او المستخدم غير صحيحة";
                        response.Status = 400;
                        response.Response = null;
                        return response;

                    }
                }


                response.HasError = true;
                response.Message = "كلمة السر او المستخدم غير صحيحة";
                response.Status = 400;
                response.Response = null;
                return response;


            }
        }
        async public Task<ResponseRequest<AuthenticateResponse>> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            try
            {
                response = new ResponseRequest<commonUserInfo>();
                AuthenticateResponse_ = new ResponseRequest<AuthenticateResponse>();
                response = AuthenticateUser(model);
                if (response.HasError)
                {
                    AuthenticateResponse_.HasError = true;
                    AuthenticateResponse_.Message = response.Message;
                    AuthenticateResponse_.Response = null;
                    return AuthenticateResponse_;
                }


                commonUserInfo usercommonInfo = response.Response;

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken(user: usercommonInfo);
                var refreshToken = generateRefreshToken(ipAddress);

                // save refresh token
                _context.RefreshTokens.Add(new EntityModels.RefreshToken { UserId = usercommonInfo.UserId, Token = refreshToken.Token, Expires = (DateTime)refreshToken.Expires, CreatedByIp = ipAddress });
                _context.SaveChanges();

                AuthenticateResponse_.HasError = false;
                AuthenticateResponse_.Message ="";
                AuthenticateResponse_.Response = new AuthenticateResponse(_user: usercommonInfo, jwtToken: jwtToken, refreshToken: refreshToken.Token);
                return AuthenticateResponse_;
            }
            catch (System.Exception ex)
            {
                AuthenticateResponse_.HasError = true;
                AuthenticateResponse_.Message = "حدث خطأ فى تسجيل الدخول ";
                AuthenticateResponse_.Response =null;
                return AuthenticateResponse_;
            }

        }
        async public Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            //var user = _context.Users.SingleOrDefault (u => u.RefreshTokens.Any (t => t.Token == token));
            //List<RefreshTokens> refreshtokens = await _context.getRefreshTokens(actionId: 2, token: token);
            List<RefreshToken> refreshtokens = _context.RefreshTokens.Where(r => r.Token == token).ToList<RefreshToken>();
            // return null if no user found with token
            if (refreshtokens == null) return null;

            RefreshToken refreshToken = refreshtokens.FirstOrDefault();

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            refreshToken.RevokedByIp = ipAddress;
            var user_ = from ru in _context.Users
                        join rsap in _context.SapUsers on ru.UserId equals rsap.UserId
                        join rna in _context.Nationalities on ru.NationalityId equals rna.NationalityId
                        select (new
                        {
                            userid = ru.UserId,
                            MobileNumber = ru.MobileNumber,
                            Email = ru.Email,
                            ResidencyByMoa = ru.ResidencyByMoa,
                            IsSapUser = ru.IsSapUser,
                            IsActive = ru.IsActive,
                            CivilId = rsap.CivilId,
                            EmployeeNumber = rsap.EmployeeNumber,
                            EmployeeName = rsap.EmployeeName,
                            EmployeeType = rsap.EmployeeType,
                            Sector = rsap.Sector,
                            Department = rsap.Department,
                            Section = rsap.Section,
                            Nationality = rna.NationalityName,
                            BirthDate = rsap.BirthDate,
                            JobTitle = rsap.JobTitle,
                            HireDate = rsap.HireDate,
                            Organization = "",
                            UserTypeId = 0,
                            RegistrationStatusId = 0
                        });
            _context.RefreshTokens.Add(new EntityModels.RefreshToken { UserId = refreshToken.UserId, Token = newRefreshToken.Token, Expires = (DateTime)refreshToken.Expires, CreatedByIp = ipAddress });
            _context.SaveChanges();


            // generate new jwt
            var jwtToken = generateJwtToken(refreshToken: refreshToken);
            return new AuthenticateResponse(_refreshToken: refreshToken, jwtToken: jwtToken, refreshToken: newRefreshToken.Token, _user: user_);
        }
        async public Task<object> RevokeToken(string token, string ipAddress)
        {
            List<RefreshToken> refreshtokens = _context.RefreshTokens.Where(r => r.Token == token).ToList<RefreshToken>();

            // return null if no user found with token
            if (refreshtokens.Count == 0) return null;

            RefreshToken refreshToken = refreshtokens.FirstOrDefault();

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.Revoked = DateTime.Now;
            _context.SaveChanges();
            return true;
        }
        private string generateJwtToken(RefreshToken refreshToken = null, commonUserInfo user = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                user == null ? new Claim (ClaimTypes.Name, refreshToken.UserId.ToString ()) : new Claim (ClaimTypes.Name, user.UserId.ToString ())
                }),
                Expires = DateTime.UtcNow.AddMinutes(600),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
        SpeakerInfoModels IJwtServices.ReturnSapUserFromDb(string UserName)
        {
            throw new NotImplementedException();
        }
   
    
    }
}
