using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using DomainModel.Model.com.vaps.Fee;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using DomainModel.Model.com.vaps.admission;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vapstech.MobileApp;
using CommonLibrary;
using DomainModel.Model.com.vapstech.College.Admission;
using System.IO;
using DomainModel.Model.com.vapstech.Portals.IVRS;
using DomainModel.Model.com.vapstech.HRMS;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Fee;
using Razorpay.Api;
using System.Dynamic;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using System.Text;
using DomainModel.Model.com.vapstech.Portals;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class LoginFacadeController : Controller
    {
        public logininterface log;
        public registration _reg;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DomainModelMsSqlServerContext _db;
        private readonly Enquirycontext _Enquirycontext;
        private readonly PortalContext _PortalContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        public LoginFacadeController(registration regi, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, DomainModelMsSqlServerContext db, Enquirycontext Enquirycontext, PortalContext _Portal, IHostingEnvironment hostingEnvironment) //, 
        {
            _reg = regi;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _Enquirycontext = Enquirycontext;
            _PortalContext = _Portal;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<LoginDTO> Post([FromBody] regis reg)
        {
            LoginDTO lgnDto = new LoginDTO();
            try
            {
                ApplicationUser user = new ApplicationUser();
                user = await _userManager.FindByNameAsync(reg.username);
                var getuserinstidata = new List<int>();
                List<Institution> schoolcollege = new List<Institution>();

                if (reg.Logintype == "KIOSK")
                {
                    // var usrid = _db.UserRoleWithInstituteDMO.Where(d => d.username == user.Id).ToList();

                    reg.MI_Id = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.MI_Id).FirstOrDefault();
                }
                long? expdays = 0;
                var pwdflg = "0";

                if (reg.MI_Id != 0)
                {
                    schoolcollege = _db.Institute.Where(t => t.MI_Id == reg.MI_Id).ToList();
                    lgnDto.schoolcollegeflag = schoolcollege.FirstOrDefault().MI_SchoolCollegeFlag;
                    lgnDto.MI_BackgroundImage = schoolcollege.FirstOrDefault().MI_BackgroundImage;
                    lgnDto.MI_Logo = schoolcollege.FirstOrDefault().MI_Logo;

                    expdays = _db.GenConfig.Where(s => s.MI_Id == reg.MI_Id).Select(m => m.IVRMGC_PasswordExpiryDuration).FirstOrDefault();
                    if (expdays == null)
                    {
                        expdays = 0;
                    }
                    pwdflg = _db.Institution.Where(s => s.MI_Id == reg.MI_Id).FirstOrDefault().MI_PasswordFlag;

                }
                else
                {
                    getuserinstidata = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.Id).ToList();
                    if (getuserinstidata.Count() > 0)
                    {
                        List<UserRoleWithInstituteDMO> Mi_id_list = new List<UserRoleWithInstituteDMO>();
                        Mi_id_list = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();
                        List<MI_ID_DTO> MI_ID_DTO_list = new List<MI_ID_DTO>();
                        for (int l = 0; l < Mi_id_list.Count; l++)
                        {
                            MI_ID_DTO MI_ID_DTO = new MI_ID_DTO();
                            MI_ID_DTO.MI_Id = Mi_id_list.FirstOrDefault().MI_Id;
                            lgnDto.MI_ID = Mi_id_list.FirstOrDefault().MI_Id;
                            MI_ID_DTO_list.Add(MI_ID_DTO);
                        }
                        lgnDto.Mi_Id_List = MI_ID_DTO_list;
                        lgnDto.MI_Idforlogin = lgnDto.Mi_Id_List[0].MI_Id;
                        schoolcollege = _db.Institute.Where(t => t.MI_Id == lgnDto.MI_Idforlogin).ToList();
                        lgnDto.schoolcollegeflag = schoolcollege.FirstOrDefault().MI_SchoolCollegeFlag;
                        lgnDto.instituteName = schoolcollege.FirstOrDefault().MI_Name;
                    }
                    else
                    {
                        lgnDto.message = "User Does Not Belong To This Institution!!";
                        return lgnDto;
                    }
                }
                if (lgnDto.schoolcollegeflag.ToString() == "S")
                {
                    if (reg.MI_Id != 0)
                    {
                        getuserinstidata = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id && d.MI_Id == reg.MI_Id).Select(d => d.Id).ToList();
                        expdays = _db.GenConfig.Where(s => s.MI_Id == reg.MI_Id).Select(m => m.IVRMGC_PasswordExpiryDuration).FirstOrDefault();
                        if (expdays == null)
                        {
                            expdays = 0;
                        }
                        pwdflg = _db.Institution.Where(s => s.MI_Id == reg.MI_Id).FirstOrDefault().MI_PasswordFlag;

                    }
                    else
                    {
                        getuserinstidata = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.Id).ToList();
                        long miid = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.MI_Id).FirstOrDefault();
                        expdays = _db.GenConfig.Where(s => s.MI_Id == miid).Select(m => m.IVRMGC_PasswordExpiryDuration).FirstOrDefault();
                        if (expdays == null)
                        {
                            expdays = 0;
                        }
                        pwdflg = _db.Institution.Where(s => s.MI_Id == miid).FirstOrDefault().MI_PasswordFlag;

                    }
                    if (getuserinstidata.Count() > 0)
                    {
                        var Leftflagcheck = (from a in _db.HR_Master_Employee_DMO
                                             from b in _db.Staff_User_Login
                                             where (a.HRME_Id == b.Emp_Code && b.Id == user.Id && a.HRME_LeftFlag == true)
                                             select a).ToList();
                        if (Leftflagcheck.Count() == 0)
                        {
                            var activeuserornot = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id && d.Activeflag == 1).Select(d => d.Id).ToList();
                            if (activeuserornot.Count() > 0)
                            {
                                if (user != null)
                                {
                                    //Date:15-12-2016 for email confirmation.
                                    if (_userManager.IsEmailConfirmedAsync(user).Result)
                                    {
                                        var passwordVerify = await _signInManager.PasswordSignInAsync(user.UserName, reg.password, false, lockoutOnFailure: false);

                                        if (passwordVerify.Succeeded)
                                        {
                                            //First Time Login And Chaging Default Password

                                            long AutoGeneratedNo = 0;
                                            try
                                            {
                                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "Get_IVRM_User_Login_Count_Details";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = user.Id });
                                                    cmd.Parameters.Add(new SqlParameter("@ROWCount", SqlDbType.VarChar, Int32.MaxValue)
                                                    { Direction = ParameterDirection.Output });
                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();

                                                    var data1 = cmd.ExecuteNonQuery();

                                                    AutoGeneratedNo = Convert.ToInt64(cmd.Parameters["@ROWCount"].Value.ToString());
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }

                                            //var fisrttime_login_details = _db.IVRM_MobileApp_LoginDetailsDMO.Where(a => a.IVRMUL_Id == user.Id).ToList();

                                            if (AutoGeneratedNo == 0 && (pwdflg == "1" || pwdflg != null))
                                            {
                                                lgnDto.message = "FirstTimeLogin";
                                            }
                                            else
                                            {
                                                int totaldays = 0;
                                                if (user.UpdatedDate != null && Convert.ToString(user.UpdatedDate) != "")
                                                {
                                                    DateTime dt1 = Convert.ToDateTime(user.UpdatedDate);
                                                    DateTime dt2 = DateTime.Now;
                                                    totaldays = (dt2 - dt1).Days;
                                                }

                                                if (user.UserName.Equals("vaps super admin", StringComparison.OrdinalIgnoreCase)
                                                    || user.UserName.Equals("sadmin", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    totaldays = 0;
                                                }

                                                //if (totaldays <= 30)
                                                if (expdays > 0)
                                                {
                                                    if (totaldays <= expdays)
                                                    {
                                                        var roleId = (from role in _db.appUserRole where role.UserId == user.Id select role.RoleTypeId);
                                                        lgnDto.roleId = roleId.First();
                                                        List<UserRoleWithInstituteDMO> id_list = new List<UserRoleWithInstituteDMO>();
                                                        id_list = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();
                                                        bool logintype = true;
                                                        var id = _db.MasterRoleType.Single(d => d.IVRMRT_Id == lgnDto.roleId);

                                                        if (id.IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            List<AlumniUserRegistrationDMO> alumnilist = new List<AlumniUserRegistrationDMO>();
                                                            alumnilist = (from a in _db.AlumniUserRegistrationDMO
                                                                          from b in _db.Alumni_User_LoginDMO
                                                                          where (a.ALSREG_Id == b.ALSREG_Id && a.MI_Id == id_list[0].MI_Id && b.IVRMUL_Id == user.Id && a.ALSREG_ApprovedFlag == true)
                                                                          select new AlumniUserRegistrationDMO
                                                                          {
                                                                              ALSREG_Id = a.ALSREG_Id,
                                                                              ALMST_Id = a.ALMST_Id
                                                                          }).ToList();
                                                            if (alumnilist.Count > 0)
                                                            {
                                                                lgnDto.ALMST_Id = Convert.ToInt64(alumnilist[0].ALMST_Id);
                                                            }

                                                            if (alumnilist.Count > 0)
                                                            {
                                                                logintype = true;
                                                            }
                                                            else
                                                            {
                                                                logintype = false;
                                                            }
                                                        }

                                                        if (logintype == true)
                                                        {
                                                            lgnDto.userId = user.Id;
                                                            lgnDto.UserName = user.UserName;
                                                            List<MI_ID_DTO> MI_ID_DTO_list = new List<MI_ID_DTO>();
                                                            MI_ID_DTO MI_ID_DTO = new MI_ID_DTO();
                                                            List<long> MobileId = new List<long>();
                                                            List<UserRoleWithInstituteDMO> Mi_id_list = new List<UserRoleWithInstituteDMO>();
                                                            Mi_id_list = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();
                                                            for (int l = 0; l < Mi_id_list.Count; l++)
                                                            {
                                                                MI_ID_DTO.MI_Id = Mi_id_list.FirstOrDefault().MI_Id;
                                                                lgnDto.MI_ID = Mi_id_list.FirstOrDefault().MI_Id;
                                                                MI_ID_DTO_list.Add(MI_ID_DTO);
                                                            }
                                                            if (Mi_id_list != null && Mi_id_list.Count > 1)
                                                            {
                                                               
                                                                for (int c = 0; c < Mi_id_list.Count; c++)
                                                                {
                                                                    MobileId.Add(Mi_id_list[c].MI_Id);
                                                                }
                                                                lgnDto.institutionlist = (from a in _db.Institute

                                                                                          where (MobileId.Contains(a.MI_Id))
                                                                                          select new MI_ID_DTO
                                                                                          {
                                                                                              MI_Id = a.MI_Id,

                                                                                          }).Distinct().ToArray();                                                               
                                                            }
                                                            else
                                                            {
                                                                lgnDto.institutionlist = MI_ID_DTO_list.ToArray();
                                                            }

                                                            if (reg.MI_Id == 0)
                                                            {
                                                                lgnDto.Mi_Id_List = MI_ID_DTO_list;
                                                                lgnDto.MI_Idforlogin = lgnDto.Mi_Id_List[0].MI_Id;
                                                            }
                                                            else
                                                            {
                                                                MI_ID_DTO.MI_Id = reg.MI_Id;
                                                                lgnDto.MI_ID = reg.MI_Id;
                                                                MI_ID_DTO_list.Add(MI_ID_DTO);
                                                                if (Mi_id_list != null && Mi_id_list.Count > 1)
                                                                {                                                                   
                                                                    lgnDto.Mi_Id_List = (from a in _db.Institute

                                                                                              where (MobileId.Contains(a.MI_Id))
                                                                                              select new MI_ID_DTO
                                                                                              {
                                                                                                  MI_Id = a.MI_Id,

                                                                                              }).Distinct().ToList();
                                                                }
                                                                else
                                                                {
                                                                    lgnDto.Mi_Id_List = MI_ID_DTO_list;
                                                                }
                                                                // 

                                                                lgnDto.MI_Idforlogin = lgnDto.Mi_Id_List[0].MI_Id;
                                                            }
                                                            var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(lgnDto.Mi_Id_List[0].MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                                                            var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                                                            var acd_name = _db.AcademicYear.Where(t => t.ASMAY_Id == acd_Id).Select(d => d.ASMAY_Year).FirstOrDefault();
                                                            if (acd_Id == 0)
                                                            {
                                                                lgnDto.message = "Kindly contact Administrator";
                                                            }
                                                            else
                                                            {
                                                                lgnDto.ASMAY_Id = acd_Id;
                                                            }
                                                            lgnDto.ASMAY_Id = acd_Id;
                                                            lgnDto.ASMAY_Year = acd_name;
                                                            var financilayear = _db.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.IMFY_Id).FirstOrDefault();
                                                            if (financilayear != 0)
                                                            {
                                                                lgnDto.IMFY_Id = financilayear;
                                                            }
                                                            Master_Institution_SubscriptionValidity Master_Institution_SubscriptionValidity = new Master_Institution_SubscriptionValidity();
                                                            Master_Institution_SubscriptionValidity Subscriptiondetails = _db.Master_Institution_SubscriptionValidity.Where(t => t.MI_Id.Equals(lgnDto.Mi_Id_List[0].MI_Id)).FirstOrDefault();
                                                            var rolelist = _db.MasterRoleType.Where(t => t.IVRMRT_Id == lgnDto.roleId).ToList();
                                                            lgnDto.roleforlogin = rolelist[0].IVRMRT_Role.ToString();
                                                            if (rolelist[0].IVRMRT_Role.Equals("Staff") || rolelist[0].IVRMRT_Role.Equals("HOD") || rolelist[0].IVRMRT_Role.Equals("Manager"))
                                                            {
                                                                var getempolyeeid = (from emp in _db.Staff_User_Login where emp.Id == user.Id select emp.Emp_Code);
                                                                lgnDto.empcode = getempolyeeid.First();
                                                                List<Institution> institutionanme2 = new List<Institution>();
                                                                institutionanme2 = _db.Institute.Where(t => t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id).ToList();
                                                                lgnDto.fillinstition1 = institutionanme2.ToArray();
                                                                List<MasterEmployee> p = new List<MasterEmployee>();
                                                                p = (from a in _db.HR_Master_Employee_DMO
                                                                     where (a.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && a.HRME_Id == lgnDto.empcode)
                                                                     select new MasterEmployee
                                                                     {
                                                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                                         HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                                         HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                                         HRME_Photo = a.HRME_Photo
                                                                     }).ToList();

                                                                if (p.Count() > 0)
                                                                {
                                                                    lgnDto.studname = ((p.FirstOrDefault().HRME_EmployeeFirstName == null ? "" : p.FirstOrDefault().HRME_EmployeeFirstName.ToUpper()) + " " + (p.FirstOrDefault().HRME_EmployeeMiddleName == null ? "" : p.FirstOrDefault().HRME_EmployeeMiddleName.ToUpper()) + " " + (p.FirstOrDefault().HRME_EmployeeLastName == null ? "" : p.FirstOrDefault().HRME_EmployeeLastName.ToUpper())).Trim().ToString();

                                                                    lgnDto.UserImagePath = p.FirstOrDefault().HRME_Photo;
                                                                }
                                                                if (reg.Logintype == "Mobile")
                                                                {
                                                                    var stureg = _db.HR_Master_Employee_DMO.Where(s => s.HRME_Id == lgnDto.empcode).ToList().FirstOrDefault();
                                                                    stureg.HRME_AppDownloadedDeviceId = reg.mobiledeviceid;
                                                                    lgnDto.HRME_EmployeeCode = stureg.HRME_EmployeeCode;
                                                                    _db.Update(stureg);
                                                                    _db.SaveChanges();
                                                                }
                                                            }
                                                            if (!rolelist[0].IVRMRT_Role.Equals("Student"))
                                                            {
                                                                List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                                                                Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == user.Id
                                                                && t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id).ToList();

                                                                var check_staff_user_count = _db.Staff_User_Login.Where(a => a.Id == user.Id).ToList();

                                                                if (check_staff_user_count.Count > 0)
                                                                {
                                                                    lgnDto.empcode = check_staff_user_count.FirstOrDefault().Emp_Code;
                                                                }

                                                                if (Staffmobileappprivileges.Count() > 0)
                                                                {
                                                                    lgnDto.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                       from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                       from UserRolePrivileges in _db.IVRM_User_MobileApp_Login_Privileges
                                                                                                       where (Mobilepage.IVRMMAP_AnalyticalFlag == false && MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                                                                       && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                                                                       && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                                                                       && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId
                                                                                                       && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id
                                                                                                       && UserRolePrivileges.IVRMUL_Id == user.Id
                                                                                                       && Mobilepage.IVRMMAP_ActiveFlg == true
                                                                                                       && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true
                                                                                                       && UserRolePrivileges.IVRMUMALP_ActiveFlg == true)
                                                                                                       select new LoginDTO
                                                                                                       {
                                                                                                           Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                           Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                           Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                           IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                                                                           IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                                                                           IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                                                                           IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                                                                       }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                                                                    lgnDto.Staffanalyticalprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                        from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                        where (Mobilepage.IVRMMAP_AnalyticalFlag == true && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true)
                                                                                                        select new LoginDTO
                                                                                                        {
                                                                                                            Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                            Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                            Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                            IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id
                                                                                                        }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                                                                    lgnDto.mobileprivileges = "true";
                                                                }
                                                                else
                                                                {
                                                                    lgnDto.mobileprivileges = "false";
                                                                }

                                                                if (!rolelist[0].IVRMRT_Role.Equals("Staff"))
                                                                {
                                                                    lgnDto.UserImagePath = user.UserImagePath;
                                                                }
                                                            }

                                                            lgnDto.flag = rolelist[0].flag;
                                                            //lgnDto.firsttimelogin = true;

                                                            // var moIDcount = _db.Organisation.Where(t => t.M == lgnDto.roleId).ToList();
                                                            var moId = _db.Institute.Where(t => t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && t.MI_ActiveFlag == 1).ToList();
                                                            int moIDActive = 0;

                                                            //  lgnDto.Institutedetails = _db.Institute.Where(t => t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && t.MI_ActiveFlag == 1).ToArray();
                                                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                            {
                                                                cmd.CommandText = "Institutedetails_AdminData";
                                                                cmd.CommandType = CommandType.StoredProcedure;
                                                                cmd.CommandTimeout = 0;
                                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = lgnDto.Mi_Id_List[0].MI_Id });
                                                                if (cmd.Connection.State != ConnectionState.Open)
                                                                    cmd.Connection.Open();
                                                                var retObject = new List<dynamic>();
                                                                try
                                                                {
                                                                    using (var dataReader = cmd.ExecuteReader())
                                                                    {
                                                                        while (dataReader.Read())
                                                                        {
                                                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                                                            {
                                                                                dataRow.Add(
                                                                                    dataReader.GetName(iFiled),
                                                                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                                                );
                                                                            }
                                                                            retObject.Add((ExpandoObject)dataRow);
                                                                        }
                                                                    }
                                                                    lgnDto.Institutedetails = retObject.ToArray();
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Console.WriteLine(ex.Message);
                                                                }
                                                            }
                                                            if (moId.Count > 0)
                                                            {
                                                                moIDActive = _db.Organisation.Where(t => t.MO_Id == moId.FirstOrDefault().MO_Id && t.MO_ActiveFlag == 1).Count();
                                                            }
                                                            //amstid
                                                            if (rolelist[0].IVRMRT_Role.Equals("Student"))
                                                            {
                                                                var studentid = (from stud in _db.StudentAppUserLoginDMO where stud.STD_APP_ID == user.Id select stud.AMST_ID);
                                                                lgnDto.AMST_Id = studentid.First();

                                                                List<Adm_M_Student> t = new List<Adm_M_Student>();

                                                                t = _db.Adm_M_Student.Where(g => g.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && g.AMST_Id == lgnDto.AMST_Id).ToList();

                                                                t = (from a in _db.Adm_M_Student
                                                                     where (a.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && a.AMST_Id == lgnDto.AMST_Id)
                                                                     select new Adm_M_Student
                                                                     {
                                                                         AMST_FirstName = a.AMST_FirstName,
                                                                         AMST_MiddleName = a.AMST_MiddleName,
                                                                         AMST_LastName = a.AMST_LastName,
                                                                         AMST_Photoname = a.AMST_Photoname
                                                                     }).ToList();

                                                                if (t.Count() > 0)
                                                                {
                                                                    lgnDto.studname = ((t.FirstOrDefault().AMST_FirstName == null ? "" : t.FirstOrDefault().AMST_FirstName.ToUpper())
                                                                        + (t.FirstOrDefault().AMST_MiddleName == null ? "" : " " + t.FirstOrDefault().AMST_MiddleName.ToUpper())
                                                                        + (t.FirstOrDefault().AMST_LastName == null ? "" : " " + t.FirstOrDefault().AMST_LastName.ToUpper())).
                                                                        Trim().ToString();

                                                                    lgnDto.UserImagePath = t.FirstOrDefault().AMST_Photoname;
                                                                }

                                                                List<IVRM_Role_MobileApp_Privileges> Staffmobileappprivileges = new List<IVRM_Role_MobileApp_Privileges>();
                                                                Staffmobileappprivileges = _db.IVRM_Role_MobileApp_Privileges.Where(f => f.IVRMRT_Id == lgnDto.roleId
                                                                && f.MI_ID == lgnDto.Mi_Id_List[0].MI_Id).ToList();

                                                                if (Staffmobileappprivileges.Count() > 0)
                                                                {
                                                                    lgnDto.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                       from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                       where (Mobilepage.IVRMMAP_AnalyticalFlag == false && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                                                                       && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId
                                                                                                       && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id
                                                                                                       && Mobilepage.IVRMMAP_ActiveFlg == true
                                                                                                       && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true)
                                                                                                       select new LoginDTO
                                                                                                       {
                                                                                                           Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                           Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                           Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                           IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                                                                           IVRMMAP_AddFlg = MobileRolePrivileges.IVRMMAP_AddFlg,
                                                                                                           IVRMMAP_UpdateFlg = MobileRolePrivileges.IVRMMAP_UpdateFlg,
                                                                                                           IVRMMAP_DeleteFlg = MobileRolePrivileges.IVRMMAP_DeleteFlg
                                                                                                       }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                                                                    lgnDto.Staffanalyticalprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                        from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                        where (Mobilepage.IVRMMAP_AnalyticalFlag == true && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true)
                                                                                                        select new LoginDTO
                                                                                                        {
                                                                                                            Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                            Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                            Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                            IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id
                                                                                                        }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                                                                    lgnDto.mobileprivileges = "true";
                                                                }
                                                                else
                                                                {
                                                                    lgnDto.mobileprivileges = "false";
                                                                }

                                                                if (reg.Logintype == "Mobile")
                                                                {
                                                                    var stureg = _db.Adm_M_Student.Single(s => s.AMST_Id == lgnDto.AMST_Id);
                                                                    stureg.AMST_AppDownloadedDeviceId = reg.mobiledeviceid;
                                                                    lgnDto.AMST_AdmNo = stureg.AMST_AdmNo;
                                                                    _db.Update(stureg);
                                                                    _db.SaveChanges();
                                                                }

                                                                //Check Student Current Year Details
                                                                var student_current_year_details = _db.School_Adm_Y_StudentDMO.Where(a => a.AMST_Id == lgnDto.AMST_Id
                                                                && a.ASMAY_Id == lgnDto.ASMAY_Id && a.AMAY_ActiveFlag == 1).Distinct().ToList();
                                                                if (student_current_year_details.Count == 0)
                                                                {
                                                                    var get_student_previous_year_details = _db.School_Adm_Y_StudentDMO.Where(a => a.AMST_Id == lgnDto.AMST_Id
                                                                    && a.AMAY_ActiveFlag == 1).OrderByDescending(a => a.ASYST_Id).Distinct().ToList();

                                                                    long asmay_id = get_student_previous_year_details.FirstOrDefault().ASMAY_Id;

                                                                    var previous_acd_name = _db.AcademicYear.Where(at => at.ASMAY_Id == asmay_id).Select(d =>
                                                                    d.ASMAY_Year).FirstOrDefault();

                                                                    lgnDto.ASMAY_Id = asmay_id;
                                                                    lgnDto.ASMAY_Year = previous_acd_name;
                                                                    if (get_student_previous_year_details.Count == 0)
                                                                    {
                                                                        var admdetails = _db.Adm_M_Student.Where(a => a.AMST_Id == lgnDto.AMST_Id && a.ASMAY_Id == lgnDto.ASMAY_Id).ToList();
                                                                        lgnDto.ASMCL_Id = admdetails[0].ASMCL_Id.Value;

                                                                        lgnDto.ASMS_Id = 0;

                                                                    }
                                                                    else
                                                                    {
                                                                        lgnDto.ASMCL_Id = get_student_previous_year_details[0].ASMCL_Id;
                                                                        lgnDto.ASMS_Id = get_student_previous_year_details[0].ASMS_Id;

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lgnDto.ASMCL_Id = student_current_year_details[0].ASMCL_Id;
                                                                    lgnDto.ASMS_Id = student_current_year_details[0].ASMS_Id;
                                                                }


                                                                if (lgnDto.ASMCL_Id > 0)
                                                                {
                                                                    lgnDto.ASMCL_ClassName = _db.School_M_Class.Where(y => y.ASMCL_Id == lgnDto.ASMCL_Id).Select(y => y.ASMCL_ClassName).FirstOrDefault();
                                                                }
                                                                if (lgnDto.ASMS_Id > 0)
                                                                {
                                                                    lgnDto.ASMS_SectionName = _db.School_M_Section.Where(z => z.ASMS_Id == lgnDto.ASMS_Id).Select(z => z.ASMC_SectionName).FirstOrDefault();


                                                                }




                                                                var get_student_previous_year_details_class = _db.School_Adm_Y_StudentDMO.Where(a => a.AMST_Id == lgnDto.AMST_Id
                                                                    && a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == lgnDto.ASMAY_Id).OrderByDescending(a => a.ASMCL_Id).Distinct().ToList();

                                                                if (get_student_previous_year_details_class.Count() > 0)
                                                                {
                                                                    lgnDto.clsnme = get_student_previous_year_details_class.FirstOrDefault().ASMCL_Id.ToString();
                                                                }

                                                            }
                                                            //amstid

                                                            if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                                                            {
                                                                lgnDto.subscriptionenddate = DateTime.Today.AddDays(1);

                                                                lgnDto.subscriptionFlag = true;
                                                            }
                                                            else
                                                            {
                                                                if (moId.Count == 0)
                                                                {
                                                                    lgnDto.disableflag = "INT";
                                                                    lgnDto.message = geterrormessage(lgnDto);
                                                                    return lgnDto;
                                                                }

                                                                if (moIDActive == 0)
                                                                {
                                                                    lgnDto.disableflag = "ORG";
                                                                    lgnDto.message = geterrormessage(lgnDto);
                                                                    return lgnDto;
                                                                }

                                                                lgnDto.subscriptionenddate = Convert.ToDateTime(Subscriptiondetails.MISV_ToDate);
                                                                DateTime curdate = DateTime.Now;
                                                                if (lgnDto.subscriptionenddate < curdate)
                                                                {
                                                                    lgnDto.disableflag = "SUBDIS";
                                                                    lgnDto.message = geterrormessage(lgnDto);
                                                                }

                                                                lgnDto.subscriptionFlag = Subscriptiondetails.MISV_ActiveFlag;
                                                                if (lgnDto.subscriptionFlag == false)
                                                                {
                                                                    lgnDto.disableflag = "SUBACTIVE";
                                                                    lgnDto.message = geterrormessage(lgnDto);
                                                                }
                                                            }

                                                            //.Select(d => d.ASMAY_Id).
                                                            //&& Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) >= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) <= Convert.ToDateTime(System.DateTime.Today.Date)
                                                        }
                                                        else
                                                        {
                                                            lgnDto.message = "Alumni";
                                                            lgnDto.roleId = 0;
                                                            return lgnDto;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lgnDto.message = "expired";
                                                    }
                                                }


                                                else
                                                {
                                                    lgnDto.message = "Password Expiry Days not Entered!";
                                                }

                                                //login History

                                                var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                                                //string myIP1 = remoteIpAddress.ToString();

                                                string netip = remoteIpAddress.ToString();

                                                string hostName = Dns.GetHostName();
                                                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                                                string myIP1 = ip_list.ToString();

                                                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                                                String sMacAddress = string.Empty;
                                                foreach (NetworkInterface adapter in nics)
                                                {
                                                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                                                    {
                                                        IPInterfaceProperties properties = adapter.GetIPProperties();
                                                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                                                    }
                                                }

                                                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                                                //_db.Database.ExecuteSqlCommand("insert_history @p0,@p1,@p2,@p3,@p4,@p5,@p6", reg.MI_Id, user.Id,
                                                //    sMacAddress, myIP1, "0", netip, indianTime);

                                                //if (reg.Logintype != "" && reg.Logintype != null)
                                                //{
                                                //    _db.Database.ExecuteSqlCommand("logintype_history @p0,@p1,@p2,@p3", reg.MI_Id, user.Id,
                                                //        indianTime, reg.Logintype);
                                                //}
                                                _db.Database.ExecuteSqlCommand("insert_history @p0,@p1,@p2,@p3,@p4,@p5,@p6", lgnDto.Mi_Id_List[0].MI_Id, user.Id,
                                                    sMacAddress, myIP1, "0", netip, indianTime);

                                                if (reg.Logintype != "" && reg.Logintype != null)
                                                {
                                                    _db.Database.ExecuteSqlCommand("logintype_history @p0,@p1,@p2,@p3", lgnDto.Mi_Id_List[0].MI_Id, user.Id,
                                                        indianTime, reg.Logintype);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lgnDto.message = " Password Is Incorrect";
                                        }
                                    }
                                    else
                                    {
                                        lgnDto.message = "Email Not Confirmed!";
                                    }
                                }
                                else
                                {
                                    lgnDto.message = " User Name Is Incorrect";
                                }
                            }
                            else
                            {
                                lgnDto.message = "Your Account Is Deactivated!!";
                            }
                        }
                        else
                        {
                            lgnDto.message = "Your Account Is Deactivated!!";
                        }
                    }
                    else
                    {
                        lgnDto.message = "User Does Not Belong To This Institution!!";
                    }
                }

                else if (lgnDto.schoolcollegeflag.ToString() == "C" || lgnDto.schoolcollegeflag.ToString() == "U")
                {
                    if (reg.MI_Id != 0)
                    {
                        getuserinstidata = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id && d.MI_Id == reg.MI_Id).Select(d => d.Id).ToList();
                        expdays = _db.GenConfig.Where(s => s.MI_Id == reg.MI_Id).Select(m => m.IVRMGC_PasswordExpiryDuration).FirstOrDefault();
                        if (expdays == null)
                        {
                            expdays = 0;
                        }
                        pwdflg = _db.Institution.Where(s => s.MI_Id == reg.MI_Id).FirstOrDefault().MI_PasswordFlag;

                    }
                    else
                    {
                        getuserinstidata = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.Id).ToList();
                        long miid = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.MI_Id).FirstOrDefault();
                        expdays = _db.GenConfig.Where(s => s.MI_Id == miid).Select(m => m.IVRMGC_PasswordExpiryDuration).FirstOrDefault();
                        if (expdays == null)
                        {
                            expdays = 0;
                        }
                        pwdflg = _db.Institution.Where(s => s.MI_Id == miid).FirstOrDefault().MI_PasswordFlag;

                    }


                    if (getuserinstidata.Count() > 0)
                    {
                        var activeuserornot = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id && d.Activeflag == 1).Select(d => d.Id).ToList();

                        if (activeuserornot.Count() > 0)
                        {
                            if (user != null)
                            {
                                //Date:15-12-2016 for email confirmation.
                                if (_userManager.IsEmailConfirmedAsync(user).Result)
                                {
                                    var passwordVerify = await _signInManager.PasswordSignInAsync(user.UserName, reg.password, false, lockoutOnFailure: false);

                                    if (passwordVerify.Succeeded)
                                    {
                                        //First Time Login And Chaging Default Password

                                        long AutoGeneratedNo = 0;
                                        try
                                        {
                                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "Get_IVRM_User_Login_Count_Details";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = user.Id });
                                                cmd.Parameters.Add(new SqlParameter("@ROWCount", SqlDbType.VarChar, Int32.MaxValue)
                                                { Direction = ParameterDirection.Output });
                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

                                                var data1 = cmd.ExecuteNonQuery();

                                                AutoGeneratedNo = Convert.ToInt64(cmd.Parameters["@ROWCount"].Value.ToString());
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                        //var fisrttime_login_details = _db.IVRM_MobileApp_LoginDetailsDMO.Where(a => a.IVRMUL_Id == user.Id).ToList();
                                        if (AutoGeneratedNo == 0 && (pwdflg == "1" || pwdflg != null))
                                        {
                                            lgnDto.message = "FirstTimeLogin";
                                        }
                                        else
                                        {
                                            int totaldays = 0;
                                            if (user.UpdatedDate != null && Convert.ToString(user.UpdatedDate) != "")
                                            {
                                                DateTime dt1 = Convert.ToDateTime(user.UpdatedDate);
                                                DateTime dt2 = DateTime.Now;
                                                totaldays = (dt2 - dt1).Days;
                                            }

                                            if (user.UserName.Equals("vaps super admin", StringComparison.OrdinalIgnoreCase)
                                                || user.UserName.Equals("sadmin", StringComparison.OrdinalIgnoreCase))
                                            {
                                                totaldays = 0;
                                            }

                                            //if (totaldays <= 30)

                                            if (expdays > 0)
                                            {
                                                if (totaldays <= expdays)
                                                {

                                                    var roleId = (from role in _db.appUserRole where role.UserId == user.Id select role.RoleTypeId);

                                                    lgnDto.roleId = roleId.First();

                                                    bool logintype = true;
                                                    List<UserRoleWithInstituteDMO> miidslist = new List<UserRoleWithInstituteDMO>();
                                                    miidslist = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();

                                                    var id = _db.MasterRoleType.Single(d => d.IVRMRT_Id == lgnDto.roleId);
                                                    if (id.IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        List<CLGAlumniUserRegistrationDMO> alumnilist = new List<CLGAlumniUserRegistrationDMO>();
                                                        alumnilist = (from a in _db.CLGAlumniUserRegistrationDMO
                                                                      from b in _db.CLGAlumni_User_LoginDMO
                                                                      where (a.ALCSREG_Id == b.ALCSREG_Id && a.MI_Id == miidslist[0].MI_Id && b.IVRMUL_Id == user.Id
                                                                      && a.ALCSREG_ApprovedFlag == true)
                                                                      select new CLGAlumniUserRegistrationDMO
                                                                      {
                                                                          ALCSREG_Id = a.ALCSREG_Id,
                                                                          AMCST_Id = a.AMCST_Id
                                                                      }).ToList();

                                                        if (alumnilist.Count > 0)
                                                        {
                                                            logintype = true;
                                                        }
                                                        else
                                                        {
                                                            logintype = false;
                                                        }
                                                    }
                                                    if (logintype == true)
                                                    {
                                                        lgnDto.userId = user.Id;

                                                        lgnDto.UserName = user.UserName;
                                                        List<MI_ID_DTO> MI_ID_DTO_list = new List<MI_ID_DTO>();
                                                        MI_ID_DTO MI_ID_DTO = new MI_ID_DTO();
                                                        List<UserRoleWithInstituteDMO> Mi_id_list = new List<UserRoleWithInstituteDMO>();
                                                        Mi_id_list = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();

                                                        for (int l = 0; l < Mi_id_list.Count; l++)
                                                        {
                                                            MI_ID_DTO.MI_Id = Mi_id_list.FirstOrDefault().MI_Id;
                                                            lgnDto.MI_ID = Mi_id_list.FirstOrDefault().MI_Id;
                                                            MI_ID_DTO_list.Add(MI_ID_DTO);
                                                        }

                                                        lgnDto.institutionlist = MI_ID_DTO_list.ToArray();

                                                        if (reg.MI_Id == 0)
                                                        {
                                                            lgnDto.Mi_Id_List = MI_ID_DTO_list;
                                                            lgnDto.MI_Idforlogin = lgnDto.Mi_Id_List[0].MI_Id;
                                                        }
                                                        else
                                                        {
                                                            MI_ID_DTO.MI_Id = reg.MI_Id;
                                                            lgnDto.MI_ID = reg.MI_Id;
                                                            MI_ID_DTO_list.Add(MI_ID_DTO);
                                                            lgnDto.Mi_Id_List = MI_ID_DTO_list;

                                                            lgnDto.MI_Idforlogin = lgnDto.Mi_Id_List[0].MI_Id;
                                                        }

                                                        //var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(lgnDto.IVRM_MI_Id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                                                        // var Mi_id = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.MI_Id).ToArray();

                                                        //var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(lgnDto.Mi_Id_List[0].MI_Id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                                                        var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(lgnDto.Mi_Id_List[0].MI_Id)
                                                        && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date)
                                                        && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d =>
                                                        d.ASMAY_Id).FirstOrDefault();

                                                        var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true
                                                        && t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                                                        var acd_name = _db.AcademicYear.Where(t => t.ASMAY_Id == acd_Id).Select(d => d.ASMAY_Year).FirstOrDefault();

                                                        if (acd_Id == 0)
                                                        {
                                                            lgnDto.message = "Kindly contact Administrator";
                                                        }
                                                        else
                                                        {
                                                            lgnDto.ASMAY_Id = acd_Id;
                                                        }

                                                        lgnDto.ASMAY_Id = acd_Id;
                                                        lgnDto.ASMAY_Year = acd_name;

                                                        Master_Institution_SubscriptionValidity Master_Institution_SubscriptionValidity = new Master_Institution_SubscriptionValidity();
                                                        Master_Institution_SubscriptionValidity Subscriptiondetails = _db.Master_Institution_SubscriptionValidity.Where(t => t.MI_Id.Equals(lgnDto.Mi_Id_List[0].MI_Id)).FirstOrDefault();

                                                        var rolelist = _db.MasterRoleType.Where(t => t.IVRMRT_Id == lgnDto.roleId).ToList();

                                                        lgnDto.roleforlogin = rolelist[0].IVRMRT_Role.ToString();

                                                        if (rolelist[0].IVRMRT_Role.Equals("Staff")  || rolelist[0].IVRMRT_Role.Equals("Principal"))
                                                        {
                                                            var getempolyeeid = (from emp in _db.Staff_User_Login where emp.Id == user.Id select emp.Emp_Code);
                                                            lgnDto.empcode = getempolyeeid.First();

                                                            List<Institution> institutionanme2 = new List<Institution>();
                                                            institutionanme2 = _db.Institute.Where(t => t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id).ToList();
                                                            lgnDto.fillinstition1 = institutionanme2.ToArray();

                                                            if (reg.Logintype == "Mobile")
                                                            {
                                                                var stureg = _db.HR_Master_Employee_DMO.Where(s => s.HRME_Id == lgnDto.empcode).ToList().FirstOrDefault();
                                                                stureg.HRME_AppDownloadedDeviceId = reg.mobiledeviceid;
                                                                _db.Update(stureg);
                                                                _db.SaveChanges();
                                                            }

                                                            List<MasterEmployee> p = new List<MasterEmployee>();
                                                            p = (from a in _db.HR_Master_Employee_DMO
                                                                 where (a.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && a.HRME_Id == lgnDto.empcode)
                                                                 select new MasterEmployee
                                                                 {
                                                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                                     HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                                     HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                                     HRME_Photo = a.HRME_Photo,
                                                                     HRME_EmployeeCode = a.HRME_EmployeeCode,

                                                                 }).ToList();

                                                            if (p.Count() > 0)
                                                            {
                                                                lgnDto.studname = ((p.FirstOrDefault().HRME_EmployeeFirstName == null ? "" : p.FirstOrDefault().HRME_EmployeeFirstName.ToUpper()) + " " + (p.FirstOrDefault().HRME_EmployeeMiddleName == null ? "" : p.FirstOrDefault().HRME_EmployeeMiddleName.ToUpper()) + " " + (p.FirstOrDefault().HRME_EmployeeLastName == null ? "" : p.FirstOrDefault().HRME_EmployeeLastName.ToUpper())).Trim().ToString();

                                                                lgnDto.UserImagePath = p.FirstOrDefault().HRME_Photo;
                                                                lgnDto.HRME_EmployeeCode = p.FirstOrDefault().HRME_EmployeeCode;

                                                            }

                                                            List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                                                            Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == user.Id
                                                            && t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id).ToList();

                                                            if (Staffmobileappprivileges.Count() > 0)
                                                            {

                                                                lgnDto.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                   from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                   from UserRolePrivileges in _db.IVRM_User_MobileApp_Login_Privileges
                                                                                                   where (Mobilepage.IVRMMAP_AnalyticalFlag == false && MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id && UserRolePrivileges.IVRMUL_Id == user.Id && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true && UserRolePrivileges.IVRMUMALP_ActiveFlg == true)
                                                                                                   select new LoginDTO
                                                                                                   {
                                                                                                       Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                       Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                       Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                       IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id
                                                                                                   }).OrderBy(d => d.IVRMRMAP_Id).ToArray();


                                                                lgnDto.Staffanalyticalprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                    from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                    where (Mobilepage.IVRMMAP_AnalyticalFlag == true && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true)
                                                                                                    select new LoginDTO
                                                                                                    {
                                                                                                        Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                        Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                        Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                        IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id
                                                                                                    }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                                                                lgnDto.mobileprivileges = "true";
                                                            }
                                                            else
                                                            {
                                                                lgnDto.mobileprivileges = "false";
                                                            }
                                                        }

                                                        lgnDto.flag = rolelist[0].flag;

                                                        // var moIDcount = _db.Organisation.Where(t => t.M == lgnDto.roleId).ToList();
                                                        var moId = _db.Institute.Where(t => t.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && t.MI_ActiveFlag == 1).ToList();
                                                        int moIDActive = 0;

                                                        if (moId.Count > 0)
                                                        {
                                                            moIDActive = _db.Organisation.Where(t => t.MO_Id == moId.FirstOrDefault().MO_Id && t.MO_ActiveFlag == 1).Count();
                                                        }
                                                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Institutedetails_AdminData";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.CommandTimeout = 0;
                                                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = lgnDto.Mi_Id_List[0].MI_Id });
                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();
                                                            var retObject = new List<dynamic>();
                                                            try
                                                            {
                                                                using (var dataReader = cmd.ExecuteReader())
                                                                {
                                                                    while (dataReader.Read())
                                                                    {
                                                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                                                        {
                                                                            dataRow.Add(
                                                                                dataReader.GetName(iFiled),
                                                                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                                            );
                                                                        }
                                                                        retObject.Add((ExpandoObject)dataRow);
                                                                    }
                                                                }
                                                                lgnDto.Institutedetails = retObject.ToArray();
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.WriteLine(ex.Message);
                                                            }
                                                        }

                                                        //amstid
                                                        if (rolelist[0].IVRMRT_Role.Equals("Student"))
                                                        {
                                                            var studentid = (from stud in _db.CollegeStudentlogin where stud.IVRMUL_Id == user.Id select stud.AMCST_Id);
                                                            lgnDto.AMST_Id = studentid.First();
                                                            List<Adm_Master_College_StudentDMO> tt = new List<Adm_Master_College_StudentDMO>();
                                                            List<Adm_Master_College_StudentDMO> t = new List<Adm_Master_College_StudentDMO>();

                                                            t = _db.Adm_Master_College_StudentDMO.Where(g => g.MI_Id == lgnDto.Mi_Id_List[0].MI_Id
                                                            && g.AMCST_Id == lgnDto.AMST_Id).ToList();

                                                            if (reg.Logintype == "Mobile")
                                                            {
                                                                var stureg = _db.Adm_Master_College_StudentDMO.Single(s => s.AMCST_Id == lgnDto.AMST_Id);
                                                                stureg.AMCST_AppDownloadedDeviceId = reg.mobiledeviceid;
                                                                lgnDto.AMST_AdmNo = stureg.AMCST_AdmNo;
                                                                _db.Update(stureg);
                                                                _db.SaveChanges();
                                                            }

                                                            t = (from a in _db.Adm_Master_College_StudentDMO
                                                                 where (a.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && a.AMCST_Id == lgnDto.AMST_Id)
                                                                 select new Adm_Master_College_StudentDMO
                                                                 {
                                                                     AMCST_FirstName = a.AMCST_FirstName,
                                                                     AMCST_MiddleName = a.AMCST_MiddleName,
                                                                     AMCST_LastName = a.AMCST_LastName,
                                                                     AMCST_StudentPhoto = a.AMCST_StudentPhoto,
                                                                     AMCST_AdmNo = a.AMCST_AdmNo
                                                                 }).ToList();

                                                            if (t.Count() > 0)
                                                            {
                                                                lgnDto.studname = ((t.FirstOrDefault().AMCST_FirstName == null ? "" : t.FirstOrDefault().AMCST_FirstName.ToUpper()) + " " + (t.FirstOrDefault().AMCST_MiddleName == null ? "" : t.FirstOrDefault().AMCST_MiddleName.ToUpper()) + " " + (t.FirstOrDefault().AMCST_LastName == null ? "" : t.FirstOrDefault().AMCST_LastName.ToUpper())).Trim().ToString();

                                                                lgnDto.UserImagePath = t.FirstOrDefault().AMCST_StudentPhoto;
                                                            }

                                                            long newasmayid = 0;
                                                            var asmay_year = (from a in _db.AcademicYear
                                                                              where a.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && a.ASMAY_ActiveFlag == 1 && a.ASMAY_To_Date >= DateTime.Now
                                                                              select new Adm_Master_College_StudentDMO
                                                                              {
                                                                                  ASMAY_Id = a.ASMAY_Id,


                                                                              }).OrderBy(s => s.ASMAY_Id).ToArray();




                                                            if (asmay_year.Length > 0)
                                                            {

                                                                for (long i = 0; i < asmay_year.Length; i++)
                                                                {
                                                                    var exist = (from z in _db.Adm_College_Yearly_StudentDMO
                                                                                 from y in _db.Adm_Master_College_StudentDMO
                                                                                 where (z.AMCST_Id == y.AMCST_Id && y.AMCST_SOL == "S" && y.AMCST_ActiveFlag == true && y.ASMAY_Id == asmay_year[i].ASMAY_Id && y.AMCST_Id == lgnDto.AMST_Id)
                                                                                 select y).ToArray();
                                                                    if (exist.Length > 0)
                                                                    {
                                                                        newasmayid = asmay_year[i].ASMAY_Id;
                                                                    }

                                                                }
                                                                if (newasmayid == 0)
                                                                {
                                                                    newasmayid = lgnDto.ASMAY_Id;
                                                                }
                                                            }


                                                            tt = (from a in _db.Adm_Master_College_StudentDMO
                                                                  from b in _db.Adm_College_Yearly_StudentDMO
                                                                  from c in _db.ClgMasterBranchDMO
                                                                  from d in _db.CLG_Adm_Master_SemesterDMO
                                                                  where (a.MI_Id == lgnDto.Mi_Id_List[0].MI_Id && a.AMCST_Id == lgnDto.AMST_Id && a.AMCST_Id == b.AMCST_Id && b.AMB_Id == c.AMB_Id && b.AMSE_Id == d.AMSE_Id && b.ASMAY_Id == newasmayid && b.ACYST_ActiveFlag == 1)
                                                                  select new Adm_Master_College_StudentDMO
                                                                  {
                                                                      AMCST_FirstName = c.AMB_BranchName,
                                                                      AMCST_MiddleName = d.AMSE_SEMName,

                                                                  }).ToList();

                                                            if (tt.Count > 0)
                                                            {
                                                                lgnDto.ASMCL_ClassName = tt.FirstOrDefault().AMCST_FirstName;
                                                                lgnDto.ASMS_SectionName = tt.FirstOrDefault().AMCST_MiddleName;
                                                            }

                                                            List<IVRM_Role_MobileApp_Privileges> Staffmobileappprivileges = new List<IVRM_Role_MobileApp_Privileges>();
                                                            Staffmobileappprivileges = _db.IVRM_Role_MobileApp_Privileges.Where(f => f.IVRMRT_Id == lgnDto.roleId
                                                            && f.MI_ID == lgnDto.Mi_Id_List[0].MI_Id).ToList();

                                                            if (Staffmobileappprivileges.Count() > 0)
                                                            {

                                                                lgnDto.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                   from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                   where (Mobilepage.IVRMMAP_AnalyticalFlag == false && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true)
                                                                                                   select new LoginDTO
                                                                                                   {
                                                                                                       Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                       Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                       Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                       IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id
                                                                                                   }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                                                                lgnDto.Staffanalyticalprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                                                                    from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                                                                    where (Mobilepage.IVRMMAP_AnalyticalFlag == true && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == lgnDto.roleId && MobileRolePrivileges.MI_ID == lgnDto.Mi_Id_List[0].MI_Id && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true)
                                                                                                    select new LoginDTO
                                                                                                    {
                                                                                                        Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                                                        Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                                                        Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                                                        IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id
                                                                                                    }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                                                                lgnDto.mobileprivileges = "true";
                                                            }
                                                            else
                                                            {
                                                                lgnDto.mobileprivileges = "false";
                                                            }
                                                        }
                                                        //amstid

                                                        if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
                                                        {
                                                            lgnDto.subscriptionenddate = DateTime.Today.AddDays(1);

                                                            lgnDto.subscriptionFlag = true;
                                                        }
                                                        else
                                                        {

                                                            if (moId.Count == 0)
                                                            {
                                                                lgnDto.message = "institution";
                                                                return lgnDto;
                                                            }

                                                            if (moIDActive == 0)
                                                            {
                                                                lgnDto.message = "organisation";
                                                                return lgnDto;
                                                            }

                                                            lgnDto.subscriptionenddate = Convert.ToDateTime(Subscriptiondetails.MISV_ToDate);

                                                            lgnDto.subscriptionFlag = Subscriptiondetails.MISV_ActiveFlag;
                                                        }
                                                        //.Select(d => d.ASMAY_Id).
                                                        //&& Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) >= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) <= Convert.ToDateTime(System.DateTime.Today.Date)
                                                    }
                                                    else
                                                    {
                                                        lgnDto.message = "Alumni";
                                                        lgnDto.roleId = 0;
                                                        return lgnDto;
                                                    }
                                                }
                                                else
                                                {
                                                    lgnDto.message = "expired";
                                                }
                                            }

                                            else
                                            {
                                                lgnDto.message = "Expiry days not entered!";
                                            }

                                            //login History
                                            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                                            //string myIP1 = remoteIpAddress.ToString();

                                            string netip = remoteIpAddress.ToString();

                                            string hostName = Dns.GetHostName();
                                            var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                                            string myIP1 = ip_list.ToString();

                                            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                                            String sMacAddress = string.Empty;
                                            foreach (NetworkInterface adapter in nics)
                                            {
                                                if (sMacAddress == String.Empty)// only return MAC Address from first card
                                                {
                                                    IPInterfaceProperties properties = adapter.GetIPProperties();
                                                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                                                }
                                            }

                                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                                            _db.Database.ExecuteSqlCommand("insert_history @p0,@p1,@p2,@p3,@p4,@p5,@p6", lgnDto.Mi_Id_List[0].MI_Id, user.Id, sMacAddress, myIP1, "0", netip, indianTime);


                                            if (reg.Logintype != "" && reg.Logintype != null)
                                            {
                                                _db.Database.ExecuteSqlCommand("logintype_history @p0,@p1,@p2,@p3", lgnDto.Mi_Id_List[0].MI_Id, user.Id, indianTime, reg.Logintype);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lgnDto.message = " Password Is Incorrect";
                                    }
                                }
                                else
                                {
                                    lgnDto.message = "Email Not Confirmed!";
                                }
                            }
                            else
                            {
                                lgnDto.message = " User Name Is Incorrect";
                            }
                        }
                        else
                        {
                            lgnDto.message = "Your Account Is Deactivated!!";
                        }
                    }
                    else
                    {
                        lgnDto.message = "User Does Not Belong To This Institution!!";
                    }
                }
            }
            catch (Exception ex)
            {
                lgnDto.message = " User Name Is Incorrect";
                Console.Write(ex.Message);
            }

            // user = await _userManager.FindByNameAsync(reg.username);
            //var verify = await _userManager.CheckPasswordAsync(user, reg.password);
            return lgnDto;  // changed return type on 5-11-2016
        }

        [Route("getinstitutionapi")]
        public CommonDTO getinstitutionapi([FromBody] CommonDTO API)
        {
            try
            {
                if (API.INSTITUTECODE != "" && API.INSTITUTECODE != null)
                {
                    List<MOBILE_INSTITUTION> VSchoolData = new List<MOBILE_INSTITUTION>();
                    VSchoolData = _db.MOBILE_INSTITUTION.Where(d => d.INT_NAME == API.INSTITUTECODE).ToList();
                    if (VSchoolData.Count() > 0)
                    {
                        var VSchoolDataa = _db.API_MOBILE.Where(d => d.INT_ID == VSchoolData.FirstOrDefault().INSTITUTIONID).ToArray();
                        API.mi_id = VSchoolData.FirstOrDefault().MI_ID;

                        API.INSTITUTION_NAME = VSchoolData.FirstOrDefault().INSTITUTION_NAME;

                        API.INSTITUTION_LOGO = VSchoolData.FirstOrDefault().INSTITUTION_LOGO;
                        if (VSchoolDataa.Count() > 0)
                        {
                            API.APIARRAY = VSchoolDataa.ToArray();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return API;
        }

        [Route("getinstitutionapiNew")]
        public CommonDTO getinstitutionapiNew([FromBody] CommonDTO API)
        {
            try
            {
                if (API.INSTITUTECODE != "" && API.INSTITUTECODE != null)
                {
                    List<IVRM_IVRS_ConfigurationDMO> VSchoolData = new List<IVRM_IVRS_ConfigurationDMO>();
                    VSchoolData = _db.IVRS_ConfigurationDMO.Where(d => d.IIVRSC_SchoolCode == API.INSTITUTECODE).ToList();
                    if (VSchoolData.Count() > 0)
                    {
                        var VSchoolDataa = _db.IVRM_Configuration_URLDMO.Where(d => d.IIVRSC_Id == VSchoolData.FirstOrDefault().IIVRSC_Id).ToArray();
                        API.mi_id = VSchoolData.FirstOrDefault().IIVRSC_MI_Id;

                        API.INSTITUTION_NAME = VSchoolData.FirstOrDefault().IIVRSC_SchoolName;

                        API.INSTITUTION_LOGO = VSchoolData.FirstOrDefault().IIVRSC_AppLogo;
                        if (VSchoolDataa.Count() > 0)
                        {
                            API.APIARRAY = VSchoolDataa.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return API;
        }

        [Route("mobilelogout")]
        public CommonDTO mobilelogout([FromBody] CommonDTO API)
        {
            try
            {
                if (API.flag.Equals("StudentLgout", StringComparison.OrdinalIgnoreCase))
                {
                    var stureg = _db.Adm_M_Student.Single(s => s.AMST_Id == API.amst_id);
                    stureg.AMST_AppDownloadedDeviceId = null;
                    _db.Update(stureg);
                    _db.SaveChanges();
                }
                else if (API.flag.Equals("StaffLogout", StringComparison.OrdinalIgnoreCase))
                {
                    var stureg = _db.HR_Master_Employee_DMO.Single(s => s.HRME_Id == API.amst_id);
                    stureg.HRME_AppDownloadedDeviceId = null;
                    _db.Update(stureg);
                    _db.SaveChanges();

                }
                else if (API.flag.Equals("StudentDeviceId", StringComparison.OrdinalIgnoreCase))
                {
                    var stureg = _db.Adm_M_Student.Single(s => s.AMST_Id == API.amst_id);
                    stureg.AMST_AppDownloadedDeviceId = API.Message;
                    _db.Update(stureg);
                    _db.SaveChanges();
                }
                else if (API.flag.Equals("StaffDeviceId", StringComparison.OrdinalIgnoreCase))
                {
                    var stureg = _db.HR_Master_Employee_DMO.Single(s => s.HRME_Id == API.amst_id);
                    stureg.HRME_AppDownloadedDeviceId = API.Message;
                    _db.Update(stureg);
                    _db.SaveChanges();

                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return API;
        }

        [Route("mobilelogoutcollege")]
        public CommonDTO mobilelogoutcollege([FromBody] CommonDTO API)
        {
            try
            {
                if (API.flag.Equals("StudentLgout", StringComparison.OrdinalIgnoreCase))
                {
                    var stureg = _db.Adm_Master_College_StudentDMO.Single(s => s.AMCST_Id == API.amst_id);
                    stureg.AMCST_AppDownloadedDeviceId = null;
                    _db.Update(stureg);
                    _db.SaveChanges();
                }
                else if (API.flag.Equals("StaffLogout", StringComparison.OrdinalIgnoreCase))
                {
                    var stureg = _db.HR_Master_Employee_DMO.Where(s => s.HRME_Id == API.amst_id).ToList().FirstOrDefault();
                    stureg.HRME_AppDownloadedDeviceId = null;
                    _db.Update(stureg);
                    _db.SaveChanges();

                }
                else if (API.flag.Equals("StudentDeviceId", StringComparison.OrdinalIgnoreCase))
                {
                    var stureg = _db.Adm_Master_College_StudentDMO.Single(s => s.AMCST_Id == API.amst_id);
                    stureg.AMCST_AppDownloadedDeviceId = API.Message;
                    _db.Update(stureg);
                    _db.SaveChanges();
                }
                else if (API.flag.Equals("StaffDeviceId", StringComparison.OrdinalIgnoreCase))
                {

                    var stureg = _db.HR_Master_Employee_DMO.Where(s => s.HRME_Id == API.amst_id).ToList().FirstOrDefault();
                    stureg.HRME_AppDownloadedDeviceId = API.Message;
                    _db.Update(stureg);
                    _db.SaveChanges();

                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return API;
        }

        [Route("uploadtoazuremobile")]
        public async Task<CommonDTO> uploadtoazuremobile([FromBody] CommonDTO fileupload)
        {
            string newImageNamePath = "";
            string newImageName = "";
            string accountname = "";
            string accesskey = "";
            try
            {
                var data = _db.IVRM_Storage_path_Details.ToList();
                if (data.Count() > 0)
                {
                    accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                }
                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(fileupload.containername);
                string fileExt = "";
                if (fileupload.file == "image/jpeg")
                {
                    fileExt = ".jpg";
                }
                if (fileupload.file == "application/pdf")
                {
                    fileExt = ".pdf";
                }
                if (fileupload.file == "application/kswps")
                {
                    fileExt = ".pdf";
                }
                else if (fileupload.file == "image/png")
                {
                    fileExt = ".png";
                }
                else if (fileupload.file == "video/mp4")
                {
                    fileExt = ".mp4";
                }
                else if (fileupload.file == "audio/mp3")
                {
                    fileExt = ".mp3";
                }
                else if (fileupload.file == "audio/mpeg")
                {
                    fileExt = ".mp3";
                }
                else if (fileupload.file == "application/msword")
                {
                    fileExt = ".docx";
                }
                else if (fileupload.file == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    fileExt = ".docx";
                }
                else if (fileupload.file == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    fileExt = ".xls";
                }
                else if (fileupload.file == "application/vnd.ms-excel")
                {
                    fileExt = ".xlx";
                }
                var ext = fileExt.ToLower();
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + ext;
                string blobName = newImageName;
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                var blockBlob = container.GetBlockBlobReference(fileupload.mi_id + "/" + fileupload.folder + "/" + blobName);
                var Base64 = fileupload.Base64string;
                string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                convert2 = convert2.Replace("data:" + fileupload.file + ";base64,", String.Empty);
                convert2 = convert2.Replace(" ", "+");
                int mod4 = convert2.Length % 4;
                if (mod4 > 0)
                {
                    convert2 += new string('=', 4 - mod4);
                }
                var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                using (var stream = new MemoryStream(bytes))
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }
                newImageNamePath = blockBlob.Uri.ToString();
                fileupload.Message = newImageNamePath;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return fileupload;
        }

        [Route("HomeworkUpload")]
        public async Task<List<filedetails>> HomeworkkUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "HomeworkUpload", fileDescriptionShort.MI_Id);

                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }

        [Route("ClassworkUpload")]
        public async Task<List<filedetails>> ClassworkUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "ClassworkUpload", fileDescriptionShort.MI_Id);

                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }

        [Route("NoticeUpload")]
        public async Task<List<filedetails>> NoticeUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "NoticeUpload", fileDescriptionShort.MI_Id);

                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }

        [Route("InteractionUpload")]
        public async Task<List<filedetails>> InteractionUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "InteractionUpload", fileDescriptionShort.MI_Id);

                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }


        //[Route("HomeworkUpload")]
        //public async Task<List<string>> HomeworkkUpload(FileDescriptionDTO fileDescriptionShort)
        //{
        //    var names = new List<string>();
        //    var contentTypes = new List<string>();

        //    string newImageName = "";
        //    List<string> FilesPaths = new List<string>();
        //    StudentApplicationDTO stu = new StudentApplicationDTO();


        //    if (ModelState.IsValid)
        //    {
        //        foreach (var file in fileDescriptionShort.File)
        //        {
        //            if (file.Length > 0)
        //            {
        //                try
        //                {
        //                    string containername = "files";
        //                    newImageName = await uploadtoazure_1(file, containername, "HomeworkUpload", fileDescriptionShort.MI_Id);
        //                    FilesPaths.Add(newImageName);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //    }
        //    return FilesPaths;
        //}

        //[Route("ClassworkUpload")]
        //public async Task<List<string>> ClassworkUpload(FileDescriptionDTO fileDescriptionShort)
        //{
        //    var names = new List<string>();
        //    var contentTypes = new List<string>();

        //    string newImageName = "";
        //    List<string> FilesPaths = new List<string>();
        //    StudentApplicationDTO stu = new StudentApplicationDTO();


        //    if (ModelState.IsValid)
        //    {
        //        foreach (var file in fileDescriptionShort.File)
        //        {
        //            if (file.Length > 0)
        //            {
        //                try
        //                {
        //                    string containername = "files";
        //                    newImageName = await uploadtoazure_1(file, containername, "ClassworkUpload", fileDescriptionShort.MI_Id);
        //                    FilesPaths.Add(newImageName);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //    }
        //    return FilesPaths;
        //}
        public async Task<string> uploadtoazure_1(IFormFile file, string containername, string folder, long MI_Id)
        {
            string newImageNamePath = "";
            string newImageName = "";
            var newFileName = string.Empty;
            var fileName = string.Empty;
            var nameofthefile = "";
            string accountname = "";
            string accesskey = "";

            try
            {
                if (file.Length > 0)
                {
                    string fileExt = "";
                    if (file.ContentType == "image/jpeg")
                    {
                        fileExt = ".jpg";
                    }
                    if (file.ContentType == "image/jpg")
                    {
                        fileExt = ".jpg";
                    }
                    if (file.ContentType == "application/pdf")
                    {
                        fileExt = ".pdf";
                    }
                    if (file.ContentType == "application/kswps")
                    {
                        fileExt = ".pdf";
                    }
                    else if (file.ContentType == "image/png")
                    {
                        fileExt = ".png";
                    }
                    else if (file.ContentType == "video/mp4")
                    {
                        fileExt = ".mp4";
                    }
                    else if (file.ContentType == "audio/mp3")
                    {
                        fileExt = ".mp3";
                    }
                    else if (file.ContentType == "audio/mpeg")
                    {
                        fileExt = ".mp3";
                    }
                    else if (file.ContentType == "video/x-ms-wmv")
                    {
                        fileExt = ".wmv";
                    }
                    else if (file.ContentType == "application/vnd.ms-excel")
                    {
                        fileExt = ".xls";
                    }
                    else if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        fileExt = ".xlsx";
                    }
                    else if (file.ContentType == "application/msword")
                    {
                        fileExt = ".doc";
                    }
                    else if (file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                    {
                        fileExt = ".docx";
                    }
                    else if (file.ContentType == "application/vnd.ms-powerpoint")
                    {
                        fileExt = ".ppt";
                    }
                    else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
                    {
                        fileExt = ".pptx";
                    }
                    else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.slideshow")
                    {
                        fileExt = ".ppsx";
                    }
                    if (fileExt == ".jpg" || fileExt == ".png")
                    {
                        //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                        //uploads = Path.GetPathRoot("/UploadImages");
                        //uploads = Path.GetFullPath("UploadImages");

                        var exePath = Path.GetDirectoryName(System.Reflection
                           .Assembly.GetExecutingAssembly().CodeBase);

                        Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                        var appRoot = appPathMatcher.Match(exePath).Value;
                        appRoot = appRoot.Replace("WebApplication1", "IVRMUX");
                        var uploads = Path.Combine(appRoot, "wwwroot\\UploadImages");


                        fileName = file.FileName;
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            nameofthefile = fileName;
                        }
                        fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        fileName = Path.Combine(appRoot, "wwwroot\\UploadImages") + $@"\{fileName}";
                    }
                    var data = _db.IVRM_Storage_path_Details.ToList();
                    if (data.Count() > 0)
                    {
                        accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                        accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                    }
                    StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                    BlobRequestOptions requestoptions = new BlobRequestOptions()
                    {
                        SingleBlobUploadThresholdInBytes = 1024 * 1024 * 50, //50MB
                        ParallelOperationThreadCount = 12,
                    };

                    CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                    CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(containername);
                    var ext = fileExt.ToLower();
                    var guid = Guid.NewGuid().ToString();
                    newImageName = guid + ext;
                    string blobName = newImageName;
                    await container.CreateIfNotExistsAsync();
                    await container.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                    var blockBlob = container.GetBlockBlobReference(MI_Id + "/" + folder + "/" + blobName);
                    if (fileExt == ".jpg" || fileExt == ".png")
                    {
                        const int size = 900;
                        const int quality = 100;
                        using (var image = new MagickImage(fileName))
                        {
                            image.Resize(size, size);
                            //image.Rotate(90d);
                            image.AutoOrient();
                            image.Strip();
                            image.Quality = quality;
                            image.Write(fileName);
                        }
                        await blockBlob.UploadFromFileAsync(fileName);
                        newImageNamePath = blockBlob.Uri.ToString();
                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }
                    }
                    else
                    {
                        using (var fileStream = file.OpenReadStream())
                        {
                            await blockBlob.UploadFromStreamAsync(fileStream);
                        }
                        newImageNamePath = blockBlob.Uri.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }

        [Route("Mobileappotp")]
        public async Task<CommonDTO> Mobileappotp([FromBody] CommonDTO API)
        {
            CommonDTO lgnDto = new CommonDTO();

            string genotp = "";
            CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
            genotp = generate.getOTP();


            if (genotp.Length < 6)
            {
                genotp = generate.getOTP();
            }
            else
            {
                SMS sms = new SMS(_db);

                string s = await sms.sendSms(API.mi_id, Convert.ToInt64(API.mobileNo), "EMAILOTP", Convert.ToInt64(genotp));

                lgnDto.displaymessage = genotp;
                lgnDto.Message = "Please check Your SMS, OTP Is Sent To Your Mobile Number";
            }

            lgnDto.displaymessage = genotp;
            // user = await _userManager.FindByNameAsync(reg.username);
            //var verify = await _userManager.CheckPasswordAsync(user, reg.password);
            return lgnDto;  // changed return type on 5-11-2016
        }

        [Route("getMIdata")]
        public async Task<LoginDTO> getMIdata([FromBody] regis reg) // changed return type on 5-11-2016
        {
            LoginDTO lgnDto = new LoginDTO();
            try
            {
                ApplicationUser user = new ApplicationUser();
                user = await _userManager.FindByNameAsync(reg.username);

                if (user != null)
                {
                    var passwordVerify = await _signInManager.PasswordSignInAsync(user.UserName, reg.password, false, lockoutOnFailure: false);

                    if (passwordVerify.Succeeded)
                    {

                        var MIId = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.MI_Id).ToArray().ToArray();

                        var MIdata = _db.Institution.Where(d => MIId.Contains(d.MI_Id)).ToArray().ToArray();

                        lgnDto.MIdata = MIdata.ToArray();

                    }
                    else
                    {
                        //lgnDto.message = " Password Is Incorrect";
                        return lgnDto;
                    }
                }
                else
                {
                    //lgnDto.message = " User Name Is Incorrect";
                    return lgnDto;
                }

            }
            catch (Exception ex)
            {
                lgnDto.message = " User Name Is Incorrect";
                Console.Write(ex.Message);
            }

            // user = await _userManager.FindByNameAsync(reg.username);
            //var verify = await _userManager.CheckPasswordAsync(user, reg.password);
            return lgnDto;  // changed return type on 5-11-2016
        }

        [Route("getconfig")]
        public LoginDTO getconfig([FromBody] CommonDTO reg)
        {
            LoginDTO lgnDto = new LoginDTO();
            try
            {
                List<GeneralConfigDMO> ivrmgencon = new List<GeneralConfigDMO>();
                ivrmgencon = _db.GenConfig.Where(t => t.MI_Id == reg.mi_id).ToList();
                lgnDto.ivrmconfiglist = ivrmgencon.ToArray();

                List<Institution> lorgins = new List<Institution>();
                lorgins = _db.Institute.Where(t => t.MI_Id == reg.mi_id).ToList();
                lgnDto.fillinstitution = lorgins.ToArray();

                List<Institution_Phone_No> phn = new List<Institution_Phone_No>();
                List<Institution_EmailId> email = new List<Institution_EmailId>();

                email = _db.Institution_EmailId.AsNoTracking().Where(t => t.MI_Id.Equals(reg.mi_id)).ToList();
                lgnDto.EmailarrayList = email.ToArray();


                phn = _db.Institution_Phone_No.AsNoTracking().Where(t => t.MI_Id.Equals(reg.mi_id)).ToList();
                lgnDto.PhonearrayList = phn.ToArray();


                //var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(reg.mi_id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                List<MasterAcademic> allyearget = new List<MasterAcademic>();
                List<MasterAcademic> allyeargetlogin = new List<MasterAcademic>();
                List<AdmissionClass> allclasslogin = new List<AdmissionClass>();

                allyearget = (from a in _db.AcademicYear
                              where (a.MI_Id == reg.mi_id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == reg.mi_id)
                              select new MasterAcademic
                              {
                                  ASMAY_Id = a.ASMAY_Id,
                                  ASMAY_Year = a.ASMAY_Year,
                                  ASMAY_Order = a.ASMAY_Order,
                                  ASMAY_PreAdm_F_Date = a.ASMAY_PreAdm_F_Date,
                                  ASMAY_PreAdm_T_Date = a.ASMAY_PreAdm_T_Date
                              }
                    ).OrderByDescending(d => d.ASMAY_Order).ToList();

                if (allyearget.Count > 0)
                {
                    lgnDto.prestartdate = allyearget.FirstOrDefault().ASMAY_PreAdm_F_Date;
                    lgnDto.presenddate = allyearget.FirstOrDefault().ASMAY_PreAdm_T_Date;
                    lgnDto.CreatedDate = indianTime;
                }

                lgnDto.allyeargetlogin = (from a in _db.AcademicYear
                                          where (a.MI_Id == reg.mi_id && a.MI_Id == reg.mi_id)
                                          select new MasterAcademic
                                          {
                                              ASMAY_Id = a.ASMAY_Id,
                                              ASMAY_Year = a.ASMAY_Year,
                                              ASMAY_Order = a.ASMAY_Order
                                          }
                   ).OrderByDescending(d => d.ASMAY_Order).ToArray();

                lgnDto.allclasslogin = (from a in _db.admissioncls
                                        where (a.MI_Id == reg.mi_id)
                                        select new AdmissionClass
                                        {
                                            ASMCL_Id = a.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMCL_Order = a.ASMCL_Order
                                        }
                 ).ToArray();

                allyear = (from a in _db.AcademicYear
                           where (a.MI_Id == reg.mi_id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                           select new MasterAcademic
                           {
                               ASMAY_Id = a.ASMAY_Id,
                               ASMAY_Year = a.ASMAY_Year,
                               ASMAY_Order = a.ASMAY_Order
                           }
                  ).OrderByDescending(d => d.ASMAY_Order).ToList();
                if (allyear.Count() > 0)
                {
                    lgnDto.registration = true;
                }
                else
                {
                    lgnDto.registration = false;
                }


                var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == reg.mi_id).Select(d => d.ASMAY_Id).FirstOrDefault();

                if (Acdemic_preadmission == 0)
                {
                    Acdemic_preadmission = reg.ASMAY_Id;
                }

                List<MasterConfiguration> configurationsettings = new List<MasterConfiguration>();
                configurationsettings = _db.mstConfig.Where(t => t.ASMAY_Id == Acdemic_preadmission && t.MI_Id == reg.mi_id).ToList();

                if (configurationsettings.Count() > 0)
                {
                    if (configurationsettings[0].ISPAC_RegFeeFlag == 1)
                    {
                        lgnDto.payment = true;
                    }
                    else
                    {
                        lgnDto.payment = false;
                    }
                }




                //ApplicationUser user = new ApplicationUser();
                //user = await _userManager.FindByNameAsync(reg.username);

                //if (user != null)
                //{
                //    var passwordVerify = await _signInManager.PasswordSignInAsync(user.UserName, reg.password, false, lockoutOnFailure: false);

                //    if (passwordVerify.Succeeded)
                //    {

                //        var MIId = _db.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).Select(d => d.MI_Id).ToArray().ToArray();

                //        var MIdata = _db.Institution.Where(d => MIId.Contains(d.MI_Id)).ToArray().ToArray();

                //        lgnDto.MIdata = MIdata.ToArray();

                //    }
                //    else
                //    {
                //        lgnDto.message = " Password Is Incorrect";
                //    }
                //}
                //else
                //{
                //    lgnDto.message = " User Name Is Incorrect";
                //}

            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }

            // user = await _userManager.FindByNameAsync(reg.username);
            //var verify = await _userManager.CheckPasswordAsync(user, reg.password);
            return lgnDto;  // changed return type on 5-11-2016
        }

        [Route("getconfigTrust")]
        public LoginDTO getconfigTrust([FromBody] CommonDTO reg)
        {
            LoginDTO lgnDto = new LoginDTO();
            try
            {
                List<Organisation> lorgins = new List<Organisation>();
                lorgins = _db.Organisation.Where(t => t.MT_Domain_name == reg.hostname).ToList();
                lgnDto.fillinstitution = lorgins.ToArray();

                List<OrganisationPhone> phn = new List<OrganisationPhone>();
                List<OrganisationEmail> email = new List<OrganisationEmail>();

                if (lgnDto.fillinstitution.Length > 0)
                {
                    email = _db.OrganisationEmail.AsNoTracking().Where(t => t.MO_Id == lorgins.FirstOrDefault().MO_Id).ToList();
                    lgnDto.EmailarrayList = email.ToArray();


                    phn = _db.OrganisationPhone.AsNoTracking().Where(t => t.MO_Id == lorgins.FirstOrDefault().MO_Id).ToList();
                    lgnDto.PhonearrayList = phn.ToArray();

                    //List<Institution> institutiondetails = new List<Institution>();
                    //institutiondetails = _db.Institution.Where(t => t.MO_Id == lorgins.FirstOrDefault().MO_Id).ToList();
                    //lgnDto.institutiondetails = institutiondetails.ToArray();

                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    lgnDto.institutiondetails = (from a in _db.AcademicYear
                                                 from b in _db.Institution
                                                 where (a.MI_Id == b.MI_Id && b.MO_Id == lorgins.FirstOrDefault().MO_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Pre_ActiveFlag == 1)
                                                 select new LoginDTO
                                                 {
                                                     MI_ID = b.MI_Id,
                                                     MI_Name = b.MI_Name
                                                 }
               ).OrderBy(t => t.MI_ID).ToArray();
                }
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }

            // user = await _userManager.FindByNameAsync(reg.username);
            //var verify = await _userManager.CheckPasswordAsync(user, reg.password);
            return lgnDto;  // changed return type on 5-11-2016
        }

        [Route("getMIdataMaster")]
        public LoginDTO getMIdataMaster([FromBody] regis reg)  // changed return type on 5-11-2016
        {

            LoginDTO lgnDto = new LoginDTO();

            var MIId = _db.UserRoleWithInstituteDMO.Where(d => d.Id == reg.User_Id).Select(d => d.MI_Id).ToArray().ToArray();

            var MIdata = _db.Institution.Where(d => MIId.Contains(d.MI_Id)).ToArray().ToArray();

            lgnDto.MIdata = MIdata.ToArray();


            // user = await _userManager.FindByNameAsync(reg.username);
            //var verify = await _userManager.CheckPasswordAsync(user, reg.password);
            return lgnDto;  // changed return type on 5-11-2016
        }

        [Route("getsiblinglist")]
        public LoginDTO getsiblinglist([FromBody] regis reg)  // changed return type on 5-11-2016
        {

            LoginDTO lgnDto = new LoginDTO();

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "GET_LOGIN_STUDENT_SIBLINGS";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
            SqlDbType.BigInt)
                {
                    Value = reg.AMST_Id
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    lgnDto.siblinglist = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return lgnDto;
        }

        [Route("getRoleData")]
        public async Task<LoginDTO> getData([FromBody] CommonDTO cmn)
        {
            LoginDTO lgData = new LoginDTO();
            lgData.chleft = "";
            lgData.PaymentNootificationGeneral = cmn.PaymentNootificationGeneral;
            lgData.smscreditalert = cmn.smscreditalert;

            PreadmisionLoginHistory history = new PreadmisionLoginHistory();
            List<Institution> schoolcollege = new List<Institution>();
            if (cmn.IVRM_MI_Id != 0)
            {
                schoolcollege = _db.Institute.Where(f => f.MI_Id == cmn.IVRM_MI_Id).ToList();

                lgData.schoolcollegeflag = schoolcollege.FirstOrDefault().MI_SchoolCollegeFlag;
            }

            try
            {
                if (cmn.roleId != 0 && cmn.IVRM_MI_Id != 0)
                {

                    var rolelist = _db.MasterRoleType.Where(t => t.IVRMRT_Id == cmn.roleId).ToList();

                    var MIId = _db.UserRoleWithInstituteDMO.Where(d => d.Id == cmn.userId).Select(d => d.MI_Id).ToArray().ToArray();

                    var MIdata = _db.Institution.Where(d => MIId.Contains(d.MI_Id)).ToArray().ToArray();

                    lgData.MIdata = MIdata.ToArray();


                    if (rolelist[0].IVRMRT_Role.Equals("Super Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        var pageList = (from page in _db.masterpage
                                        from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                        from institutionModule in _db.Institution_Module
                                        from instModulePage in _db.Institution_Module_Page
                                        from institutionroleprevlg in _db.InstitutionRolePrivileges
                                        from institutewisemastermenu in _db.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == cmn.IVRM_MI_Id && institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == cmn.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true && institutewisemastermenu.MI_Id == cmn.IVRM_MI_Id
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,
                                            moduleId = institutionModule.IVRMM_Id,
                                            menuId = institutionwisemenupagemapp.IVRMMMI_Id,
                                            IVRMP_PageURL = page.IVRMP_PageURL,
                                            IVRMIMP_Id = instModulePage.IVRMIMP_Id,
                                            IVRMIMP_PageOrder = instModulePage.IVRMIMP_PageOrder
                                        }
                                        ).Distinct().OrderBy(t => t.IVRMIMP_PageOrder).ToList();
                        lgData.pageList = pageList.ToArray();

                        var privileges = (from institutionRolePrivileges in _db.InstitutionRolePrivileges
                                          from institutionModulePage in _db.Institution_Module_Page
                                          from intmodule in _db.Institution_Module
                                          where (institutionRolePrivileges.IVRMIMP_Id == institutionModulePage.IVRMIMP_Id && institutionModulePage.IVRMIM_Id == intmodule.IVRMIM_Id && intmodule.MI_Id == cmn.IVRM_MI_Id && institutionRolePrivileges.IVRMRT_Id == cmn.roleId)
                                          select new LoginDTO
                                          {
                                              pageId = institutionModulePage.IVRMP_Id,
                                              IVRMIRP_AddFlag = institutionRolePrivileges.IVRMIRP_AddFlag,
                                              IVRMIRP_DeleteFlag = institutionRolePrivileges.IVRMIRP_DeleteFlag,
                                              IVRMIRP_UpdateFlag = institutionRolePrivileges.IVRMIRP_UpdateFlag,
                                              IVRMIRP_SearchFlag = institutionRolePrivileges.IVRMIRP_SearchFlag,
                                              IVRMIRP_ReportFlag = institutionRolePrivileges.IVRMIRP_ReportFlag,
                                              IVRMIRP_ProcessFlag = institutionRolePrivileges.IVRMIRP_ProcessFlag
                                          }).ToList();
                        lgData.privileges = privileges.ToArray();
                    }

                    else if (rolelist[0].IVRMRT_Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {

                        var pageList = (from page in _db.masterpage
                                        from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                        from institutionModule in _db.Institution_Module
                                        from instModulePage in _db.Institution_Module_Page
                                        from institutionroleprevlg in _db.InstitutionRolePrivileges
                                        from institutewisemastermenu in _db.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == cmn.IVRM_MI_Id &&
                                        institutionModule.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == cmn.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,
                                            moduleId = institutionModule.IVRMM_Id,
                                            menuId = institutionwisemenupagemapp.IVRMMMI_Id,
                                            IVRMP_PageURL = page.IVRMP_PageURL,
                                            IVRMIMP_Id = instModulePage.IVRMIMP_Id
                                        }
                                   ).Distinct().ToList();
                        lgData.pageList = pageList.ToArray();


                        var privileges = (from institutionRolePrivileges in _db.InstitutionRolePrivileges
                                          from institutionModulePage in _db.Institution_Module_Page
                                          from intmodule in _db.Institution_Module
                                          where (institutionRolePrivileges.IVRMIMP_Id == institutionModulePage.IVRMIMP_Id && institutionModulePage.IVRMIM_Id == intmodule.IVRMIM_Id && intmodule.MI_Id == cmn.IVRM_MI_Id && institutionRolePrivileges.IVRMRT_Id == cmn.roleId)
                                          select new LoginDTO
                                          {
                                              pageId = institutionModulePage.IVRMP_Id,
                                              IVRMIRP_AddFlag = institutionRolePrivileges.IVRMIRP_AddFlag,
                                              IVRMIRP_DeleteFlag = institutionRolePrivileges.IVRMIRP_DeleteFlag,
                                              IVRMIRP_UpdateFlag = institutionRolePrivileges.IVRMIRP_UpdateFlag,
                                              IVRMIRP_SearchFlag = institutionRolePrivileges.IVRMIRP_SearchFlag,
                                              IVRMIRP_ReportFlag = institutionRolePrivileges.IVRMIRP_ReportFlag,
                                              IVRMIRP_ProcessFlag = institutionRolePrivileges.IVRMIRP_ProcessFlag
                                          }).ToList();
                        lgData.privileges = privileges.ToArray();


                        if (lgData.schoolcollegeflag == "S")
                        {

                            var studentid = (from stud in _db.StudentAppUserLoginDMO where stud.STD_APP_ID == cmn.userId select stud.AMST_ID);
                            lgData.AMST_Id = studentid.First();

                            List<Adm_M_Student> t = new List<Adm_M_Student>();

                            t = _db.Adm_M_Student.Where(g => g.MI_Id == cmn.IVRM_MI_Id && g.AMST_Id == lgData.AMST_Id).ToList();

                            t = (from a in _db.Adm_M_Student
                                 where (a.MI_Id == cmn.IVRM_MI_Id && a.AMST_Id == lgData.AMST_Id)
                                 select new Adm_M_Student
                                 {
                                     AMST_FirstName = a.AMST_FirstName,
                                     AMST_MiddleName = a.AMST_MiddleName,
                                     AMST_LastName = a.AMST_LastName,
                                     AMST_Photoname = a.AMST_Photoname
                                 }
        ).ToList();
                            if (t.Count() > 0)
                            {
                                lgData.studname = ((t.FirstOrDefault().AMST_FirstName == null ? "" : t.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (t.FirstOrDefault().AMST_MiddleName == null ? "" : t.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (t.FirstOrDefault().AMST_LastName == null ? "" : t.FirstOrDefault().AMST_LastName.ToUpper())).Trim().ToString();

                                lgData.studnamehalf = ((t.FirstOrDefault().AMST_FirstName == null ? "" : t.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (t.FirstOrDefault().AMST_MiddleName == null ? "" : t.FirstOrDefault().AMST_MiddleName.ToUpper())).Trim().ToString();

                                lgData.UserImagePath = t.FirstOrDefault().AMST_Photoname;
                            }

                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "GET_LOGIN_STUDENT_SIBLINGS";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                            SqlDbType.BigInt)
                                {
                                    Value = lgData.AMST_Id
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = await cmd.ExecuteReaderAsync())
                                    {
                                        while (await dataReader.ReadAsync())
                                        {
                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                            {
                                                dataRow.Add(
                                                    dataReader.GetName(iFiled),
                                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                );
                                            }

                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    lgData.siblinglist = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }


                            var checkactive = (from a in _db.School_Adm_Y_StudentDMO
                                               from b in _db.AcademicYear
                                               where a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == lgData.AMST_Id
                                               select new LoginDTO
                                               {
                                                   AMST_Id = a.AMST_Id,
                                                   AMAY_ActiveFlag = a.AMAY_ActiveFlag,
                                                   ASMAY_Order = b.ASMAY_Order
                                               }).Distinct().OrderByDescending(r => r.ASMAY_Order).Take(1).ToList();

                            if (checkactive.Count > 0)
                            {
                                if (checkactive.FirstOrDefault().AMAY_ActiveFlag == 0)
                                {
                                    lgData.chleft = "S";
                                }
                                else
                                {
                                    lgData.chleft = "";
                                }
                            }
                        }
                        else if (lgData.schoolcollegeflag == "C" || lgData.schoolcollegeflag == "U")
                        {

                            var studentid = (from stud in _db.CollegeStudentlogin where stud.IVRMUL_Id == cmn.userId select stud.AMCST_Id);
                            lgData.AMST_Id = studentid.First();

                            List<Adm_Master_College_StudentDMO> t = new List<Adm_Master_College_StudentDMO>();

                            t = _db.Adm_Master_College_StudentDMO.Where(g => g.MI_Id == cmn.IVRM_MI_Id && g.AMCST_Id == lgData.AMST_Id).ToList();

                            t = (from a in _db.Adm_Master_College_StudentDMO
                                 where (a.MI_Id == cmn.IVRM_MI_Id && a.AMCST_Id == lgData.AMST_Id)
                                 select new Adm_Master_College_StudentDMO
                                 {
                                     AMCST_FirstName = a.AMCST_FirstName,
                                     AMCST_MiddleName = a.AMCST_MiddleName,
                                     AMCST_LastName = a.AMCST_LastName,
                                     AMCST_StudentPhoto = a.AMCST_StudentPhoto
                                 }
        ).ToList();
                            if (t.Count() > 0)
                            {
                                lgData.studname = ((t.FirstOrDefault().AMCST_FirstName == null ? "" : t.FirstOrDefault().AMCST_FirstName.ToUpper()) + " " + (t.FirstOrDefault().AMCST_MiddleName == null ? "" : t.FirstOrDefault().AMCST_MiddleName.ToUpper()) + " " + (t.FirstOrDefault().AMCST_LastName == null ? "" : t.FirstOrDefault().AMCST_LastName.ToUpper())).Trim().ToString();

                                lgData.studnamehalf = ((t.FirstOrDefault().AMCST_FirstName == null ? "" : t.FirstOrDefault().AMCST_FirstName.ToUpper()) + " " + (t.FirstOrDefault().AMCST_MiddleName == null ? "" : t.FirstOrDefault().AMCST_MiddleName.ToUpper())).Trim().ToString();

                                lgData.UserImagePath = t.FirstOrDefault().AMCST_StudentPhoto;
                            }
                        }
                    }

                    else if (rolelist[0].IVRMRT_Role.Equals("PARENTS", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("PARENT", StringComparison.OrdinalIgnoreCase))
                    {

                        var pageList = (from page in _db.masterpage
                                        from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                        from institutionModule in _db.Institution_Module
                                        from instModulePage in _db.Institution_Module_Page
                                        from institutionroleprevlg in _db.InstitutionRolePrivileges
                                        from institutewisemastermenu in _db.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == cmn.IVRM_MI_Id &&
                                        institutionModule.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == cmn.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,
                                            moduleId = institutionModule.IVRMM_Id,
                                            menuId = institutionwisemenupagemapp.IVRMMMI_Id,
                                            IVRMP_PageURL = page.IVRMP_PageURL,
                                            IVRMIMP_Id = instModulePage.IVRMIMP_Id
                                        }
                                   ).Distinct().ToList();
                        lgData.pageList = pageList.ToArray();


                        var privileges = (from institutionRolePrivileges in _db.InstitutionRolePrivileges
                                          from institutionModulePage in _db.Institution_Module_Page
                                          from intmodule in _db.Institution_Module
                                          where (institutionRolePrivileges.IVRMIMP_Id == institutionModulePage.IVRMIMP_Id && institutionModulePage.IVRMIM_Id == intmodule.IVRMIM_Id && intmodule.MI_Id == cmn.IVRM_MI_Id && institutionRolePrivileges.IVRMRT_Id == cmn.roleId)
                                          select new LoginDTO
                                          {
                                              pageId = institutionModulePage.IVRMP_Id,
                                              IVRMIRP_AddFlag = institutionRolePrivileges.IVRMIRP_AddFlag,
                                              IVRMIRP_DeleteFlag = institutionRolePrivileges.IVRMIRP_DeleteFlag,
                                              IVRMIRP_UpdateFlag = institutionRolePrivileges.IVRMIRP_UpdateFlag,
                                              IVRMIRP_SearchFlag = institutionRolePrivileges.IVRMIRP_SearchFlag,
                                              IVRMIRP_ReportFlag = institutionRolePrivileges.IVRMIRP_ReportFlag,
                                              IVRMIRP_ProcessFlag = institutionRolePrivileges.IVRMIRP_ProcessFlag
                                          }).ToList();
                        lgData.privileges = privileges.ToArray();
                    }

                    else if (rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("ALumni", StringComparison.OrdinalIgnoreCase))
                    {
                        var pageList = (from page in _db.masterpage
                                        from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                        from institutionModule in _db.Institution_Module
                                        from instModulePage in _db.Institution_Module_Page
                                        from institutionroleprevlg in _db.InstitutionRolePrivileges
                                        from institutewisemastermenu in _db.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == cmn.IVRM_MI_Id &&
                                        institutionModule.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == cmn.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,
                                            moduleId = institutionModule.IVRMM_Id,
                                            menuId = institutionwisemenupagemapp.IVRMMMI_Id,
                                            IVRMP_PageURL = page.IVRMP_PageURL,
                                            IVRMIMP_Id = instModulePage.IVRMIMP_Id,
                                            IVRMIMP_PageOrder = instModulePage.IVRMIMP_PageOrder
                                        }
                                    ).Distinct().OrderBy(t => t.IVRMIMP_PageOrder).ToList();
                        lgData.pageList = pageList.ToArray();

                        var privileges = (from institutionRolePrivileges in _db.InstitutionRolePrivileges
                                          from institutionModulePage in _db.Institution_Module_Page
                                          from intmodule in _db.Institution_Module
                                          where (institutionRolePrivileges.IVRMIMP_Id == institutionModulePage.IVRMIMP_Id && institutionModulePage.IVRMIM_Id == intmodule.IVRMIM_Id && intmodule.MI_Id == cmn.IVRM_MI_Id && institutionRolePrivileges.IVRMRT_Id == cmn.roleId)
                                          select new LoginDTO
                                          {
                                              pageId = institutionModulePage.IVRMP_Id,
                                              IVRMIRP_AddFlag = institutionRolePrivileges.IVRMIRP_AddFlag,
                                              IVRMIRP_DeleteFlag = institutionRolePrivileges.IVRMIRP_DeleteFlag,
                                              IVRMIRP_UpdateFlag = institutionRolePrivileges.IVRMIRP_UpdateFlag,
                                              IVRMIRP_SearchFlag = institutionRolePrivileges.IVRMIRP_SearchFlag,
                                              IVRMIRP_ReportFlag = institutionRolePrivileges.IVRMIRP_ReportFlag,
                                              IVRMIRP_ProcessFlag = institutionRolePrivileges.IVRMIRP_ProcessFlag
                                          }).ToList();
                        lgData.privileges = privileges.ToArray();
                    }

                    else
                    {
                        var pageList = (from page in _db.masterpage
                                        from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                        from institutionModule in _db.Institution_Module
                                        from instModulePage in _db.Institution_Module_Page
                                        from institutionroleprevlg in _db.InstitutionRolePrivileges
                                        from institutewisemastermenu in _db.Mastermenuinstitutewise
                                        from staffloginpreviledge in _db.UserLoginPrivileges
                                        where (institutionModule.MI_Id == cmn.IVRM_MI_Id && staffloginpreviledge.IVRMIMP_Id == instModulePage.IVRMIMP_Id &&
                                        staffloginpreviledge.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == cmn.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true && staffloginpreviledge.Id == cmn.userId)
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,
                                            moduleId = institutionModule.IVRMM_Id,
                                            menuId = institutionwisemenupagemapp.IVRMMMI_Id,
                                            IVRMP_PageURL = page.IVRMP_PageURL,
                                            IVRMIMP_Id = instModulePage.IVRMIMP_Id,
                                            IVRMIMP_PageOrder = instModulePage.IVRMIMP_PageOrder
                                        }
                                ).Distinct().OrderBy(t => t.IVRMIMP_PageOrder).ToList();
                        lgData.pageList = pageList.ToArray();

                        var privileges = (from institutionRolePrivileges in _db.UserLoginPrivileges
                                          from institutionModulePage in _db.Institution_Module_Page
                                          from intmodule in _db.Institution_Module
                                          where (institutionRolePrivileges.IVRMIMP_Id == institutionModulePage.IVRMIMP_Id && institutionModulePage.IVRMIM_Id == intmodule.IVRMIM_Id && intmodule.MI_Id == cmn.IVRM_MI_Id && institutionRolePrivileges.Id == cmn.userId)
                                          select new LoginDTO
                                          {
                                              pageId = institutionModulePage.IVRMP_Id,
                                              IVRMIRP_AddFlag = institutionRolePrivileges.IVRMSTUUP_AddFlag,
                                              IVRMIRP_DeleteFlag = institutionRolePrivileges.IVRMSTUUP_DeleteFlag,
                                              IVRMIRP_UpdateFlag = institutionRolePrivileges.IVRMSTUUP_UpdateFlag,
                                              IVRMIRP_SearchFlag = institutionRolePrivileges.IVRMSTUUP_SearchFlag,
                                              IVRMIRP_ReportFlag = institutionRolePrivileges.IVRMSTUUP_ReportFlag,
                                              IVRMIRP_ProcessFlag = institutionRolePrivileges.IVRMSTUUP_ProcessFlag
                                          }).ToList();
                        lgData.privileges = privileges.ToArray();

                    }

                    List<long> pages = new List<long>();

                    foreach (Institution_Module_PageDTO a in lgData.pageList)
                    {
                        pages.Add(a.IVRMIMP_Id);
                    }


                    var moduleList1 = (from menumaster in _db.Mastermenuinstitutewise
                                       from institutionmodulepage in _db.Institution_Module_Page
                                       from institutionmodule in _db.Institution_Module
                                       from institutionroleprevlg in _db.InstitutionRolePrivileges
                                       from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                       where (
                                       menumaster.MI_Id == institutionmodule.MI_Id &&
                                               institutionmodulepage.IVRMIM_Id == institutionmodule.IVRMIM_Id &&
                                               institutionmodule.IVRMM_Id == menumaster.IVRMM_Id &&
                                               institutionmodulepage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                            menumaster.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                             institutionwisemenupagemapp.IVRMP_Id == institutionmodulepage.IVRMP_Id && pages.Contains(institutionmodulepage.IVRMIMP_Id) && menumaster.MI_Id == cmn.IVRM_MI_Id && institutionroleprevlg.IVRMRT_Id == cmn.roleId
                                             )
                                       select new Institution_Module_PageDTO
                                       {
                                           menuId = menumaster.IVRMMMI_Id,
                                           IVRMM_ModuleName = menumaster.IVRMMMI_MenuName,
                                           moduleId = menumaster.IVRMM_Id,
                                           parentId = menumaster.IVRMMMI_ParentId,
                                           pagenonpageflag = menumaster.IVRMMMI_PageNonPageFlag,
                                           order = menumaster.IVRMMMI_MenuOrder
                                       }
                                    ).OrderBy(d => d.order).Distinct().ToList();



                    var Menuids = moduleList1.Select(t => t.menuId).ToList();

                    var Parentsids = moduleList1.Select(t => t.parentId).ToArray();

                    for (int i = 0; i < Parentsids.Length; i++)
                    {
                        Menuids.Add(Parentsids[i]);
                    }

                    var moduleList = (from a in _db.Mastermenuinstitutewise.Where(t => Menuids.Contains(t.IVRMMMI_Id))
                                      select new Institution_Module_PageDTO
                                      {
                                          menuId = a.IVRMMMI_Id,
                                          IVRMM_ModuleName = a.IVRMMMI_MenuName,
                                          moduleId = a.IVRMM_Id,
                                          parentId = a.IVRMMMI_ParentId,
                                          pagenonpageflag = a.IVRMMMI_PageNonPageFlag,
                                          order = a.IVRMMMI_MenuOrder,
                                          IVRMMMI_Color = a.IVRMMMI_Color,
                                          IVRMMMI_Icon = a.IVRMMMI_Icon
                                      }).OrderBy(d => d.order).Distinct().ToList();


                    var moduleListonly = (from a in _db.Mastermenuinstitutewise.Where(t => Menuids.Contains(t.IVRMMMI_Id) && t.IVRMMMI_ParentId == 0 && t.MI_Id == cmn.IVRM_MI_Id)
                                          select new Institution_Module_PageDTO
                                          {
                                              menuId = a.IVRMMMI_Id,
                                              IVRMM_ModuleName = a.IVRMMMI_MenuName,
                                              moduleId = a.IVRMM_Id,
                                              parentId = a.IVRMMMI_ParentId,
                                              pagenonpageflag = a.IVRMMMI_PageNonPageFlag,
                                              order = a.IVRMMMI_MenuOrder,
                                              IVRMMMI_Color = a.IVRMMMI_Color,
                                              IVRMMMI_Icon = a.IVRMMMI_Icon
                                          }).OrderBy(d => d.order).Distinct().ToList();

                    lgData.moduleListonly = moduleListonly.ToArray();

                    lgData.moduleList = moduleList.ToArray();


                    var phtoemp = (from appuse in _db.Staff_User_Login
                                   where (appuse.IVRMSTAUL_UserName == cmn.username.Trim() && appuse.MI_Id == cmn.IVRM_MI_Id)
                                   select new Institution_Module_PageDTO
                                   {
                                       empid = appuse.Emp_Code
                                   }).FirstOrDefault();

                    if (phtoemp != null)
                    {
                        List<MasterEmployee> p = new List<MasterEmployee>();
                        p = (from a in _db.HR_Master_Employee_DMO
                             where (a.MI_Id == cmn.IVRM_MI_Id && a.HRME_Id == phtoemp.empid)
                             select new MasterEmployee
                             {
                                 HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                 HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                 HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                 HRME_Photo = a.HRME_Photo
                             }).ToList();

                        if (p.Count() > 0)
                        {
                            lgData.studname = ((p.FirstOrDefault().HRME_EmployeeFirstName == null ? "" : p.FirstOrDefault().HRME_EmployeeFirstName.ToUpper()) + " " + (p.FirstOrDefault().HRME_EmployeeMiddleName == null ? "" : p.FirstOrDefault().HRME_EmployeeMiddleName.ToUpper()) + " " + (p.FirstOrDefault().HRME_EmployeeLastName == null ? "" : p.FirstOrDefault().HRME_EmployeeLastName.ToUpper())).Trim().ToString();

                            lgData.studnamehalf = ((p.FirstOrDefault().HRME_EmployeeFirstName == null ? "" : p.FirstOrDefault().HRME_EmployeeFirstName.ToUpper()) + " " + (p.FirstOrDefault().HRME_EmployeeMiddleName == null ? "" : p.FirstOrDefault().HRME_EmployeeMiddleName.ToUpper())).Trim().ToString();

                            lgData.UserImagePath = p.FirstOrDefault().HRME_Photo;
                        }
                    }


                    var manadatoryfields = _db.IVRM_Mandatory_Setting_IW.Where(t => t.MI_Id == cmn.IVRM_MI_Id).ToList();
                    lgData.Manadatoryfields = manadatoryfields.ToArray();

                    //to fetch configuration data

                    var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == cmn.IVRM_MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                    if (Acdemic_preadmission == 0)
                    {
                        Acdemic_preadmission = cmn.ASMAY_Id;
                    }

                    lgData.currentyear = _db.AcademicYear.Where(t => t.MI_Id.Equals(cmn.IVRM_MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                    List<MasterConfiguration> configurationsettings = new List<MasterConfiguration>();
                    configurationsettings = _db.mstConfig.Where(t => t.ASMAY_Id == Acdemic_preadmission && t.MI_Id == cmn.IVRM_MI_Id).ToList();
                    lgData.configlist = configurationsettings.ToArray();

                    List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
                    admissionconfigurationsettings = _db.AdmissionStandardDMO.Where(t => t.MI_Id == cmn.IVRM_MI_Id && t.MI_Id == cmn.IVRM_MI_Id).ToList();
                    lgData.admissioncongigurationList = admissionconfigurationsettings.ToArray();

                    List<Master_Numbering> masnum = new List<Master_Numbering>();
                    masnum = _db.Master_Numbering.Where(t => t.MI_Id == cmn.IVRM_MI_Id).ToList();
                    lgData.transnumconfig = masnum.ToArray();

                    List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                    feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == cmn.IVRM_MI_Id).ToList();
                    lgData.feeconfiglist = feemasnum.ToArray();

                    List<IVRM_Storage_path_Details> IVRM_Storage_path_Details = new List<IVRM_Storage_path_Details>();
                    IVRM_Storage_path_Details = _db.IVRM_Storage_path_Details.ToList();
                    lgData.storagedetails = IVRM_Storage_path_Details.ToArray();


                    List<GeneralConfigDMO> ivrmgencon = new List<GeneralConfigDMO>();
                    ivrmgencon = _db.GenConfig.Where(t => t.MI_Id == cmn.IVRM_MI_Id).ToList();
                    lgData.ivrmconfiglist = ivrmgencon.ToArray();

                    List<MasterAcademic> year = new List<MasterAcademic>();
                    year = _db.AcademicYear.Where(t => t.MI_Id == cmn.IVRM_MI_Id && t.ASMAY_Id == cmn.ASMAY_Id).ToList();
                    lgData.fillyear = year.ToArray();

                    //added on 30/10/2017
                    List<Institution> institutionanme = new List<Institution>();
                    institutionanme = _db.Institute.Where(t => t.MI_Id == cmn.IVRM_MI_Id).ToList();
                    lgData.fillinstition = institutionanme.ToArray();

                    List<Dashboard_page_mapping> fillnme = new List<Dashboard_page_mapping>();
                    fillnme = _db.Dashboard_page_mapping.Where(t => t.IVRMRT_Role.Equals(rolelist[0].IVRMRT_Role)).ToList();
                    lgData.filldashpagemap = fillnme.ToArray();

                    if (lgData.filldashpagemap.Length > 0)
                    {
                        List<MasterPage> pgeexists = new List<MasterPage>();
                        pgeexists = _db.masterPage.Where(t => t.IVRMP_PageURL.Equals(fillnme[0].IVRMP_Dasboard_PageName)).ToList();
                        lgData.pageexists = pgeexists.ToArray();

                        #region sms credit alert
                        if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                        {
                            if (lgData.smscreditalert == 0)
                            {
                                SMSCreditAlert _credit = new SMSCreditAlert(_db);
                                CommonDTO creditcnt = _credit.getalertcount(cmn);
                                lgData.smsalrtflag = creditcnt.smsalrtflag;
                                lgData.Rcredit = creditcnt.Rcredit;
                            }
                        }
                        #endregion
                    }
                    //to fetch configuration data
                    var UserImagePath = "";
                    ApplicationUser user = new ApplicationUser();
                    user = await _userManager.FindByIdAsync(Convert.ToString(cmn.userId));
                    if (user.UserImagePath != "" && user.UserImagePath != null && user.UserImagePath != "undefined")
                    {
                        string accountname = "";
                        string accesskey = "";
                        var datastrg = _db.IVRM_Storage_path_Details.ToList();
                        if (datastrg.Count() > 0)
                        {
                            accountname = datastrg.FirstOrDefault().IVRM_SD_Access_Name;
                            accesskey = datastrg.FirstOrDefault().IVRM_SD_Access_Key;
                        }

                        string containername = "files", mid = cmn.IVRM_MI_Id.ToString(), folder = "StudentProfilePics", blobName = "";

                        string fullpath = user.UserImagePath;
                        string[] fullpathArr = fullpath.Split('\\');
                        string[] fullfileArr = fullpathArr.Last().Split('/');
                        int flenmemaxlen = fullfileArr.Length - 1;

                        string flenme = fullfileArr[flenmemaxlen].ToString();

                        blobName = flenme;

                        StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                        CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);

                        CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(containername);

                        CloudBlockBlob blob123 = container.GetBlockBlobReference(blobName);
                        var sta = true;
                        var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                        if (rolelist[0].IVRMRT_Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                        {
                            sta = await blockBlob.ExistsAsync();
                        }
                        else
                        {
                            string photopath = UserImagePath = user.UserImagePath.Substring(0, 3).ToString();

                            if (user.UserImagePath.Substring(0, 4).ToString() == "http")
                            {
                                UserImagePath = user.UserImagePath;
                            }
                            else
                            {
                                UserImagePath = user.UserImagePath.Substring(user.UserImagePath.IndexOf("images"));
                            }
                        }

                        lgData.UserImagePath = "";
                        if (sta == false)
                        {
                            UserImagePath = "https://bdcampusstrg.blob.core.windows.net/files/" + cmn.IVRM_MI_Id + "/StudentProfilePics/little-girl-clipart-student-16.jpg";
                        }
                    }
                    else
                    {
                        UserImagePath = "https://bdcampusstrg.blob.core.windows.net/files/2/EmployeeProfilePics/a_1c2e412b-a150-4cbb-8bfd-d9354ed84946.jpg";
                    }

                    var UserList = (from appuserRole in _db.appUserRole
                                    from appRole in _db.applicationRole
                                    where (appRole.Id == appuserRole.RoleId && appuserRole.RoleTypeId == cmn.roleId && appuserRole.UserId == cmn.userId)
                                    select new LoginDTO
                                    {
                                        RoleName = appRole.Name,
                                        UserName = user.NormalizedUserName,
                                        UserImagePath = UserImagePath,
                                        Useremail = user.Email,
                                        Userappname = user.Name,
                                        usermob = user.PhoneNumber

                                    }).OrderBy(d => d.RoleName).Distinct().ToList();

                    lgData.userData = UserList.ToArray();
                    if (cmn.amst_id > 0)
                    {
                        string studentfilenme = "";
                        if (lgData.schoolcollegeflag == "S")
                        {
                            var studlist = (
                                  from b in _db.Adm_M_Student
                                  where (b.AMST_Id == cmn.amst_id)
                                  select new LoginDTO
                                  {
                                      imgnme = b.AMST_Photoname
                                  }).ToArray();

                            lgData.studentdata = studlist.ToArray();
                            for (int r = 0; r < studlist.Count(); r++)
                            {
                                if (studlist[r].imgnme != null)
                                {
                                    studentfilenme = studlist[r].imgnme;
                                }
                                else
                                {
                                    studentfilenme = "";
                                }
                            }
                        }
                        else if (lgData.schoolcollegeflag == "C" || lgData.schoolcollegeflag == "U")
                        {
                            var studlist = (
                                  from b in _db.Adm_Master_College_StudentDMO
                                  where (b.AMCST_Id == cmn.amst_id)
                                  select new LoginDTO
                                  {
                                      imgnme = b.AMCST_StudentPhoto
                                  }).ToArray();

                            lgData.studentdata = studlist.ToArray();
                            for (int r = 0; r < studlist.Count(); r++)
                            {
                                if (studlist[r].imgnme != null)
                                {
                                    studentfilenme = studlist[r].imgnme;
                                }
                                else
                                {
                                    studentfilenme = "";
                                }
                            }
                        }

                        if (studentfilenme != "")
                        {
                            string accountname = "";
                            string accesskey = "";
                            var datastrg = _db.IVRM_Storage_path_Details.ToList();
                            if (datastrg.Count() > 0)
                            {
                                accountname = datastrg.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = datastrg.FirstOrDefault().IVRM_SD_Access_Key;
                            }

                            //string accountname = "bdcampusstrg";
                            //string accesskey = "2GbpRyxTMjVYZc0wnKLbpgCAPYRrdX3HPUE6kcYLmk19vkq8ErTC1eYIMl1oFMhzihqlq3j0eqWmiOGt1sfZ5w==";
                            string containername = "files", mid = cmn.IVRM_MI_Id.ToString(), folder = "StudentProfilePics", blobName = "";

                            if (studentfilenme != null || studentfilenme != "")
                            {
                                string fullpath = studentfilenme;
                                string[] fullpathArr = fullpath.Split('\\');
                                string[] fullfileArr = fullpathArr.Last().Split('/');
                                int flenmemaxlen = fullfileArr.Length - 1;

                                string flenme = fullfileArr[flenmemaxlen].ToString();

                                blobName = flenme;

                                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);

                                CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                                CloudBlobContainer container = blobClient.GetContainerReference(containername);

                                CloudBlockBlob blob123 = container.GetBlockBlobReference(blobName);

                                var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);

                                var sta = await blockBlob.ExistsAsync();
                                lgData.UserImagePath = "";

                                if (sta == false)
                                {
                                    lgData.UserImagePath = "https://bdcampusstrg.blob.core.windows.net/files/4/StudentProfilePics/little-girl-clipart-student-16.jpg";
                                }
                            }

                        }
                        else
                        {
                            lgData.UserImagePath = "https://bdcampusstrg.blob.core.windows.net/files/2/EmployeeProfilePics/a_1c2e412b-a150-4cbb-8bfd-d9354ed84946.jpg";
                        }
                    }

                    if (cmn.PaymentNootificationGeneral == 0 && !rolelist[0].IVRMRT_Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var check_paymenttemplate = (from a in _db.SMSEmailSettingUserMapping
                                                     from b in _db.smsEmailSetting
                                                     where (a.ISES_Id == b.ISES_Id && a.ISESUSR_ActiveFlg == true && a.UserId == cmn.userId
                                                     && b.MI_Id == cmn.IVRM_MI_Id && b.ISES_Template_Name == "Payment_Remainder")
                                                     select b).Distinct().ToList();

                        if (check_paymenttemplate.Count > 0)
                        {
                            lgData.payment_content = check_paymenttemplate.FirstOrDefault().ISES_SMSMessage;

                            #region Payment Notfication

                            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                            var checkdata = _PortalContext.IVRM_Payment_Subscription_RemarksDetilsDMO.Where(a => a.UserId == cmn.userId
                            && a.IVRMPSLR_LoginDatetime.Value.Date == indiantime0.Date && a.MI_Id == cmn.IVRM_MI_Id
                            && a.IVRMPSLR_RemainderTemplateName == "Payment_Remainder").ToList();

                            if (checkdata.Count == 0)
                            {
                                SubscriptionPaymentNotification _sub = new SubscriptionPaymentNotification(_PortalContext);
                                lgData.getpaymentnotificationdetails = _sub.getpaymentnotificationdetails(cmn.IVRM_MI_Id, cmn.userId);
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return lgData;
        }

        [Route("VerifyUserName")]
        public async Task<LoginDTO> VerifyUserName([FromBody] LoginDTO UserName)
        {
            LoginDTO lgData = new LoginDTO();

            ApplicationUser user = new ApplicationUser();
            user = await _userManager.FindByNameAsync(Convert.ToString(UserName.UserName));
            if (user != null)
            {
                lgData.UserNameVerifyStatus = "Success";
                lgData.mobileNo = user.PhoneNumber;
                lgData.Email = user.Email;
            }
            else
            {
                lgData.UserNameVerifyStatus = "Fail";
            }

            return lgData;
        }

        [Route("setVirtualSession/{id:int}")]
        public CommonDTO setSessionDataForVirtual(int id)
        {
            CommonDTO cmn = new CommonDTO();
            try
            {
                if (id != 0)
                {
                    var VSchoolData = _db.VirtualSchool.Single(d => d.IVRM_Virtual_School_Id == id);
                    cmn.IVRM_MI_Id = Convert.ToInt32(VSchoolData.IVRM_MI_Id);
                    cmn.IVRM_MO_Id = Convert.ToInt32(VSchoolData.IVRM_MO_Id);
                }
                else
                {
                    var VSchoolData = _db.VirtualSchool.FirstOrDefault();
                    cmn.IVRM_MI_Id = Convert.ToInt32(VSchoolData.IVRM_MI_Id);
                    cmn.IVRM_MO_Id = Convert.ToInt32(VSchoolData.IVRM_MO_Id);
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cmn;
        }

        [Route("setVirtualSessionsubdomain")]
        public CommonDTO setSessionDataForVirtual([FromBody] CommonDTO dto)
        {
            CommonDTO cmn = new CommonDTO();
            try
            {
                if (dto.subDomainName != "")
                {
                    var VSchoolData = _db.VirtualSchool.Where(d => d.IVRM_Sub_Domain_Name.Equals(dto.subDomainName, StringComparison.OrdinalIgnoreCase));
                    if (VSchoolData.Count() > 0)
                    {
                        cmn.IVRM_MI_Id = Convert.ToInt32(VSchoolData.FirstOrDefault().IVRM_MI_Id);
                        cmn.IVRM_MO_Id = Convert.ToInt32(VSchoolData.FirstOrDefault().IVRM_MO_Id);
                        cmn.virtualid = Convert.ToInt32(VSchoolData.FirstOrDefault().IVRM_Virtual_School_Id);
                    }
                    else
                    {
                        cmn.Message = "NoINT";
                        return cmn;
                    }

                }
                else
                {
                    var VSchoolData = _db.VirtualSchool.FirstOrDefault();
                    cmn.IVRM_MI_Id = Convert.ToInt32(VSchoolData.IVRM_MI_Id);
                    cmn.IVRM_MO_Id = Convert.ToInt32(VSchoolData.IVRM_MO_Id);
                    cmn.virtualid = Convert.ToInt32(VSchoolData.IVRM_Virtual_School_Id);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cmn;
        }

        [Route("forgotpassword")]
        public async Task<LoginDTO> forgotPassword([FromBody] LoginDTO email)
        {
            ApplicationUser user = new ApplicationUser();
            try
            {
                user = new ApplicationUser { UserName = email.UserName };
                var result = await _userManager.CreateAsync(user, email.newPassword);
                if (result.Errors.FirstOrDefault().Code == "DuplicateUserName")
                {
                    user = await _userManager.FindByNameAsync(email.UserName);
                    await _userManager.RemovePasswordAsync(user);
                    var resultpass = await _userManager.AddPasswordAsync(user, email.newPassword);
                    email.message = "Success";
                }
                else
                {
                    email.message = "Password Must Contain At Least One Uppercase,One Number and One Special Character.";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return email;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        [Route("setSessionDataForSubDomainName")]
        public CommonDTO setSessionDataForSubDomainName(CommonDTO cmn)
        {
            try
            {
                if (cmn.subDomainName != "" && cmn.subDomainName != null)
                {
                    var VSchoolData = _db.VirtualSchool.Single(d => d.IVRM_Sub_Domain_Name.Equals(cmn.subDomainName));
                    cmn.IVRM_MI_Id = Convert.ToInt32(VSchoolData.IVRM_MI_Id);
                    cmn.IVRM_MO_Id = Convert.ToInt32(VSchoolData.IVRM_MO_Id);
                }
                else
                {
                    var VSchoolData = _db.VirtualSchool.FirstOrDefault();
                    cmn.IVRM_MI_Id = Convert.ToInt32(VSchoolData.IVRM_MI_Id);
                    cmn.IVRM_MO_Id = Convert.ToInt32(VSchoolData.IVRM_MO_Id);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cmn;
        }

        [Route("getInstituteDataByRoleTypeName/{id:int}")]
        public async Task<LoginDTO> getInstituteDataByRoleTypeName(int id)
        {
            LoginDTO reg = new LoginDTO();
            reg.MI_ID = id;
            List<LoginDTO> result = new List<LoginDTO>();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "GET_ROL_TYP_ID";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@mi_Id",
                    SqlDbType.BigInt)
                {
                    Value = id
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                //var data = cmd.ExecuteNonQuery();
                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (dataReader.Read())
                        {
                            result.Add(new LoginDTO
                            {
                                userId = Convert.ToInt32(dataReader["userId"]),
                                roleId = Convert.ToInt64(dataReader["RoleTypeId"]),
                            });
                            reg.multiAdminSelectedDetails = result.ToArray();
                        }

                    }

                    //reg
                    reg.userId = result.FirstOrDefault().userId;
                    reg.roleId = result.FirstOrDefault().roleId;

                }
                catch (Exception ex)
                {

                }

                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                if (acd_Id == 0)
                {
                    reg.message = "Kindly contact Administrator";
                }
                else
                {
                    reg.ASMAY_Id = acd_Id;
                }

                reg.ASMAY_Id = acd_Id;

            }
            GetLayoutDetails(reg);
            return reg;
        }

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {

            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);


            stu.MI_Id = Convert.ToInt64(response.udf3);
            stu.PASR_MobileNo = response.phone;
            stu.pasR_Id = 0;
            stu.PASR_emailId = response.email;

            var confirmstatus = _db.Database.ExecuteSqlCommand("Preadmission_userregistration_online @p0,@p1,@p2,@p3,@p4", response.udf3, response.udf6, response.amount, response.txnid, response.mihpayid);

            if (confirmstatus > 0)
            {
                response.responseupdate = "true";
            }
            else
            {
                response.responseupdate = "false";
            }

            return response;
        }
        public LoginDTO GetLayoutDetails(LoginDTO lgData)
        {
            try
            {
                if (lgData.roleId != 0 && lgData.MI_ID != 0)
                {
                    //var moduleList = (from rolePlvg in _db.Institution_Module
                    //                  from mod_rol in _db.Institution_Module_Page
                    //                  from mod in _db.masterModule
                    //                      //where (rolePlvg.IVRMMP_Id == mod_rol.IVRMMP_Id && mod_rol.IVRMM_Id == mod.IVRMM_Id && rolePlvg.IVRMRT_Id == Convert.ToInt32(id))
                    //                  where (rolePlvg.IVRMIM_Id == mod_rol.IVRMIM_Id && rolePlvg.MI_Id == cmn.IVRM_MI_Id && mod_rol.Ref_IVRMRT_Id == cmn.roleId)
                    //                  select new Institution_Module_PageDTO { ivrmmP_Id = mod.IVRMM_Id, IVRMM_ModuleName = mod.IVRMM_ModuleName }
                    //                ).Distinct().ToList();

                    // Changed on 11-11-2016 as per new change request


                    var moduleList1 = (from menumaster in _db.Mastermenuinstitutewise
                                       from institutionmodulepage in _db.Institution_Module_Page
                                       from institutionmodule in _db.Institution_Module
                                       from institutionroleprevlg in _db.InstitutionRolePrivileges
                                       from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                       where (menumaster.MI_Id == lgData.MI_ID &&
                                               menumaster.MI_Id == institutionmodule.MI_Id &&
                                               institutionmodulepage.IVRMIM_Id == institutionmodule.IVRMIM_Id &&
                                               institutionmodule.IVRMM_Id == menumaster.IVRMM_Id &&
                                               institutionmodulepage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                               institutionroleprevlg.IVRMRT_Id == lgData.roleId &&
                                            menumaster.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                             institutionwisemenupagemapp.IVRMP_Id == institutionmodulepage.IVRMP_Id
                                             )
                                       select new Institution_Module_PageDTO
                                       {
                                           menuId = menumaster.IVRMMMI_Id,
                                           IVRMM_ModuleName = menumaster.IVRMMMI_MenuName,
                                           moduleId = menumaster.IVRMM_Id,
                                           parentId = menumaster.IVRMMMI_ParentId,
                                           pagenonpageflag = menumaster.IVRMMMI_PageNonPageFlag,
                                           order = menumaster.IVRMMMI_MenuOrder,
                                           IVRMMMI_Color = menumaster.IVRMMMI_Color,
                                           IVRMMMI_Icon = menumaster.IVRMMMI_Icon

                                       }
                                      ).OrderBy(d => d.order).Distinct().ToList();

                    var Menuids = moduleList1.Select(t => t.menuId).ToList();

                    var Parentsids = moduleList1.Select(t => t.parentId).ToArray();

                    for (int i = 0; i < Parentsids.Length; i++)
                    {
                        Menuids.Add(Parentsids[i]);
                    }


                    var moduleList = (from a in _db.Mastermenuinstitutewise.Where(t => Menuids.Contains(t.IVRMMMI_Id))
                                      select new Institution_Module_PageDTO
                                      {
                                          menuId = a.IVRMMMI_Id,
                                          IVRMM_ModuleName = a.IVRMMMI_MenuName,
                                          moduleId = a.IVRMM_Id,
                                          parentId = a.IVRMMMI_ParentId,
                                          pagenonpageflag = a.IVRMMMI_PageNonPageFlag,
                                          order = a.IVRMMMI_MenuOrder,
                                          IVRMMMI_Color = a.IVRMMMI_Color,
                                          IVRMMMI_Icon = a.IVRMMMI_Icon
                                      }).OrderBy(d => d.order).Distinct().ToList();


                    lgData.moduleList = moduleList.ToArray();

                    var pageList = (from page in _db.masterpage
                                    from institutionwisemenupagemapp in _db.Mastermenupagemappinginstitutewise
                                    from institutionModule in _db.Institution_Module
                                    from instModulePage in _db.Institution_Module_Page
                                    from institutionroleprevlg in _db.InstitutionRolePrivileges
                                    from institutewisemastermenu in _db.Mastermenuinstitutewise
                                    where (institutionModule.MI_Id == lgData.MI_ID &&
                                            institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                            instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                            institutionroleprevlg.IVRMRT_Id == lgData.roleId &&
                                            page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                            page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                           institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                           page.IVRMP_TemplateFlag == true
                                    )
                                    select new Institution_Module_PageDTO
                                    {
                                        pageId = page.IVRMP_Id,
                                        parentId = institutewisemastermenu.IVRMMMI_ParentId,
                                        IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,
                                        moduleId = institutionModule.IVRMM_Id,
                                        menuId = institutionwisemenupagemapp.IVRMMMI_Id,
                                        IVRMP_PageURL = page.IVRMP_PageURL,
                                        IVRMIMP_PageOrder = instModulePage.IVRMIMP_PageOrder
                                    }
                                        ).Distinct().OrderBy(t => t.IVRMIMP_PageOrder).ToList();
                    lgData.pageList = pageList.ToArray();

                    //Date:26-12-2016 for getting privileges.
                    var privileges = (from institutionRolePrivileges in _db.InstitutionRolePrivileges
                                      from institutionModulePage in _db.Institution_Module_Page
                                      where (institutionRolePrivileges.IVRMIMP_Id == institutionModulePage.IVRMIMP_Id && institutionRolePrivileges.IVRMRT_Id == lgData.roleId)
                                      select new LoginDTO
                                      {
                                          pageId = institutionModulePage.IVRMP_Id,
                                          IVRMIRP_AddFlag = institutionRolePrivileges.IVRMIRP_AddFlag,
                                          IVRMIRP_DeleteFlag = institutionRolePrivileges.IVRMIRP_DeleteFlag,
                                          IVRMIRP_UpdateFlag = institutionRolePrivileges.IVRMIRP_UpdateFlag,
                                          IVRMIRP_SearchFlag = institutionRolePrivileges.IVRMIRP_SearchFlag,
                                          IVRMIRP_ReportFlag = institutionRolePrivileges.IVRMIRP_ReportFlag,
                                          IVRMIRP_ProcessFlag = institutionRolePrivileges.IVRMIRP_ProcessFlag
                                      }).ToList();
                    lgData.privileges = privileges.ToArray();

                    //to fetch configuration data

                    var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == lgData.MI_ID).Select(d => d.ASMAY_Id).FirstOrDefault();

                    if (Acdemic_preadmission == 0)
                    {
                        Acdemic_preadmission = lgData.ASMAY_Id;
                    }

                    List<MasterConfiguration> configurationsettings = new List<MasterConfiguration>();
                    configurationsettings = _db.mstConfig.Where(t => t.ASMAY_Id == Acdemic_preadmission && t.MI_Id == lgData.MI_ID).ToList();
                    lgData.configlist = configurationsettings.ToArray();

                    List<Master_Numbering> masnum = new List<Master_Numbering>();
                    masnum = _db.Master_Numbering.Where(t => t.MI_Id == lgData.MI_ID).ToList();
                    lgData.transnumconfig = masnum.ToArray();

                    List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                    feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == lgData.MI_ID).ToList();
                    lgData.feeconfiglist = feemasnum.ToArray();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return lgData;
        }

        [Route("getrolewisepage/")]
        public LoginDTO getdashboarddetails([FromBody]LoginDTO lgData)
        {
            try
            {
                List<Dashboard_page_mapping> fillnme = new List<Dashboard_page_mapping>();
                fillnme = _db.Dashboard_page_mapping.Where(t => t.IVRMRT_Role.Equals(lgData.RoleName) && t.MI_ID == lgData.MI_ID).ToList();
                lgData.filldashpagemap = fillnme.ToArray();

                if (lgData.filldashpagemap.Length > 0)
                {
                    List<MasterPage> pgeexists = new List<MasterPage>();
                    pgeexists = _db.masterPage.Where(t => t.IVRMP_PageURL.Equals(fillnme[0].IVRMP_Dasboard_PageName)).ToList();
                    lgData.pageexists = pgeexists.ToArray();
                }
            }
            catch (Exception ex)
            {

            }
            return lgData;
        }

        [Route("KioskStudentSmartCardPunch")]
        public LoginDTO KioskStudentSmartCardPunch([FromBody] LoginDTO smartcardData)
        {
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(smartcardData.MI_ID) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var instituteName = _db.Institution.Where(d => d.MI_Id == smartcardData.MI_ID).Select(d => new InstitutionDTO { MI_Name = d.MI_Name, MI_Logo = d.MI_Logo }).ToList();

                // Student.
                if (smartcardData.AMST_Id > 0)
                {
                    var loginData = _Enquirycontext.StudentUserLoginDMO.Where(d => d.AMST_Id == smartcardData.AMST_Id).Select(d => new LoginDTO { AMST_Id = d.AMST_Id, UserName = d.IVRMSTUUL_UserName }).ToList();
                    var userId = _Enquirycontext.StudentAppUserLoginDMO.Where(d => d.AMST_ID == smartcardData.AMST_Id).Select(d => new LoginDTO { userId = d.STD_APP_ID }).ToList();
                    var userRoleId = _db.appUserRole.Where(d => d.UserId == userId.FirstOrDefault().userId).ToList();
                    var roleName = _db.MasterRoleType.Where(d => d.IVRMRT_Id == userRoleId.FirstOrDefault().RoleTypeId).ToList();

                    smartcardData.ASMAY_Id = acd_Id;
                    smartcardData.instituteName = instituteName.FirstOrDefault().MI_Name;
                    smartcardData.instituteLogo = instituteName.FirstOrDefault().MI_Logo;
                    smartcardData.AMST_Id = loginData.FirstOrDefault().AMST_Id;
                    smartcardData.UserName = loginData.FirstOrDefault().UserName;
                    smartcardData.userId = userId.FirstOrDefault().userId;
                    smartcardData.roleId = Convert.ToInt64(userRoleId.FirstOrDefault().RoleId);
                    smartcardData.RoleName = roleName.FirstOrDefault().IVRMRT_Role;

                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    _db.Database.ExecuteSqlCommand("logintype_history @p0,@p1,@p2,@p3", smartcardData.MI_ID, smartcardData.userId, indianTime, "KIOSK");
                }

                //Employee.
                else if (smartcardData.Emp_Id > 0)
                {
                    var loginData = _db.Staff_User_Login.Where(d => d.Emp_Code == smartcardData.Emp_Id).Select(d => new LoginDTO { Emp_Id = d.Emp_Code, UserName = d.IVRMSTAUL_UserName, userId = Convert.ToInt32(d.Id) }).ToList();
                    var userRoleId = _db.appUserRole.Where(d => d.UserId == loginData.FirstOrDefault().userId).ToList();
                    var roleName = _db.MasterRoleType.Where(d => d.IVRMRT_Id == userRoleId.FirstOrDefault().RoleTypeId).ToList();

                    smartcardData.ASMAY_Id = acd_Id;
                    smartcardData.instituteName = instituteName.FirstOrDefault().MI_Name;
                    smartcardData.instituteLogo = instituteName.FirstOrDefault().MI_Logo;
                    smartcardData.AMST_Id = loginData.FirstOrDefault().AMST_Id;
                    smartcardData.UserName = loginData.FirstOrDefault().UserName;
                    smartcardData.userId = loginData.FirstOrDefault().userId;
                    smartcardData.roleId = Convert.ToInt64(userRoleId.FirstOrDefault().RoleId);
                    smartcardData.RoleName = roleName.FirstOrDefault().IVRMRT_Role;

                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    _db.Database.ExecuteSqlCommand("logintype_history @p0,@p1,@p2,@p3", smartcardData.MI_ID, smartcardData.userId, indianTime, "KIOSK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return smartcardData;
        }

        [Route("getgateway")]
        public FileDescriptionDTO getgateway([FromBody] FileDescriptionDTO dto)
        {

            try
            {

                // var VSchoolData = _db.VirtualSchool.Single(d => d.IVRM_Virtual_School_Id == dto.vr_id);
                dto.fillpaymentgateway = (from a in _db.PAYUDETAILS
                                          from b in _db.Fee_PaymentGateway_Details
                                          where (a.IMPG_Id == b.IMPG_Id && a.IMPG_ActiveFlg == true && b.MI_Id == dto.MI_Id)
                                          select new FileDescriptionDTO
                                          {
                                              FPGD_Id = a.IMPG_Id,
                                              IMPG_PGFlag = a.IMPG_PGFlag,
                                              FPGD_Image = b.FPGD_Image,
                                              FPGD_PGName = b.FPGD_PGName,
                                              MI_Idnew = b.MI_Id,
                                          }).Distinct().ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniRegistrationFee";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.alumnifee = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                var alu = _db.GenConfig.Where(a => a.MI_Id == dto.MI_Id).ToArray();
                if (alu.Length > 0)
                {
                    dto.gencon = alu;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return dto;
        }

        [Route("GetpaymentDetails")]
        public FileDescriptionDTO GetpaymentDetails([FromBody] FileDescriptionDTO dto)
        {

            try
            {

                dto.paymentgateway = _db.Fee_PaymentGateway_Details.Where(a => a.MI_Id == dto.MI_Idnew && a.FPGD_PGName == dto.FPGD_PGName && a.FPGD_PGActiveFlag == "1").ToArray();
                GenerateOrderId(dto);
                dto.institution = _db.Institution.Where(a => a.MI_Id == dto.MI_Idnew).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniRegistrationFee";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = dto.MI_Idnew
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.alumnifee = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return dto;
        }
        public FileDescriptionDTO GenerateOrderId(FileDescriptionDTO dto)
        {
            try
            {


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _db.Fee_PaymentGateway_Details.Where(t => t.MI_Id == dto.MI_Idnew && t.FPGD_PGName == dto.FPGD_PGName && t.FPGD_PGActiveFlag == "1").Distinct().ToList();

                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("notes_1", dto.AlumniName);
                transfersnotes.Add("notes_2", dto.Email_id);
                transfersnotes.Add("notes_3", dto.Mobile.ToString());
                transfersnotes.Add("notes_4", dto.admission.ToString());
                //transfersnotes.Add("notes_5", dto.ALMST_PerCity);
                //transfersnotes.Add("notes_6", dto.ALMST_PerPincode);

                Dictionary<string, object> input = new Dictionary<string, object>();
                //input.Add("amount", 1 * 100);
                input.Add("amount", dto.Donation_Amount * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("payment_capture", 1);
                input.Add("notes", transfersnotes);

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                dto.orderId = order["id"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        //Azure Storage Iamges Optimization
        [Route("DownloadContainer")]
        public async Task<string> DownloadContainer()
        {
            var fileName = string.Empty;
            MemoryStream ms = new MemoryStream();
            var blobContainers = new List<CloudBlobContainer>();
            List<string> blobs = new List<string>();

            if (CloudStorageAccount.TryParse("DefaultEndpointsProtocol=https;AccountName=stjamesstorage;AccountKey=XC0swqWKUF0iAwNHD4MjDn5q9DkB6PuICQUbUG8NLBR95Kdg11m1UjHyaPycSW3n18wrESF4wPj84GfB5ZE3mg==;EndpointSuffix=core.windows.net", out CloudStorageAccount storageAccount))
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                BlobContinuationToken blobContinuationToken = null;
                do
                {
                    var containerSegment = await blobClient.ListContainersSegmentedAsync(blobContinuationToken);
                    blobContainers.AddRange(containerSegment.Results);
                    blobContinuationToken = containerSegment.ContinuationToken;
                }
                while (blobContinuationToken != null);
                if (blobContainers.Count() > 0)
                {
                    foreach (var blobb in blobContainers)
                    {
                        string bNameC = blobb.Name;
                        CloudBlobContainer container = blobClient.GetContainerReference(bNameC);
                        BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(null);
                        if (bNameC == "files")
                        {
                            try
                            {
                                foreach (IListBlobItem item in resultSegment.Results)
                                {
                                    try
                                    {
                                        if (item.GetType() == typeof(CloudBlockBlob))
                                        {
                                            CloudBlockBlob blob = (CloudBlockBlob)item;
                                            blobs.Add(blob.Name);
                                            CloudBlob file = blob;
                                            string[] splitString = blob.Name.Split('.');
                                            if (splitString.Length > 1)
                                            {
                                                string Exten = splitString[1].Trim();
                                                if (Exten.Equals("JPG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("PNG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    Stream stream = new MemoryStream();
                                                    string content;
                                                    var reader = new StreamReader(stream);
                                                    content = reader.ReadToEnd();
                                                    if (await file.ExistsAsync())
                                                    {
                                                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                                        fileName = blob.Name;
                                                        fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";

                                                        await file.DownloadToStreamAsync(stream);

                                                        if (stream.CanSeek)
                                                        {
                                                            stream.Seek(0, SeekOrigin.Begin);
                                                        }
                                                        else
                                                        {
                                                            stream.Dispose();
                                                            stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                                                        }
                                                        var optimized = "";
                                                        const int size = 900;
                                                        const int quality = 100;
                                                        using (var image = new MagickImage(stream))
                                                        {
                                                            image.Resize(size, size);
                                                            image.AutoOrient();
                                                            image.Strip();
                                                            image.Quality = quality;
                                                            optimized = image.ToBase64();
                                                        }

                                                        var Base64 = optimized;
                                                        string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                                                        var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                                                        convert2 = convert2.Replace("data:image/jpeg;base64,", String.Empty);
                                                        convert2 = convert2.Replace(" ", "+");
                                                        int mod4 = convert2.Length % 4;
                                                        if (mod4 > 0)
                                                        {
                                                            convert2 += new string('=', 4 - mod4);
                                                        }
                                                        var blockBlob = container.GetBlockBlobReference(blob.Name);
                                                        var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                                                        using (var streamnew = new MemoryStream(bytes))
                                                        {
                                                            await blockBlob.UploadFromStreamAsync(streamnew);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (item.GetType() == typeof(CloudPageBlob))
                                        {
                                            CloudPageBlob blob = (CloudPageBlob)item;
                                            blobs.Add(blob.Name);
                                        }
                                        else if (item.GetType() == typeof(CloudBlobDirectory))
                                        {
                                            CloudBlobDirectory dir = (CloudBlobDirectory)item;
                                            BlobResultSegment blobs1 = await dir.ListBlobsSegmentedAsync(blobContinuationToken);
                                            if (dir.Prefix == "4/" || dir.Prefix == "10/" || dir.Prefix == "13/" || dir.Prefix == "14/" || dir.Prefix == "15/" || dir.Prefix == "17/" || dir.Prefix == "18/" || dir.Prefix == "19/" || dir.Prefix == "185/" || dir.Prefix == "192/" || dir.Prefix == "2/" || dir.Prefix == "20/" || dir.Prefix == "21/" || dir.Prefix == "22/" || dir.Prefix == "23/" || dir.Prefix == "24/" || dir.Prefix == "25/" || dir.Prefix == "28/" || dir.Prefix == "3/" || dir.Prefix == "30/" || dir.Prefix == "40/" || dir.Prefix == "5/")
                                            {
                                                foreach (IListBlobItem item1 in blobs1.Results)
                                                {
                                                    try
                                                    {
                                                        if (item1.GetType() == typeof(CloudBlockBlob))
                                                        {
                                                            CloudBlockBlob blob = (CloudBlockBlob)item1;
                                                            string Removedblob = blob.Name.Replace(dir.Prefix, "");
                                                            blobs.Add(Removedblob);
                                                            CloudBlob file = blob;
                                                            string[] splitString = Removedblob.Split('.');
                                                            if (splitString.Length > 1)
                                                            {
                                                                string Exten = splitString[1].Trim();
                                                                if (Exten.Equals("JPG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("PNG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
                                                                {

                                                                    Stream stream = new MemoryStream();
                                                                    string content;
                                                                    var reader = new StreamReader(stream);
                                                                    content = reader.ReadToEnd();
                                                                    if (await file.ExistsAsync())
                                                                    {
                                                                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                                                        fileName = blob.Name;
                                                                        fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";

                                                                        await file.DownloadToStreamAsync(stream);

                                                                        if (stream.CanSeek)
                                                                        {
                                                                            stream.Seek(0, SeekOrigin.Begin);
                                                                        }
                                                                        else
                                                                        {
                                                                            stream.Dispose();
                                                                            stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                                                                        }
                                                                        var optimized = "";
                                                                        const int size = 900;
                                                                        const int quality = 100;
                                                                        using (var image = new MagickImage(stream))
                                                                        {
                                                                            image.Resize(size, size);
                                                                            image.AutoOrient();
                                                                            image.Strip();
                                                                            image.Quality = quality;
                                                                            optimized = image.ToBase64();
                                                                        }

                                                                        var Base64 = optimized;
                                                                        string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                                                                        var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                                                                        convert2 = convert2.Replace("data:image/jpeg;base64,", String.Empty);
                                                                        convert2 = convert2.Replace(" ", "+");
                                                                        int mod4 = convert2.Length % 4;
                                                                        if (mod4 > 0)
                                                                        {
                                                                            convert2 += new string('=', 4 - mod4);
                                                                        }
                                                                        var blockBlob = container.GetBlockBlobReference(blob.Name);
                                                                        var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                                                                        using (var streamnew = new MemoryStream(bytes))
                                                                        {
                                                                            await blockBlob.UploadFromStreamAsync(streamnew);
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (item1.GetType() == typeof(CloudPageBlob))
                                                        {
                                                            CloudPageBlob blob = (CloudPageBlob)item1;
                                                            blobs.Add(blob.Name);
                                                        }
                                                        else if (item1.GetType() == typeof(CloudBlobDirectory))
                                                        {
                                                            CloudBlobDirectory dir1 = (CloudBlobDirectory)item1;
                                                            BlobResultSegment blobs2 = await dir1.ListBlobsSegmentedAsync(blobContinuationToken);
                                                            foreach (IListBlobItem item2 in blobs2.Results)
                                                            {
                                                                try
                                                                {
                                                                    if (item2.GetType() == typeof(CloudBlockBlob))
                                                                    {
                                                                        CloudBlockBlob blob = (CloudBlockBlob)item2;
                                                                        string Removedblob1 = blob.Name.Replace(dir1.Prefix, "");
                                                                        blobs.Add(Removedblob1);
                                                                        string[] splitString = Removedblob1.Split('.');
                                                                        if (splitString.Length > 1)
                                                                        {
                                                                            string Exten = splitString[1].Trim();
                                                                            if (Exten.Equals("JPG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("PNG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
                                                                            {
                                                                                CloudBlob file = blob;
                                                                                Stream stream = new MemoryStream();
                                                                                string content;
                                                                                var reader = new StreamReader(stream);
                                                                                content = reader.ReadToEnd();

                                                                                if (await file.ExistsAsync())
                                                                                {
                                                                                    var blockBlob1 = container.GetBlockBlobReference(blob.Name);
                                                                                    blockBlob1.FetchAttributesAsync();
                                                                                    var sizeofblob = blockBlob1.Properties.Length;

                                                                                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                                                                    fileName = blob.Name;
                                                                                    fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";

                                                                                    await file.DownloadToStreamAsync(stream);

                                                                                    if (stream.CanSeek)
                                                                                    {
                                                                                        stream.Seek(0, SeekOrigin.Begin);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        stream.Dispose();
                                                                                        stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                                                                                    }
                                                                                    var optimized = "";
                                                                                    const int size = 900;
                                                                                    const int quality = 100;
                                                                                    using (var image = new MagickImage(stream))
                                                                                    {
                                                                                        image.Resize(size, size);
                                                                                        image.AutoOrient();
                                                                                        image.Strip();
                                                                                        image.Quality = quality;
                                                                                        optimized = image.ToBase64();
                                                                                    }

                                                                                    var Base64 = optimized;
                                                                                    string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                                                                                    var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                    convert2 = convert2.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                    convert2 = convert2.Replace(" ", "+");
                                                                                    int mod4 = convert2.Length % 4;
                                                                                    if (mod4 > 0)
                                                                                    {
                                                                                        convert2 += new string('=', 4 - mod4);
                                                                                    }
                                                                                    var blockBlob = container.GetBlockBlobReference(blob.Name);
                                                                                    var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                                                                                    using (var streamnew = new MemoryStream(bytes))
                                                                                    {
                                                                                        await blockBlob.UploadFromStreamAsync(streamnew);
                                                                                    }

                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                    else if (item2.GetType() == typeof(CloudPageBlob))
                                                                    {
                                                                        CloudPageBlob blob = (CloudPageBlob)item2;
                                                                        blobs.Add(blob.Name);
                                                                    }
                                                                    else if (item2.GetType() == typeof(CloudBlobDirectory))
                                                                    {
                                                                        CloudBlobDirectory dir2 = (CloudBlobDirectory)item2;
                                                                        BlobResultSegment blobs3 = await dir2.ListBlobsSegmentedAsync(blobContinuationToken);
                                                                        foreach (IListBlobItem item3 in blobs3.Results)
                                                                        {
                                                                            try
                                                                            {
                                                                                if (item3.GetType() == typeof(CloudBlockBlob))
                                                                                {
                                                                                    CloudBlockBlob blob = (CloudBlockBlob)item3;
                                                                                    string Removedblob2 = blob.Name.Replace(dir2.Prefix, "");
                                                                                    blobs.Add(Removedblob2);
                                                                                    CloudBlob file = blob;
                                                                                    string[] splitString = Removedblob2.Split('.');
                                                                                    if (splitString.Length > 1)
                                                                                    {
                                                                                        string Exten = splitString[1].Trim();
                                                                                        if (Exten.Equals("JPG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("PNG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
                                                                                        {
                                                                                            Stream stream = new MemoryStream();
                                                                                            string content;
                                                                                            var reader = new StreamReader(stream);
                                                                                            content = reader.ReadToEnd();
                                                                                            if (await file.ExistsAsync())
                                                                                            {
                                                                                                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                                                                                fileName = blob.Name;
                                                                                                fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";

                                                                                                await file.DownloadToStreamAsync(stream);

                                                                                                if (stream.CanSeek)
                                                                                                {
                                                                                                    stream.Seek(0, SeekOrigin.Begin);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    stream.Dispose();
                                                                                                    stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                                                                                                }
                                                                                                var optimized = "";
                                                                                                const int size = 900;
                                                                                                const int quality = 100;
                                                                                                using (var image = new MagickImage(stream))
                                                                                                {
                                                                                                    image.Resize(size, size);
                                                                                                    image.AutoOrient();
                                                                                                    image.Strip();
                                                                                                    image.Quality = quality;
                                                                                                    optimized = image.ToBase64();
                                                                                                }

                                                                                                var Base64 = optimized;
                                                                                                string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                                                                                                var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                convert2 = convert2.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                convert2 = convert2.Replace(" ", "+");
                                                                                                int mod4 = convert2.Length % 4;
                                                                                                if (mod4 > 0)
                                                                                                {
                                                                                                    convert2 += new string('=', 4 - mod4);
                                                                                                }
                                                                                                var blockBlob = container.GetBlockBlobReference(blob.Name);
                                                                                                var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                                                                                                using (var streamnew = new MemoryStream(bytes))
                                                                                                {
                                                                                                    await blockBlob.UploadFromStreamAsync(streamnew);
                                                                                                }

                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else if (item3.GetType() == typeof(CloudPageBlob))
                                                                                {
                                                                                    CloudPageBlob blob = (CloudPageBlob)item3;
                                                                                    blobs.Add(blob.Name);
                                                                                }
                                                                                else if (item3.GetType() == typeof(CloudBlobDirectory))
                                                                                {
                                                                                    CloudBlobDirectory dir3 = (CloudBlobDirectory)item3;
                                                                                    BlobResultSegment blobs4 = await dir3.ListBlobsSegmentedAsync(blobContinuationToken);
                                                                                    foreach (IListBlobItem item4 in blobs4.Results)
                                                                                    {
                                                                                        try
                                                                                        {
                                                                                            if (item4.GetType() == typeof(CloudBlockBlob))
                                                                                            {
                                                                                                CloudBlockBlob blob = (CloudBlockBlob)item4;
                                                                                                string Removedblob2 = blob.Name.Replace(dir2.Prefix, "");
                                                                                                blobs.Add(Removedblob2);
                                                                                                CloudBlob file = blob;
                                                                                                string[] splitString = Removedblob2.Split('.');
                                                                                                if (splitString.Length > 1)
                                                                                                {
                                                                                                    string Exten = splitString[1].Trim();
                                                                                                    if (Exten.Equals("JPG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("PNG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
                                                                                                    {

                                                                                                        Stream stream = new MemoryStream();
                                                                                                        string content;
                                                                                                        var reader = new StreamReader(stream);
                                                                                                        content = reader.ReadToEnd();
                                                                                                        if (await file.ExistsAsync())
                                                                                                        {
                                                                                                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                                                                                            fileName = blob.Name;
                                                                                                            fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";

                                                                                                            await file.DownloadToStreamAsync(stream);

                                                                                                            if (stream.CanSeek)
                                                                                                            {
                                                                                                                stream.Seek(0, SeekOrigin.Begin);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                stream.Dispose();
                                                                                                                stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                                                                                                            }
                                                                                                            var optimized = "";
                                                                                                            const int size = 900;
                                                                                                            const int quality = 100;
                                                                                                            using (var image = new MagickImage(stream))
                                                                                                            {
                                                                                                                image.Resize(size, size);
                                                                                                                image.AutoOrient();
                                                                                                                image.Strip();
                                                                                                                image.Quality = quality;
                                                                                                                optimized = image.ToBase64();
                                                                                                            }

                                                                                                            var Base64 = optimized;
                                                                                                            string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                                                                                                            var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                            convert2 = convert2.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                            convert2 = convert2.Replace(" ", "+");
                                                                                                            int mod4 = convert2.Length % 4;
                                                                                                            if (mod4 > 0)
                                                                                                            {
                                                                                                                convert2 += new string('=', 4 - mod4);
                                                                                                            }
                                                                                                            var blockBlob = container.GetBlockBlobReference(blob.Name);
                                                                                                            var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                                                                                                            using (var streamnew = new MemoryStream(bytes))
                                                                                                            {
                                                                                                                await blockBlob.UploadFromStreamAsync(streamnew);
                                                                                                            }

                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else if (item4.GetType() == typeof(CloudPageBlob))
                                                                                            {
                                                                                                CloudPageBlob blob = (CloudPageBlob)item4;
                                                                                                blobs.Add(blob.Name);
                                                                                            }
                                                                                            else if (item4.GetType() == typeof(CloudBlobDirectory))
                                                                                            {
                                                                                                CloudBlobDirectory dir4 = (CloudBlobDirectory)item4;
                                                                                                BlobResultSegment blobs5 = await dir4.ListBlobsSegmentedAsync(blobContinuationToken);
                                                                                                foreach (IListBlobItem item5 in blobs5.Results)
                                                                                                {
                                                                                                    try
                                                                                                    {
                                                                                                        if (item5.GetType() == typeof(CloudBlockBlob))
                                                                                                        {
                                                                                                            CloudBlockBlob blob = (CloudBlockBlob)item5;
                                                                                                            string Removedblob2 = blob.Name.Replace(dir2.Prefix, "");
                                                                                                            blobs.Add(Removedblob2);
                                                                                                            CloudBlob file = blob;
                                                                                                            string[] splitString = Removedblob2.Split('.');
                                                                                                            if (splitString.Length > 1)
                                                                                                            {
                                                                                                                string Exten = splitString[1].Trim();
                                                                                                                if (Exten.Equals("JPG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("PNG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
                                                                                                                {
                                                                                                                    Stream stream = new MemoryStream();
                                                                                                                    string content;
                                                                                                                    var reader = new StreamReader(stream);
                                                                                                                    content = reader.ReadToEnd();
                                                                                                                    if (await file.ExistsAsync())
                                                                                                                    {
                                                                                                                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                                                                                                        fileName = blob.Name;
                                                                                                                        fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";

                                                                                                                        await file.DownloadToStreamAsync(stream);

                                                                                                                        if (stream.CanSeek)
                                                                                                                        {
                                                                                                                            stream.Seek(0, SeekOrigin.Begin);
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            stream.Dispose();
                                                                                                                            stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                                                                                                                        }
                                                                                                                        var optimized = "";
                                                                                                                        const int size = 900;
                                                                                                                        const int quality = 100;
                                                                                                                        using (var image = new MagickImage(stream))
                                                                                                                        {
                                                                                                                            image.Resize(size, size);
                                                                                                                            image.AutoOrient();
                                                                                                                            image.Strip();
                                                                                                                            image.Quality = quality;
                                                                                                                            optimized = image.ToBase64();
                                                                                                                        }

                                                                                                                        var Base64 = optimized;
                                                                                                                        string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                                                                                                                        var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                                        convert2 = convert2.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                                        convert2 = convert2.Replace(" ", "+");
                                                                                                                        int mod4 = convert2.Length % 4;
                                                                                                                        if (mod4 > 0)
                                                                                                                        {
                                                                                                                            convert2 += new string('=', 4 - mod4);
                                                                                                                        }
                                                                                                                        var blockBlob = container.GetBlockBlobReference(blob.Name);
                                                                                                                        var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                                                                                                                        using (var streamnew = new MemoryStream(bytes))
                                                                                                                        {
                                                                                                                            await blockBlob.UploadFromStreamAsync(streamnew);
                                                                                                                        }


                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        else if (item5.GetType() == typeof(CloudPageBlob))
                                                                                                        {
                                                                                                            CloudPageBlob blob = (CloudPageBlob)item5;
                                                                                                            blobs.Add(blob.Name);
                                                                                                        }
                                                                                                        else if (item5.GetType() == typeof(CloudBlobDirectory))
                                                                                                        {
                                                                                                            CloudBlobDirectory dir5 = (CloudBlobDirectory)item5;
                                                                                                            BlobResultSegment blobs6 = await dir5.ListBlobsSegmentedAsync(blobContinuationToken);
                                                                                                            foreach (IListBlobItem item6 in blobs6.Results)
                                                                                                            {
                                                                                                                try
                                                                                                                {
                                                                                                                    if (item6.GetType() == typeof(CloudBlockBlob))
                                                                                                                    {
                                                                                                                        CloudBlockBlob blob = (CloudBlockBlob)item6;
                                                                                                                        string Removedblob2 = blob.Name.Replace(dir2.Prefix, "");
                                                                                                                        blobs.Add(Removedblob2);
                                                                                                                        CloudBlob file = blob;
                                                                                                                        string[] splitString = Removedblob2.Split('.');
                                                                                                                        if (splitString.Length > 1)
                                                                                                                        {
                                                                                                                            string Exten = splitString[1].Trim();
                                                                                                                            if (Exten.Equals("JPG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("PNG", StringComparison.OrdinalIgnoreCase) || Exten.Equals("Jpeg", StringComparison.OrdinalIgnoreCase))
                                                                                                                            {

                                                                                                                                Stream stream = new MemoryStream();
                                                                                                                                string content;
                                                                                                                                var reader = new StreamReader(stream);
                                                                                                                                content = reader.ReadToEnd();
                                                                                                                                if (await file.ExistsAsync())
                                                                                                                                {
                                                                                                                                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                                                                                                                    fileName = blob.Name;
                                                                                                                                    fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";

                                                                                                                                    await file.DownloadToStreamAsync(stream);

                                                                                                                                    if (stream.CanSeek)
                                                                                                                                    {
                                                                                                                                        stream.Seek(0, SeekOrigin.Begin);
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        stream.Dispose();
                                                                                                                                        stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                                                                                                                                    }
                                                                                                                                    var optimized = "";
                                                                                                                                    const int size = 900;
                                                                                                                                    const int quality = 100;
                                                                                                                                    using (var image = new MagickImage(stream))
                                                                                                                                    {
                                                                                                                                        image.Resize(size, size);
                                                                                                                                        image.AutoOrient();
                                                                                                                                        image.Strip();
                                                                                                                                        image.Quality = quality;
                                                                                                                                        optimized = image.ToBase64();
                                                                                                                                    }

                                                                                                                                    var Base64 = optimized;
                                                                                                                                    string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                                                                                                                                    var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                                                    convert2 = convert2.Replace("data:image/jpeg;base64,", String.Empty);
                                                                                                                                    convert2 = convert2.Replace(" ", "+");
                                                                                                                                    int mod4 = convert2.Length % 4;
                                                                                                                                    if (mod4 > 0)
                                                                                                                                    {
                                                                                                                                        convert2 += new string('=', 4 - mod4);
                                                                                                                                    }
                                                                                                                                    var blockBlob = container.GetBlockBlobReference(blob.Name);
                                                                                                                                    var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                                                                                                                                    using (var streamnew = new MemoryStream(bytes))
                                                                                                                                    {
                                                                                                                                        await blockBlob.UploadFromStreamAsync(streamnew);
                                                                                                                                    }

                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else if (item6.GetType() == typeof(CloudPageBlob))
                                                                                                                    {
                                                                                                                        CloudPageBlob blob = (CloudPageBlob)item6;
                                                                                                                        blobs.Add(blob.Name);
                                                                                                                    }
                                                                                                                    else if (item6.GetType() == typeof(CloudBlobDirectory))
                                                                                                                    {
                                                                                                                        CloudBlobDirectory dir6 = (CloudBlobDirectory)item6;
                                                                                                                    }
                                                                                                                }
                                                                                                                catch (Exception ex)
                                                                                                                {
                                                                                                                    Console.WriteLine(ex.Message);
                                                                                                                    continue;
                                                                                                                }

                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    catch (Exception ex)
                                                                                                    {
                                                                                                        Console.WriteLine(ex.Message);
                                                                                                        continue;
                                                                                                    }

                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        catch (Exception ex)
                                                                                        {
                                                                                            Console.WriteLine(ex.Message);
                                                                                            continue;
                                                                                        }

                                                                                    }
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                Console.WriteLine(ex.Message);
                                                                                continue;
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Console.WriteLine(ex.Message);
                                                                    continue;
                                                                }

                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                        continue;
                                                    }

                                                }
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                continue;
                            }

                        }

                    }
                }
            }

            return fileName;
        }

        [Route("SaveRemaindersRemarks")]
        public LoginDTO SaveRemaindersRemarks([FromBody] LoginDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.Save_Remainders_Remarks != null && data.Save_Remainders_Remarks.Length > 0)
                {
                    foreach (var c in data.Save_Remainders_Remarks)
                    {
                        IVRM_Payment_Subscription_Login_RemarksDMO iVRM_Payment_Subscription_RemarksDetilsDMO = new IVRM_Payment_Subscription_Login_RemarksDMO();
                        iVRM_Payment_Subscription_RemarksDetilsDMO.UserId = data.userId;
                        iVRM_Payment_Subscription_RemarksDetilsDMO.MI_Id = data.MI_ID;
                        iVRM_Payment_Subscription_RemarksDetilsDMO.IVRMPSLR_LoginDatetime = indiantime0;
                        iVRM_Payment_Subscription_RemarksDetilsDMO.CreatedDate = indiantime0;
                        iVRM_Payment_Subscription_RemarksDetilsDMO.UpdatedDate = indiantime0;
                        iVRM_Payment_Subscription_RemarksDetilsDMO.IVRMPSLR_Remarks = c.Remarks;
                        iVRM_Payment_Subscription_RemarksDetilsDMO.IVRMPSLR_RemainderTemplateName = c.RemainderTemplateName;

                        _db.Add(iVRM_Payment_Subscription_RemarksDetilsDMO);
                    }

                    var i = _db.SaveChanges();

                    if (i > 0)
                    {
                        data.returnMsg = "Save";
                    }
                    else
                    {
                        data.returnMsg = "Failed";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string geterrormessage(LoginDTO dto)
        {
            List<LoginDTO> errormessage = new List<LoginDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Dashboard_Mobile_Disable_Alertmessage";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.Mi_Id_List[0].MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                 SqlDbType.VarChar)
                    {
                        Value = dto.disableflag
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                errormessage.Add(new LoginDTO
                                {
                                    messag = Convert.ToString(dataReader["messag"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            if (errormessage.Count > 0)
            {
                dto.messag = errormessage.FirstOrDefault().messag;
            }

            return dto.messag;

        }
        [Route("getOTPForEmail")]
        public CommonDTO getOTPForEmail([FromBody] CommonDTO API)
        {

            CommonDTO lgnDto = new CommonDTO();

            string genotp = "";
            CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
            genotp = generate.getOTP();


            if (genotp.Length < 6)
            {
                genotp = generate.getOTP();
            }
            else
            {
                Email Email = new Email(_db);
                string s = Email.sendmail(API.mi_id, API.mobileNo, "EMAILOTP", Convert.ToInt64(genotp));
                lgnDto.displaymessage = genotp;
                lgnDto.Message = "Please Check Your Email,OTP Is Sent To Your Email Id";
            }

            lgnDto.displaymessage = genotp;
            return lgnDto;  // changed return type on 15-12-2022
        }
    }
    public class filedetails
    {
        public string name { get; set; }
        public string path { get; set; }
    }
}
