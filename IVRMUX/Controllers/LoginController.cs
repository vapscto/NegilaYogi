using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Authorization;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using System.IO;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using SendGrid;
using System.Text;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class LoginController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<LoginController> _logger;
        private readonly UserManager<ApplicationUser> _UserManager;

        Logindelegate pre = new Logindelegate();
        registrationdelegate registr = new registrationdelegate();
        registrationcontext _registrationcontext;
        DomainModelMsSqlServerContext _db;
        ApplicationDBContext _ApplicationDBContext;

        public LoginController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> UserManager, DomainModelMsSqlServerContext context, ILogger<LoginController> log)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _context = context;
            _UserManager = UserManager;
            _logger = log;
        }

        [HttpGet]

        [Route("getMIdata")]
        public LoginDTO getMIdata(regis reg)
        {
            //Removing Session value when user logs in.
            //Added on 05-03-2018
            HttpContext.Session.Remove("Session_Trip_IsOnline");
            HttpContext.Session.Remove("Session_Trip_MI_Id");
            HttpContext.Session.Remove("Session_Trip_ASMAY_Id");

            LoginDTO lgnDto = pre.getMIdata(reg);
            return lgnDto;
        }

        [Route("getMIdataMaster/{id:int}")]
        public LoginDTO getMIdataMaster(int id)
        {
            regis reg = new regis();


            reg.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            LoginDTO lgnDto = pre.getMIdataMaster(reg);

            return lgnDto;
        }

        [Route("getsiblinglist/{id:int}")]
        public LoginDTO getsiblinglist(int id)
        {
            regis reg = new regis();

            reg.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            reg.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            LoginDTO lgnDto = pre.getsiblinglist(reg);

            return lgnDto;
        }

        [HttpPost]

        [Route("Regis")]
        public string regis(FileDescriptionDTO regdata)//[FromBody]
        {
            regis reg = new regis();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            List<MasterAcademic> allyear = new List<MasterAcademic>();
            List<MasterAcademic> allyearget = new List<MasterAcademic>();

            var EMAILOTPSTATUS = HttpContext.Session.GetString("EmailOTPStatus");
            var MOBILEOTPSTATUS = HttpContext.Session.GetString("MobileOTPStatus");
            // Mobile And Email OTP CODE
            //  START
            //if (regdata.Otpemail == "true")
            //{
            //    if (EMAILOTPSTATUS == "Fail" && EMAILOTPSTATUS == null)
            //    {
            //        reg.returnMsg = "EMAIL NOT VERIFIED.!";
            //    }
            //}
            //else
            //{
            //    EMAILOTPSTATUS = "Success";
            //}
            //if (regdata.Otpmobl == "true")
            //{
            //    if (MOBILEOTPSTATUS == "Fail" && MOBILEOTPSTATUS == null)
            //    {
            //        reg.returnMsg = "MOBILE NOT VERIFIED.!";
            //    }
            //}
            //else
            //{
            //    MOBILEOTPSTATUS = "Success";
            //}
            //if (regdata.Otpemail == "true" && regdata.Otpmobl == "true")
            //{
            //    if (EMAILOTPSTATUS == "Fail" && MOBILEOTPSTATUS == "Fail")
            //    {
            //        reg.returnMsg = "EMAIL & MOBILE NUMBER NOT VERIFIED.!";
            //    }
            //    if (EMAILOTPSTATUS == null && MOBILEOTPSTATUS == null)
            //    {
            //        reg.returnMsg = "EMAIL & MOBILE NUMBER NOT VERIFIED.!";
            //    }
            //}
            //else
            //{
            //    EMAILOTPSTATUS = "Success";
            //    MOBILEOTPSTATUS = "Success";
            //}
            //if (EMAILOTPSTATUS == "Success" && MOBILEOTPSTATUS == "Success")
            //{
            // Mobile And Email OTP CODE
            //  END 
            // Added on 11-11-2016 to store Institute id, ip address and created date to application user

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;

            //var RemotePort = HttpContext.Connection.RemotePort;
            //var RemotePortivp4 = HttpContext.Connection.RemoteIpAddress.MapToIPv4();
            //var RemotePortivp4local = HttpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6;
            //var ClientCertificate = HttpContext.Connection.ClientCertificate;
            //string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            //// Get the IP  
            //string myIP = Dns.GetHostAddressesAsync(hostName).Result.FirstOrDefault().AddressFamily.ToString();
            //var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
            //string myIP1  = ip_list.ToString();

            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
            CommonDTO cmnds = new CommonDTO();
            if (vr_id != 0)
            {
                cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                reg.MI_Id = cmnds.IVRM_MI_Id;
            }
            else
            {
                reg.MI_Id = Convert.ToInt64(regdata.Intid);
            }
            //CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);
            allyearget = (from a in _context.AcademicYear
                          where (a.MI_Id == reg.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == reg.MI_Id)
                          select new MasterAcademic
                          {
                              ASMAY_Id = a.ASMAY_Id,
                              ASMAY_Year = a.ASMAY_Year
                          }
                        ).ToList();
            allyear = (from a in _context.AcademicYear
                       where (a.MI_Id == reg.MI_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                       select new MasterAcademic
                       {
                           ASMAY_Id = a.ASMAY_Id,
                           ASMAY_Year = a.ASMAY_Year,
                           ASMAY_Order = a.ASMAY_Order
                       }
                     ).OrderByDescending(d => d.ASMAY_Order).ToList();
            if (allyear.Count > 0 || regdata.Alumni == "Alumni" || regdata.Special == "Special" || regdata.Special == "Careers")
            {
                int asmayid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
                reg.ASMAY_Id = asmayid;
                // reg.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
                reg.Entry_Date = DateTime.Now;
                reg.Machine_Ip_Address = remoteIpAddress.ToString();
                reg.Email_id = regdata.Email_id;
                reg.Mobileno = regdata.Mobileno;
                reg.password = regdata.Password;
                reg.username = regdata.Username;
                reg.admission = regdata.admission;
                reg.admittedyer = regdata.admittedyer;
                reg.leftclassid = regdata.leftclassid;
                reg.classidadmitted = regdata.classidadmitted;
                reg.leftyear = regdata.leftyear;
                reg.Name = regdata.Name;
                reg.Special = regdata.Special;
                reg.CreatedDate = DateTime.Now;
                reg.UpdatedDate = DateTime.Now;
                reg.Alumni = regdata.Alumni;
                reg.courseadmittted = regdata.courseadmittted;
                reg.courseleft = regdata.courseleft;
                reg.branchadmittted = regdata.branchadmittted;
                reg.branchleft = regdata.branchleft;
                reg.semadmittted = regdata.semadmittted;
                reg.semleft = regdata.semleft;
                //string webRootPath = _hostingEnvironment.WebRootPath;
                //if (regdata.File != null)
                //{
                //    foreach (var files in regdata.File)
                //    {
                //        if (files.Length > 0)
                //        {
                //            using (var reader = new StreamReader(files.OpenReadStream()))
                //            {
                //                var fileContent = reader.ReadToEnd();
                //                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(files.ContentDisposition);
                //                Console.WriteLine(fileContent.ToString());
                //            }
                //            var file = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"').Split('.');
                //            var fileName = file[0];
                //            var extension = file[1];
                //            var name = fileName + "_" + Guid.NewGuid() + "." + extension;
                //            //contentTypes.Add(file.ContentType);
                //            //  names.Add(file);
                //            string ss = files.ContentDisposition.ToString();
                //            Console.WriteLine(ss);
                //            var uploads = Path.Combine(webRootPath, "images\\uploads\\Profile_Pics\\");
                //            var filePath = uploads + name;
                //            files.CopyTo(new FileStream(filePath, FileMode.Create));
                //            reg.profile_pic_path = filePath;
                //        }
                //    }
                //}
                //else
                //{
                //    var uploads1 = "images\\uploads\\Profile_Pics\\a_1c6e77cd-1ba4-45eb-959a-19b80a5c0dfc.jpg";

                //    reg.profile_pic_path = uploads1;
                //}
                if (regdata.onlinepreadminfilepath != null)
                {
                    reg.profile_pic_path = regdata.onlinepreadminfilepath;
                }
                else
                {
                    reg.profile_pic_path = "images\\uploads\\Profile_Pics\\a_1c6e77cd-1ba4-45eb-959a-19b80a5c0dfc.jpg";
                }
                //Date:15-12-2016 Email Confirmation.
                //reg = registr.paymentPart(reg);
                reg = registr.regdata(reg);
                if (reg.returnMsg == "created")
                {
                    string confirmationLink = Url.Action("ConfirmEmail", "Login", new
                    {
                        userid = reg.User_Id,
                        token = reg.token
                    }, protocol: HttpContext.Request.Scheme);
                    reg.confirmationLink = confirmationLink;
                    // registr.sendMail(reg);     
                    // reg.MI_Id = cmnds.IVRM_MI_Id;
                    //var acd_Id = _context.AcademicYear.Where(t => t.MI_Id.Equals(reg.MI_Id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                    var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _context.mstConfig.Where(d => d.MI_Id.Equals(reg.MI_Id) && d.ASMAY_Id.Equals(Acdemic_preadmission)).ToList();
                    //  stu.mstConfig = mstConfig.ToArray();

                    if (mstConfig.FirstOrDefault().ISPAC_RegMailFlag == 1 && (regdata.Special == "" || regdata.Special == null) && regdata.Alumni != "Alumni")
                    {
                        Email Email = new Email(_context);
                        string s = Email.sendmailreg(reg.MI_Id, regdata.Email_id, "REG", reg.User_Id, reg.password);
                    }

                    //var message = new SendGrid.SendGridMessage();
                    //message.From = new MailAddress("vapstech123@gmail.com", "vaps@123");
                    //message.Subject = "Account Created";
                    //message.AddTo(regdata.Email_id);
                    //message.Html = "<p>Account Created Successfully,Thank You</p>";
                    //var client = new Web("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");
                    //client.DeliverAsync(message).Wait();

                    if (mstConfig.FirstOrDefault().ISPAC_RegSMSFlag == 1 && (regdata.Special == "" || regdata.Special == null) && regdata.Alumni != "Alumni")
                    {
                        SMS sms = new SMS(_context);
                        string sm = sms.sendSmsreg(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), "REG", reg.User_Id, reg.password).Result;
                    }


                    if (regdata.Special == "Special")
                    {
                        Email Email = new Email(_context);
                        string s = Email.sendmail(reg.MI_Id, regdata.Email_id, "SPECIALREG", reg.User_Id);

                        SMS sms = new SMS(_context);
                        string sm = sms.sendSms(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), "SPECIALREG", reg.User_Id).Result;
                    }

                    if (regdata.Special == "Careers")
                    {
                        Email Email = new Email(_context);
                        string s = Email.sendmail(reg.MI_Id, regdata.Email_id, "CareersREG", reg.User_Id);

                        SMS sms = new SMS(_context);
                        string sm = sms.sendSms(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), "CareersREG", reg.User_Id).Result;
                    }

                    if (regdata.Alumni == "Alumni")
                    {
                        SMS sms = new SMS(_context);
                        string sm = sms.sendSmsreg(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), "ALUMNIREG", reg.User_Id, reg.password).Result;

                        Email Email = new Email(_context);
                        string s = Email.sendmailreg(reg.MI_Id, regdata.Email_id, "ALUMNIREG", reg.User_Id, reg.password);
                    }

                    //SMS sms = new SMS(_context);
                    string Smsmessage = "Account Created Successfully,Thank You";
                    //sms.sendSms(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), reg.User_Id, "Newaccount", Smsmessage);

                }
                else
                {
                    // reg.returnMsg;
                }
                // Mobile And Email OTP CODE

                //}
                //  END 
            }
            else
            {
                reg.returnMsg = "No";
            }
            return reg.returnMsg;
        }

        [Route("paynow")]
        public FileDescriptionDTO paynow(FileDescriptionDTO regdata)//[FromBody]
        {
            regis reg = new regis();

            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

            CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            reg.MI_Id = cmnds.IVRM_MI_Id;

            int asmayid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            reg.ASMAY_Id = asmayid;

            // reg.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            reg.Entry_Date = DateTime.Now;

            reg.Email_id = regdata.Email_id;
            reg.Mobileno = regdata.Mobileno;
            reg.password = regdata.Password;
            reg.username = regdata.Username;
            reg.Name = regdata.Name;
            reg.CreatedDate = DateTime.Now;
            reg.UpdatedDate = DateTime.Now;




            //Date:15-12-2016 Email Confirmation.

            reg = registr.paymentPart(reg);
            //  reg = registr.regdata(reg);

            regdata.PaymentDetailsList = reg.PaymentDetailsList;
            regdata.Description = reg.returnMsg;


            return regdata;
        }

        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            FileDescriptionDTO reg = new FileDescriptionDTO();
            reg.Email_id = response.email;
            reg.Mobileno = Convert.ToString(response.phone);
            reg.Name = response.firstname;
            reg.Username = response.udf2;
            reg.Password = response.udf4;

            int miiid = Convert.ToInt32(response.udf3);

            //var acd_Id = _context.AcademicYear.Where(t => t.MI_Id== miiid && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

            var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == miiid).Select(d => d.ASMAY_Id).FirstOrDefault();


            response.udf6 = Convert.ToString(Acdemic_preadmission);

            LoginDTO log = new LoginDTO();

            log = getsubdomain(log);
            var sub = log.subDomainName;
            string querystring = "";
            try
            {

                if (response.status == "success")
                {
                    reg.Description = regis(reg);
                    dto = pre.getpaymentresponse(response);
                    if (reg.Description == "created")
                    {
                        querystring = "http://localhost:57606/#/login/" + sub + "?status=" + reg.Description;
                    }
                    else if (reg.Description == "Username Already exist .. !!")
                    {
                        querystring = "http://localhost:57606/#/login/" + sub + "?status=" + reg.Description;
                    }
                    else
                    {
                        reg.Description = "checkadmin";
                        querystring = "http://localhost:57606/#/login/" + sub + "?status=" + reg.Description;
                    }
                }
                else if (response.status == "failure")
                {

                    querystring = "http://localhost:57606/#/login/" + sub + "?status=failure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            //return Redirect(querystring);

            return Redirect(querystring);
        }

        public LoginDTO Getdata(regis reg)  // Changed return type from string on 5-11-2016
        {
            reg.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            LoginDTO lgnDto = pre.getregdata(reg);

            if (lgnDto.roleId != 0)
            {
                // Added on 16-11-2016 to set MI Id
                HttpContext.Session.SetInt32("Session_MI_Id", Convert.ToInt32(lgnDto.MI_ID));

                // Added on 16-11-2016
                HttpContext.Session.SetInt32("RoleId", Convert.ToInt32(lgnDto.roleId));  // Changed on 5-11-2016
                                                                                         // Added on 25-11-2016
                HttpContext.Session.SetInt32("UserId", Convert.ToInt32(lgnDto.userId));

                HttpContext.Session.SetInt32("UserName", Convert.ToInt32(lgnDto.UserName));

                HttpContext.Session.SetInt32("Role", Convert.ToInt32(lgnDto.role));

                HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(lgnDto.ASMAY_Id));

                HttpContext.Response.Cookies.Append("IsLogged", "true", new Microsoft.AspNetCore.Http.CookieOptions() { Path = "/", HttpOnly = false, Secure = false });
                HttpContext.Session.SetInt32("AMST_Id", Convert.ToInt32(lgnDto.AMST_Id));

                HttpContext.Session.SetInt32("Userpassword", Convert.ToInt32(reg.password));
            }
            else
            {
                HttpContext.Response.Cookies.Append("IsLogged", "false");
                //lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            }

            return lgnDto;  // Changed return type from string on 5-11-2016
        }

        [Route("logoutCookie")]
        public LoginDTO clearCookieData()
        {
            LoginDTO lgnDto = new LoginDTO();
            Response.Cookies.Append("IsLogged", "false");
            lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            return lgnDto;
        }
        // Added on 5-11-2016 to reset cookie flag to false

        [Route("VerifyUserName")]
        public LoginDTO VerifyUserName([FromBody] LoginDTO UserName)
        {
            return UserName = pre.VerifyUserName(UserName);
        }

        [Route("clearsession/{id:int}")]
        public CommonDTO clearsession(int id)
        {
            CommonDTO dto = new CommonDTO();
            int instiid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int virtualid = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
            dto.subDomainNamelogout = HttpContext.Session.GetString("sessionsubdomainname");
            HttpContext.Session.Clear();
            return dto;
        }

        [Route("setvirtauldeatilsnew")]
        public CommonDTO setVirtualDataToSessionnew([FromBody] CommonDTO dto)
        {
            int checkuser = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (checkuser != 0)
            {
                dto.multiplewindowvalue = "differentlogin";
                dto.subDomainName = "0";
            }
            else
            {
                var subDomain = string.Empty;
                var host = HttpContext.Request.QueryString.ToUriComponent();
                var host1 = HttpContext.Request.QueryString;
                HttpContext.Session.SetString("session_subdomainname", "");
                HttpContext.Session.Clear();
                string checksession = HttpContext.Session.GetString("sessionsubdomainname");
                if (!string.IsNullOrWhiteSpace(host))
                {
                    subDomain = host.Split('.')[0];
                    dto.subDomainName = subDomain.Trim().ToLower();
                    HttpContext.Session.SetString("session_subdomainname", dto.subDomainName);
                    HttpContext.Session.SetString("sessionsubdomainname", dto.subDomainName);
                }
                if (dto.subDomainName != "" && dto.subDomainName != null && dto.subDomainName != "0")
                {
                    HttpContext.Session.SetString("session_subdomainname", dto.subDomainName);
                    HttpContext.Session.SetString("sessionsubdomainname", dto.subDomainName);

                    CommonDTO cmn = pre.setSessionDataForVirtualSchoolsubdomain(dto);

                    if (cmn.Message != "NoINT")
                    {
                        HttpContext.Session.SetInt32("session_virtualid", Convert.ToInt32(cmn.virtualid));
                        HttpContext.Session.SetInt32("Session_MI_Id_Preadmission", cmn.IVRM_MI_Id);
                        int orgid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                        dto.mi_id = cmn.IVRM_MI_Id;
                        LoginDTO lgnDto = pre.getconfig(dto);
                        dto.ivrmconfiglist = lgnDto.ivrmconfiglist;
                        dto.fillinstitution = lgnDto.fillinstitution;
                        dto.EmailarrayList = lgnDto.EmailarrayList;
                        dto.PhonearrayList = lgnDto.PhonearrayList;
                        dto.payment = lgnDto.payment;
                        dto.registration = lgnDto.registration;
                        dto.allyearlist = lgnDto.allyeargetlogin;
                        dto.allclasslist = lgnDto.allclasslogin;
                        if (lgnDto.prestartdate != null)
                        {
                            dto.prestartdate = Convert.ToDateTime(lgnDto.prestartdate);
                            dto.presenddate = Convert.ToDateTime(lgnDto.presenddate);
                            dto.CreatedDate = Convert.ToDateTime(lgnDto.CreatedDate);
                        }
                        HttpContext.Session.SetInt32("otpsms", Convert.ToInt32(cmn.virtualid));
                        if (dto.IVRM_MO_Id != 0 || orgid != 0)
                        {
                            dto.multiplewindowvalue = "differentlogin";
                        }
                        else if (dto.IVRM_MO_Id == 0)
                        {
                            dto.multiplewindowvalue = "logout";
                        }
                        dto.Noint = false;
                    }
                    else
                    {
                        dto.Noint = true;
                    }
                }
                else
                {
                    if (dto.hostname != "")
                    {
                        LoginDTO lgnDto = pre.getconfigtrust(dto);
                        dto.fillinstitution = lgnDto.fillinstitution;
                        dto.EmailarrayList = lgnDto.EmailarrayList;
                        dto.PhonearrayList = lgnDto.PhonearrayList;
                        dto.institutiondetails = lgnDto.institutiondetails;
                        dto.Noint = false;
                    }
                }
            }

            return dto;
        }

        [Route("setvirtauldeatils/{id:int}")]
        public CommonDTO setVirtualDataToSession(int id)
        {
            CommonDTO dto = new CommonDTO();
            var subDomain = string.Empty;

            var host = HttpContext.Request.Host.Host;

            if (!string.IsNullOrWhiteSpace(host))
            {
                subDomain = host.Split('.')[0];
                dto.subDomainName = subDomain.Trim().ToLower();
            }

            dto.IVRM_MO_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));

            if (dto.IVRM_MO_Id != 0)
            {
                dto.multiplewindowvalue = "differentlogin";
            }

            if (true && dto.subDomainName == "")
            {
                CommonDTO cmn = pre.setSessionDataForVirtualSchool(id);
                HttpContext.Session.SetInt32("Session_MO_Id", cmn.IVRM_MO_Id);
                HttpContext.Session.SetInt32("Session_MI_Id", cmn.IVRM_MI_Id);
                _logger.LogInformation("Development");
                _logger.LogInformation(cmn.IVRM_MO_Id.ToString());
                _logger.LogInformation(cmn.IVRM_MI_Id.ToString());

                if (cmn.IVRM_MO_Id != 0)
                {
                    dto.multiplewindowvalue = "multiplevalue";
                }
            }
            else
            {
                CommonDTO cmn = pre.setSessionDataForVirtualSchoolsubdomain(dto);
                HttpContext.Session.SetInt32("Session_MO_Id", cmn.IVRM_MO_Id);
                HttpContext.Session.SetInt32("Session_MI_Id", cmn.IVRM_MI_Id);
                _logger.LogInformation("Development");
                _logger.LogInformation(cmn.IVRM_MO_Id.ToString());
                _logger.LogInformation(cmn.IVRM_MI_Id.ToString());

                if (cmn.IVRM_MO_Id != 0)
                {
                    dto.multiplewindowvalue = "multiplevalue";
                }
            }

            if (dto.IVRM_MO_Id == 0)
            {
                dto.multiplewindowvalue = "logout";
            }

            return dto;
        }

        [Route("getRole")]
        public LoginDTO getLoginRole()
        {
            CommonDTO cmn = new CommonDTO();

            cmn.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            cmn.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            cmn.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            cmn.IVRM_MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            cmn.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            cmn.PaymentNootificationGeneral = Convert.ToInt32(HttpContext.Session.GetInt32("PaymentNootificationGeneral"));
            cmn.smscreditalert = Convert.ToInt32(HttpContext.Session.GetInt32("smscreditalert"));




            cmn.username = Convert.ToString(HttpContext.Session.GetString("UserName"));

            cmn.amst_id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return pre.getRoleData(cmn);
        }

        [Route("getRoleoForChangeInstitute/{id:int}")]
        public LoginDTO getRoleoForChangeInstitute(int id)
        {
            LoginDTO LoginDTO = new LoginDTO();
            CommonDTO cmn = new CommonDTO();

            regis reg = new regis();
            reg.MI_Id = id;
            // Added on 16-11-2016 to set MI Id
            HttpContext.Session.SetInt32("Session_MI_Id", Convert.ToInt32(reg.MI_Id));
            reg.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            reg.password = Convert.ToString(HttpContext.Session.GetString("Userpassword"));

            var acd_Id = _context.AcademicYear.Where(t => t.MI_Id.Equals(reg.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
            //LoginDTO lgnDto = pre.getregdata(reg);
            //if (lgnDto.roleId != 0)
            //{
            HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(acd_Id));
            //}
            //else
            //{
            //    HttpContext.Response.Cookies.Append("IsLogged", "false");
            //    //lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            //}

            return LoginDTO;
        }

        [Route("getRoleoForChangesibling/{id:int}")]
        public LoginDTO getRoleoForChangesibling(int id)
        {
            LoginDTO LoginDTO = new LoginDTO();
            CommonDTO cmn = new CommonDTO();

            regis reg = new regis();
            reg.AMST_Id = id;
            LoginDTO.AMST_Id = id;
            // Added on 16-11-2016 to set MI Id
            HttpContext.Session.SetInt32("AMST_Id", Convert.ToInt32(reg.AMST_Id));
            reg.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            reg.password = Convert.ToString(HttpContext.Session.GetString("Userpassword"));

            //var acd_Id = _context.AcademicYear.Where(t => t.MI_Id.Equals(reg.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
            ////LoginDTO lgnDto = pre.getregdata(reg);
            ////if (lgnDto.roleId != 0)
            ////{
            //HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(acd_Id));
            //}
            //else
            //{
            //    HttpContext.Response.Cookies.Append("IsLogged", "false");
            //    //lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            //}

            return LoginDTO;
        }

        [Route("getsubdomain")]
        public LoginDTO getsubdomain(LoginDTO LoginDTO)
        {


            LoginDTO.subDomainName = HttpContext.Session.GetString("session_subdomainname");
            return LoginDTO;
        }

        [Route("sendforgotpassword")]
        public LoginDTO forgotPassword([FromBody] LoginDTO email)
        {
            LoginDTO LoginDTO = new LoginDTO();

            var EMAILOTPSTATUS = "";
            var MOBILEOTPSTATUS = "";


            if (email.ForgetPasswordSelection == "EMAIL")
            {
                EMAILOTPSTATUS = HttpContext.Session.GetString("EmailOTPStatus");

                if (EMAILOTPSTATUS == "Fail" || EMAILOTPSTATUS == null)
                {
                    email.returnMsg = "EMAIL NOT VERIFIED.!";
                }
            }

            if (email.ForgetPasswordSelection == "MOBILE")
            {
                MOBILEOTPSTATUS = HttpContext.Session.GetString("MobileOTPStatus");

                if (MOBILEOTPSTATUS == "Fail" && MOBILEOTPSTATUS == null)
                {
                    email.returnMsg = "MOBILE NOT VERIFIED.!";
                }
            }


            if (EMAILOTPSTATUS == "Success" || MOBILEOTPSTATUS == "Success")
            {
                //email.userOTP = HttpContext.Session.GetString("OTP");
                LoginDTO = pre.forgotPassword(email);
                //HttpContext.Session.GetString("session_subdomainname", dto.subDomainName);
                LoginDTO.subDomainName = HttpContext.Session.GetString("session_subdomainname");
            }

            return LoginDTO;

        }

        // send forgot password to user email added on 10-11-2016
        [Route("ConfirmEmail")]
        public ActionResult ConfirmEmail(string userid, string token)
        {
            regis reg = new regis();
            reg.User_Id = Convert.ToInt64(userid);
            reg.token = token;
            bool val = registr.ConfirmMail(reg);
            if (val == true)
            {
                // reg.emailSuccessmsg = "Email confirmed successfully!";
                return Redirect("http://localhost:57606/#/login/100");
            }
            else
            {
                //  reg.emailSuccessmsg = "Error while confirming your email!";
                return View("");
            }

        }

        [Route("getOTPForMobile")]
        public async Task<string> getOTPForMobile([FromBody] LoginDTO mobno)
        {
            ApplicationUser user = new ApplicationUser();
            user = await _UserManager.FindByNameAsync(mobno.UserName);
            if (user != null)
            {
                List<UserRoleWithInstituteDMO> Mi_id_list = new List<UserRoleWithInstituteDMO>();
                Mi_id_list = _context.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();
                if (Mi_id_list.Count() > 0)
                {
                    string genotp = "";
                    mobno.MI_ID = Mi_id_list.FirstOrDefault().MI_Id;
                    HttpContext.Session.SetString("exptimeFM", DateTime.Now.ToString());
                    CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                    genotp = generate.getOTP();
                    if (genotp.Length < 6)
                    {
                        genotp = generate.getOTP();
                    }
                    else
                    {
                        HttpContext.Session.SetString("MOBILEOTP", genotp);
                        SMS sms = new SMS(_context);
                        string s = await sms.sendSms(mobno.MI_ID, Convert.ToInt64(mobno.mobileNo), "EMAILOTP", Convert.ToInt64(genotp));
                        mobno.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number";
                    }
                }
            }
            return mobno.message;
        }

        [Route("VerifymobileOtp")]
        public string VerifymobileOtp([FromBody] LoginDTO mobno)
        {

            var MOBILEOTP = HttpContext.Session.GetString("MOBILEOTP");
            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFM"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 15)
            {
                mobno.message = "OTP Expired";
            }
            else
            {
                if (mobno.MOBILEOTP == MOBILEOTP)
                {
                    mobno.message = "Success";
                }
                else
                {
                    mobno.message = "Fail";
                }
            }


            HttpContext.Session.SetString("MobileOTPStatus", mobno.message);

            return mobno.message;
        }

        [Route("getOTPForEmail")]
        public async Task<string> getOTPForEmail([FromBody] LoginDTO mobno)
        {
            ApplicationUser user = new ApplicationUser();
            user = await _UserManager.FindByNameAsync(mobno.UserName);
            if (user != null)
            {
                List<UserRoleWithInstituteDMO> Mi_id_list = new List<UserRoleWithInstituteDMO>();
                Mi_id_list = _context.UserRoleWithInstituteDMO.Where(d => d.Id == user.Id).ToList();
                if (Mi_id_list.Count() > 0)
                {
                    string genotp = "";
                    mobno.MI_ID = Mi_id_list.FirstOrDefault().MI_Id;
                    CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                    genotp = generate.getOTP();
                    HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                    if (genotp.Length < 6)
                    {
                        genotp = generate.getOTP();
                    }
                    else
                    {
                        HttpContext.Session.SetString("EMAILOTP", genotp);
                        Email Email = new Email(_context);
                        string s = Email.sendmail(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp));
                        mobno.message = "Please Check Your Email,OTP Is Sent To Your Email Id";
                    }
                }
            }
            return mobno.message;
        }

        [Route("ForgotOTPForEmail")]
        public async Task<LoginDTO> ForgotOTPForEmail([FromBody] LoginDTO mobno)
        {
            // byte[] arr = {83,49,52,55,54,53,0,0,0,0,0,0,0,0,0,0,144};

            //  string s1= byteArrayToString(arr,arr.Length);
            //  byte[] sr = Encoding.ASCII.GetBytes(s1);
            //   int n= byteToInt(arr,true);
            string genotp = "";

            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            int miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

            //CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            mobno.MI_ID = cmn.IVRM_MI_Id;
            //    mobno.schoolOrcollegearray = (_context.Institution.Select(a=>a.MI_Id == mobno.MI_ID)).ToArray();
            var schoolOrcollegearray = (from a in _context.Institution
                                        where (a.MI_Id == mobno.MI_ID)
                                        select new LoginDTO
                                        {
                                            MI_ID = a.MI_Id,
                                            MI_Name = a.MI_Name,
                                            MI_SchoolCollegeFlag = a.MI_SchoolCollegeFlag
                                        }).ToList();
            if (schoolOrcollegearray.Count > 0)
            {
                if (schoolOrcollegearray.FirstOrDefault().MI_SchoolCollegeFlag != "S")
                {
                    if (mobno.clickedlinkname.Equals("forgotname"))
                    {

                        if (mobno.usertype.Equals("Student"))
                        {
                            mobno.userlist = (from t in _context.Adm_Master_College_StudentDMO
                                              where (t.MI_Id == cmn.IVRM_MI_Id && t.AMCST_ActiveFlag == true && t.AMCST_SOL == "S" && t.AMCST_emailId.Equals(mobno.Email))
                                              select new LoginDTO
                                              {
                                                  usernametype = ((t.AMCST_FirstName == null ? "" : t.AMCST_FirstName.ToUpper()) + " " + (t.AMCST_MiddleName == null ? "" : t.AMCST_MiddleName.ToUpper()) + " " + (t.AMCST_LastName == null ? "" : t.AMCST_LastName.ToUpper())).Trim(),
                                                  AMST_Id = t.AMCST_Id
                                              }
                          ).ToArray();

                            List<Adm_Master_College_StudentDMO> allstudents = new List<Adm_Master_College_StudentDMO>();
                            allstudents = _context.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == cmn.IVRM_MI_Id && t.AMCST_ActiveFlag == true && t.AMCST_SOL == "S" && t.AMCST_emailId.Equals(mobno.Email)).ToList();

                            if (allstudents.Count() > 1)
                            {
                                return mobno;
                            }
                            else
                            {
                                if (allstudents.Count() == 1)
                                {
                                    List<CollegeStudentlogin> Studentapplogin = new List<CollegeStudentlogin>();
                                    Studentapplogin = _context.CollegeStudentlogin.Where(t => t.AMCST_Id == allstudents.FirstOrDefault().AMCST_Id).ToList();
                                    if (Studentapplogin.Count() > 0)
                                    {
                                        ApplicationUser user = new ApplicationUser();
                                        user = await _UserManager.FindByIdAsync(Studentapplogin.FirstOrDefault().IVRMUL_Id.ToString());
                                        if (user != null)
                                        {
                                            var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                                            if (ismailConfirmed == true)
                                            {
                                                await _UserManager.RemovePasswordAsync(user);
                                                await _UserManager.AddPasswordAsync(user, "Password@123");
                                                user.UpdatedDate = DateTime.Now;
                                                var result = await _UserManager.UpdateAsync(user);
                                                if (result.Succeeded)
                                                {
                                                    Email Email = new Email(_context);
                                                    string s = Email.sendmail(allstudents.FirstOrDefault().MI_Id, allstudents.FirstOrDefault().AMCST_emailId, "StaffUserCreation", user.Id);
                                                    if (s.Equals("success"))
                                                    {
                                                        mobno.message = "Please Check Your Email,User Name and Password is sent";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            mobno.message = "Entered Email address is Not Exists!!";
                                        }
                                    }
                                    else
                                    {
                                        mobno.message = "Username is Not Exists for this student!!";
                                    }
                                }
                                else
                                {
                                    mobno.message = "Username is Not Exists for this Email!!";
                                }
                            }

                        }
                        else if (mobno.usertype.Equals("Staff"))
                        {

                        }
                        else
                        {
                            ApplicationUser user = new ApplicationUser();
                            user = await _UserManager.FindByEmailAsync(mobno.Email);
                            if (user != null)
                            {
                                var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                                if (ismailConfirmed == true)
                                {
                                    await _UserManager.RemovePasswordAsync(user);
                                    await _UserManager.AddPasswordAsync(user, "Password@123");
                                    user.UpdatedDate = DateTime.Now;
                                    var result = await _UserManager.UpdateAsync(user);
                                    if (result.Succeeded)
                                    {
                                        Email Email = new Email(_context);
                                        string s = Email.sendmail(mobno.MI_ID, mobno.Email, "StaffUserCreation", user.Id);
                                        if (s.Equals("success"))
                                        {
                                            mobno.message = "Please Check Your Email,User Name and Password is sent";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                mobno.message = "Entered Email address is Not Exists!!";
                            }
                        }



                        return mobno;

                    }
                    else if (mobno.clickedlinkname.Equals("forgotpwd"))
                    {
                        HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                        CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                        genotp = generate.getOTP();



                        if (genotp.Length < 6)
                        {
                            genotp = generate.getOTP();
                        }
                        else
                        {
                            HttpContext.Session.SetString("EMAILOTP", genotp);
                            Email Email = new Email(_context);

                            string s = Email.sendmail(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp));
                            mobno.message = "Please Check Your Email,OTP Is Sent To Your Email Id";
                        }
                    }
                }
                else if (schoolOrcollegearray.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    if (mobno.clickedlinkname.Equals("forgotname"))
                    {

                        if (mobno.usertype.Equals("Student"))
                        {
                            mobno.userlist = (from t in _context.Adm_M_Student
                                              where (t.MI_Id == cmn.IVRM_MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S" && t.AMST_emailId.Equals(mobno.Email))
                                              select new LoginDTO
                                              {
                                                  usernametype = ((t.AMST_FirstName == null ? "" : t.AMST_FirstName.ToUpper()) + " " + (t.AMST_MiddleName == null ? "" : t.AMST_MiddleName.ToUpper()) + " " + (t.AMST_LastName == null ? "" : t.AMST_LastName.ToUpper())).Trim(),
                                                  AMST_Id = t.AMST_Id
                                              }
                          ).ToArray();

                            List<Adm_M_Student> allstudents = new List<Adm_M_Student>();
                            allstudents = _context.Adm_M_Student.Where(t => t.MI_Id == cmn.IVRM_MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S" && t.AMST_emailId.Equals(mobno.Email)).ToList();

                            if (allstudents.Count() > 1)
                            {
                                return mobno;
                            }
                            else
                            {
                                if (allstudents.Count() == 1)
                                {
                                    List<StudentAppUserLoginDMO> Studentapplogin = new List<StudentAppUserLoginDMO>();
                                    Studentapplogin = _context.StudentAppUserLoginDMO.Where(t => t.AMST_ID == allstudents.FirstOrDefault().AMST_Id).ToList();
                                    if (Studentapplogin.Count() > 0)
                                    {
                                        ApplicationUser user = new ApplicationUser();
                                        user = await _UserManager.FindByIdAsync(Studentapplogin.FirstOrDefault().STD_APP_ID.ToString());
                                        if (user != null)
                                        {
                                            var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                                            if (ismailConfirmed == true)
                                            {
                                                await _UserManager.RemovePasswordAsync(user);
                                                await _UserManager.AddPasswordAsync(user, "Password@123");
                                                user.UpdatedDate = DateTime.Now;
                                                var result = await _UserManager.UpdateAsync(user);
                                                if (result.Succeeded)
                                                {
                                                    Email Email = new Email(_context);
                                                    string s = Email.sendmail(allstudents.FirstOrDefault().MI_Id, allstudents.FirstOrDefault().AMST_emailId, "StaffUserCreation", user.Id);
                                                    if (s.Equals("success"))
                                                    {
                                                        mobno.message = "Please Check Your Email,User Name and Password is sent";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            mobno.message = "Entered Email address is Not Exists!!";
                                        }
                                    }
                                    else
                                    {
                                        mobno.message = "Username is Not Exists for this student!!";
                                    }
                                }
                                else
                                {
                                    mobno.message = "Username is Not Exists for this Email!!";
                                }
                            }

                        }
                        else if (mobno.usertype.Equals("Staff"))
                        {

                        }
                        else
                        {
                            ApplicationUser user = new ApplicationUser();
                            user = await _UserManager.FindByEmailAsync(mobno.Email);
                            if (user != null)
                            {
                                var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                                if (ismailConfirmed == true)
                                {
                                    await _UserManager.RemovePasswordAsync(user);
                                    await _UserManager.AddPasswordAsync(user, "Password@123");
                                    user.UpdatedDate = DateTime.Now;
                                    var result = await _UserManager.UpdateAsync(user);
                                    if (result.Succeeded)
                                    {
                                        Email Email = new Email(_context);
                                        string s = Email.sendmail(mobno.MI_ID, mobno.Email, "StaffUserCreation", user.Id);
                                        if (s.Equals("success"))
                                        {
                                            mobno.message = "Please Check Your Email,User Name and Password is sent";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                mobno.message = "Entered Email address is Not Exists!!";
                            }
                        }



                        return mobno;

                    }
                    else if (mobno.clickedlinkname.Equals("forgotpwd"))
                    {
                        HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());

                        //HttpContext.Session.SetInt32("Session_MO_Id", cmnds.IVRM_MO_Id);
                        //HttpContext.Session.SetInt32("Session_MI_Id", cmnds.IVRM_MI_Id);

                        // mobno.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

                        CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                        genotp = generate.getOTP();



                        if (genotp.Length < 6)
                        {
                            genotp = generate.getOTP();
                        }
                        else
                        {
                            HttpContext.Session.SetString("EMAILOTP", genotp);
                            Email Email = new Email(_context);

                            string s = Email.sendmail(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp));

                            //var message = new SendGrid.SendGridMessage();
                            //    message.From = new MailAddress("vapstech123@gmail.com", "vaps@123");
                            //    message.Subject = "E-mail Verification";

                            //    message.AddTo(mobno.Email);

                            //    message.Html = "<p>Dear User, Your OTP Number is " + Convert.ToInt64(genotp) + ",Thank You.</p>";
                            //    var client = new Web("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");
                            //    client.DeliverAsync(message).Wait();
                            mobno.message = "Please Check Your Email,OTP Is Sent To Your Email Id";
                        }
                    }
                }

            }





            return mobno;

        }

        [Route("ForgotEmailstudent")]
        public async Task<LoginDTO> ForgotEmailstudent([FromBody] LoginDTO mobno)
        {
            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
            int miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

            //CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            mobno.MI_ID = cmn.IVRM_MI_Id;

            var schoolOrcollegearray = (from a in _context.Institution
                                        where (a.MI_Id == mobno.MI_ID)
                                        select new LoginDTO
                                        {
                                            MI_ID = a.MI_Id,
                                            MI_Name = a.MI_Name,
                                            MI_SchoolCollegeFlag = a.MI_SchoolCollegeFlag
                                        }).ToList();
            if (schoolOrcollegearray.Count > 0)
            {
                if (schoolOrcollegearray.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    List<Adm_M_Student> allstudents = new List<Adm_M_Student>();
                    allstudents = _context.Adm_M_Student.Where(t => t.AMST_Id == mobno.AMST_Id).ToList();


                    if (allstudents.Count() > 0)
                    {
                        List<StudentAppUserLoginDMO> Studentapplogin = new List<StudentAppUserLoginDMO>();
                        Studentapplogin = _context.StudentAppUserLoginDMO.Where(t => t.AMST_ID == mobno.AMST_Id).ToList();
                        if (Studentapplogin.Count() > 0)
                        {
                            ApplicationUser user = new ApplicationUser();
                            user = await _UserManager.FindByIdAsync(Studentapplogin.FirstOrDefault().STD_APP_ID.ToString());
                            if (user != null)
                            {
                                var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                                if (ismailConfirmed == true)
                                {
                                    await _UserManager.RemovePasswordAsync(user);
                                    await _UserManager.AddPasswordAsync(user, "Password@123");
                                    user.UpdatedDate = DateTime.Now;
                                    var result = await _UserManager.UpdateAsync(user);
                                    if (result.Succeeded)
                                    {
                                        Email Email = new Email(_context);
                                        string s = Email.sendmail(allstudents.FirstOrDefault().MI_Id, allstudents.FirstOrDefault().AMST_emailId, "StaffUserCreation", user.Id);
                                        if (s.Equals("success"))
                                        {
                                            mobno.message = "Please Check Your Email,User Name and Password is sent";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                mobno.message = "Entered Email address is Not Exists!!";
                            }
                        }
                        else
                        {
                            mobno.message = "Username is Not Exists for this student!!";
                        }
                    }
                    else
                    {
                        mobno.message = "Username is Not Exists for this Email!!";
                    }
                }
                else if (schoolOrcollegearray.FirstOrDefault().MI_SchoolCollegeFlag != "S")
                {
                    List<Adm_Master_College_StudentDMO> allstudents = new List<Adm_Master_College_StudentDMO>();
                    allstudents = _context.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == mobno.AMST_Id).ToList();


                    if (allstudents.Count() > 0)
                    {
                        List<CollegeStudentlogin> Studentapplogin = new List<CollegeStudentlogin>();
                        Studentapplogin = _context.CollegeStudentlogin.Where(t => t.AMCST_Id == mobno.AMST_Id).ToList();
                        if (Studentapplogin.Count() > 0)
                        {
                            ApplicationUser user = new ApplicationUser();
                            user = await _UserManager.FindByIdAsync(Studentapplogin.FirstOrDefault().IVRMUL_Id.ToString());
                            if (user != null)
                            {
                                var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                                if (ismailConfirmed == true)
                                {
                                    await _UserManager.RemovePasswordAsync(user);
                                    await _UserManager.AddPasswordAsync(user, "Password@123");
                                    user.UpdatedDate = DateTime.Now;
                                    var result = await _UserManager.UpdateAsync(user);
                                    if (result.Succeeded)
                                    {
                                        Email Email = new Email(_context);
                                        string s = Email.sendmail(allstudents.FirstOrDefault().MI_Id, allstudents.FirstOrDefault().AMCST_emailId, "StaffUserCreation", user.Id);
                                        if (s.Equals("success"))
                                        {
                                            mobno.message = "Please Check Your Email,User Name and Password is sent";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                mobno.message = "Entered Email address is Not Exists!!";
                            }
                        }
                        else
                        {
                            mobno.message = "Username is Not Exists for this student!!";
                        }
                    }
                    else
                    {
                        mobno.message = "Username is Not Exists for this Email!!";
                    }
                }
            }

          

            return mobno;

        }

        [Route("Userupdate")]
        public async Task<string> Userupdate([FromBody] LoginDTO mobno)
        {

            string genotp = "";

            if (mobno.clickedlinkname.Equals("M"))
            {
                HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();
                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {

                    HttpContext.Session.SetString("EMAILOTP", genotp);

                    SMS sms = new SMS(_context);

                    string s = await sms.sendSms(mobno.MI_ID, Convert.ToInt64(mobno.Mobile), "EMAILOTP", Convert.ToInt64(genotp));


                    mobno.message = "Please Check,OTP Is Sent To Your Mobile Number";
                }
            }

            return mobno.message;

        }

        [Route("ForgotOTPForEmailval")]
        public async Task<string> ForgotOTPForEmailval([FromBody] LoginDTO mobno)
        {

            string genotp = "";

            //int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            //CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

            //CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            //mobno.MI_ID = cmnds.IVRM_MI_Id;

            mobno.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (mobno.clickedlinkname.Equals("M"))
            {
                HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();
                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {

                    HttpContext.Session.SetString("EMAILOTP", genotp);

                    SMS sms = new SMS(_context);

                    string s = await sms.sendSms(mobno.MI_ID, Convert.ToInt64(mobno.Mobile), "EMAILOTP", Convert.ToInt64(genotp));


                    mobno.message = "Please Check,OTP Is Sent To Your Mobile Number";
                }
            }
            else if (mobno.clickedlinkname.Equals("E"))
            {
                HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();
                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {
                    HttpContext.Session.SetString("EMAILOTP", genotp);
                    Email Email = new Email(_context);

                    string s = Email.sendmail(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp));


                    mobno.message = "Please Check,OTP Is Sent To Your Email Id";
                }
            }



            return mobno.message;

        }

        [Route("VerifyEmailOtp")]
        public string VerifyEmailOtp([FromBody] LoginDTO mobno)
        {
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");

            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFE"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 15)
            {
                mobno.message = "OTP Expired";
            }
            else
            {
                if (mobno.EMAILOTP == EMAILOTP)
                {
                    mobno.message = "Success";
                }
                else
                {
                    mobno.message = "Fail";
                }
            }


            HttpContext.Session.SetString("EmailOTPStatus", mobno.message);

            return mobno.message;
        }

        [Route("VerifyEmailOtpgen")]
        public string VerifyEmailOtpgen([FromBody] LoginDTO mobno)
        {
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");

            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFE"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 30)
            {
                mobno.message = "OTP Expired";
            }
            else
            {
                if (mobno.EMAILOTP == EMAILOTP)
                {
                    mobno.message = "Success";
                }
                else
                {
                    mobno.message = "Fail";
                }
            }


            HttpContext.Session.SetString("EmailOTPStatus", mobno.message);

            return mobno.message;
        }

        [Route("getOTP")]
        public async Task<string> getOTP([FromBody] LoginDTO mobno)
        {
            string genotp = "";

            mobno = pre.forgotPassword(mobno);

            if (mobno.message == "Success")
            {
                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();

                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {

                    HttpContext.Session.SetString("OTP", genotp);
                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://trans.kapsystem.com/api/web2sms.php?&workingkey=Af7660f900c9df85ac5dfae4b055d75bd&sender=HHSJCP&to=" + mobno.mobileNo + "&message=Your OTP is " + genotp) as HttpWebRequest;
                    //optional
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                    mobno.message = "OTP is sent to your Mobile Number";
                }
            }
            else
            {
                mobno.message = "Mobile Number does not match with the entered UserName";
            }

            return mobno.message;
        }

        [Route("getDataMyInstitute/{id:int}")]
        public LoginDTO getDataMyInstitute(int id)
        {
            HttpContext.Session.SetInt32("Session_MI_Id", Convert.ToInt32(id));

            LoginDTO dto = new LoginDTO();
            dto = pre.getInstituteDataByRoleTypeName(id);

            if (dto != null)
            {
                // Added on 24-Mar-2017
                HttpContext.Session.SetInt32("RoleId", Convert.ToInt32(dto.roleId));

                // HttpContext.Session.SetInt32("UserId", Convert.ToInt32(dto.userId));

                //  HttpContext.Session.SetInt32("Role", Convert.ToInt32(dto.role));

                HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(dto.ASMAY_Id));
            }

            return dto;
        }

        [Route("getrolewisepage")]
        public LoginDTO getrolpagedata([FromBody] LoginDTO mobno)
        {
            //Removing Session value when user logs in.
            //Added on 05-03-2018
            HttpContext.Session.Remove("Session_Trip_IsOnline");
            HttpContext.Session.Remove("Session_Trip_MI_Id");
            HttpContext.Session.Remove("Session_Trip_ASMAY_Id");
            mobno.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mobno.RoleName = HttpContext.Session.GetString("RoleNme");
            mobno.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            if (mobno.userId > 0)
            {
                mobno = pre.getrolpagewisedata(mobno);
            }
            else
            {
                mobno.returnMsg = "Expired";
            }

            return mobno;
        }

        [Route("ForgotOTPForEmailval_new")]
        public async Task<string> ForgotOTPForEmailval_new([FromBody] LoginDTO mobno)
        {
            try
            {
                SMS sms = new SMS(_context);
                long trnsno = 0;

                string genotp = "";

                mobno.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

                //int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

                //CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

                //CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                //mobno.MI_ID = cmnds.IVRM_MI_Id;

                mobno.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

                trnsno = sms.getsmsno(mobno.MI_ID);

                if (mobno.clickedlinkname.Equals("M"))
                {
                    HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                    CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                    genotp = generate.getOTP();
                    if (genotp.Length < 6)
                    {
                        genotp = generate.getOTP();
                    }
                    else
                    {
                        HttpContext.Session.SetString("EMAILOTP", genotp);

                        string s = await sms.sendSmsnewTemplet_new(mobno.MI_ID, Convert.ToInt64(mobno.Mobile), "EMAILOTP", mobno.userId, trnsno, "", Convert.ToInt64(genotp));

                        mobno.message = "Please Check,OTP Is Sent To Your Mobile Number";
                    }
                }

                else if (mobno.clickedlinkname.Equals("E"))
                {
                    HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                    CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                    genotp = generate.getOTP();
                    if (genotp.Length < 6)
                    {
                        genotp = generate.getOTP();
                    }
                    else
                    {
                        HttpContext.Session.SetString("EMAILOTP", genotp);

                        Email Email = new Email(_context);

                        string s = Email.sendmail(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp));

                        mobno.message = "Please Check,OTP Is Sent To Your Email Id";
                    }
                }
            }
            catch (Exception ex)
            {
                mobno.message = "Something Went Wrong Please Contact Administrator";
                Console.WriteLine(ex.Message);
            }

            return mobno.message;
        }

        [Route("checkpagemap/")]
        public LoginDTO checkpagemap([FromBody]LoginDTO data)
        {
            try
            {

                data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                var rolelist = _context.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleId).ToList();

                string[] arr = { };
                arr = data.message.Split("/");

                var pagename = arr[5];


                var checkdashbord = (from a in _context.Dashboard_page_mapping
                                     from b in _context.masterpage
                                     where a.MI_ID == data.MI_ID && a.IVRMP_Dasboard_PageName.Trim() == b.IVRMP_PageURL.Trim() && b.IVRMP_BrowerUrl.Trim() == pagename.Trim() && a.IVRMRT_Role.ToLower().Trim() == rolelist[0].IVRMRT_Role.ToLower().Trim()
                                     select b

                                   ).ToList();

                if (checkdashbord.Count > 0)
                {
                    data.usertype = "A";
                }
                else

                if (pagename == "AccessDenied")
                {
                    data.usertype = "A";
                }
                //  pagename = "/" + pagename + "/";
                else
                {
                    if (rolelist[0].IVRMRT_Role.Equals("Super Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        var pageList = (from page in _context.masterpage
                                        from institutionwisemenupagemapp in _context.Mastermenupagemappinginstitutewise
                                        from institutionModule in _context.Institution_Module
                                        from instModulePage in _context.Institution_Module_Page
                                        from institutionroleprevlg in _context.InstitutionRolePrivileges
                                        from institutewisemastermenu in _context.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == data.MI_ID && institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == data.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true && institutewisemastermenu.MI_Id == data.MI_ID && page.IVRMP_BrowerUrl == pagename.Trim()
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,

                                        }
                                        ).Distinct().OrderBy(t => t.IVRMIMP_PageOrder).ToList();
                        data.pageList = pageList.ToArray();
                        if (pageList.Count > 0)
                        {
                            data.usertype = "A";
                        }

                    }

                    else if (rolelist[0].IVRMRT_Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {

                        var pageList = (from page in _context.masterpage
                                        from institutionwisemenupagemapp in _context.Mastermenupagemappinginstitutewise
                                        from institutionModule in _context.Institution_Module
                                        from instModulePage in _context.Institution_Module_Page
                                        from institutionroleprevlg in _context.InstitutionRolePrivileges
                                        from institutewisemastermenu in _context.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == data.MI_ID &&
                                        institutionModule.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == data.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true && page.IVRMP_BrowerUrl == pagename.Trim()
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,

                                        }
                                   ).Distinct().ToList();
                        data.pageList = pageList.ToArray();

                        if (pageList.Count > 0)
                        {
                            data.usertype = "A";
                        }
                        //var privileges = (from institutionRolePrivileges in _context.InstitutionRolePrivileges
                        //                  from institutionModulePage in _context.Institution_Module_Page
                        //                  from intmodule in _context.Institution_Module
                        //                  where (institutionRolePrivileges.IVRMIMP_Id == institutionModulePage.IVRMIMP_Id && institutionModulePage.IVRMIM_Id == intmodule.IVRMIM_Id && intmodule.MI_Id == cmn.IVRM_MI_Id && institutionRolePrivileges.IVRMRT_Id == cmn.roleId)
                        //                  select new LoginDTO
                        //                  {
                        //                      pageId = institutionModulePage.IVRMP_Id,
                        //                      IVRMIRP_AddFlag = institutionRolePrivileges.IVRMIRP_AddFlag,
                        //                      IVRMIRP_DeleteFlag = institutionRolePrivileges.IVRMIRP_DeleteFlag,
                        //                      IVRMIRP_UpdateFlag = institutionRolePrivileges.IVRMIRP_UpdateFlag,
                        //                      IVRMIRP_SearchFlag = institutionRolePrivileges.IVRMIRP_SearchFlag,
                        //                      IVRMIRP_ReportFlag = institutionRolePrivileges.IVRMIRP_ReportFlag,
                        //                      IVRMIRP_ProcessFlag = institutionRolePrivileges.IVRMIRP_ProcessFlag
                        //                  }).ToList();
                        //lgData.privileges = privileges.ToArray();





                    }
                    else if (rolelist[0].IVRMRT_Role.Equals("PARENTS", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("PARENT", StringComparison.OrdinalIgnoreCase))
                    {
                        var pageList = (from page in _context.masterpage
                                        from institutionwisemenupagemapp in _context.Mastermenupagemappinginstitutewise
                                        from institutionModule in _context.Institution_Module
                                        from instModulePage in _context.Institution_Module_Page
                                        from institutionroleprevlg in _context.InstitutionRolePrivileges
                                        from institutewisemastermenu in _context.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == data.MI_ID &&
                                        institutionModule.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == data.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true && page.IVRMP_BrowerUrl == pagename.Trim()
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,

                                        }
                                   ).Distinct().ToList();
                        data.pageList = pageList.ToArray();
                        if (pageList.Count > 0)
                        {
                            data.usertype = "A";
                        }


                    }
                    else if (rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("ALumni", StringComparison.OrdinalIgnoreCase))
                    {
                        var pageList = (from page in _context.masterpage
                                        from institutionwisemenupagemapp in _context.Mastermenupagemappinginstitutewise
                                        from institutionModule in _context.Institution_Module
                                        from instModulePage in _context.Institution_Module_Page
                                        from institutionroleprevlg in _context.InstitutionRolePrivileges
                                        from institutewisemastermenu in _context.Mastermenuinstitutewise
                                        where (institutionModule.MI_Id == data.MI_ID &&
                                        institutionModule.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == data.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true && page.IVRMP_BrowerUrl == pagename.Trim()
                                        )
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,

                                        }
                                    ).Distinct().OrderBy(t => t.IVRMIMP_PageOrder).ToList();
                        data.pageList = pageList.ToArray();
                        if (pageList.Count > 0)
                        {
                            data.usertype = "A";
                        }

                    }
                    else
                    {
                        var pageList = (from page in _context.masterpage
                                        from institutionwisemenupagemapp in _context.Mastermenupagemappinginstitutewise
                                        from institutionModule in _context.Institution_Module
                                        from instModulePage in _context.Institution_Module_Page
                                        from institutionroleprevlg in _context.InstitutionRolePrivileges
                                        from institutewisemastermenu in _context.Mastermenuinstitutewise
                                        from staffloginpreviledge in _context.UserLoginPrivileges
                                        where (institutionModule.MI_Id == data.MI_ID && staffloginpreviledge.IVRMIMP_Id == instModulePage.IVRMIMP_Id &&
                                        staffloginpreviledge.MI_Id == institutionModule.MI_Id &&
                                                institutionModule.IVRMIM_Id == instModulePage.IVRMIM_Id &&
                                                instModulePage.IVRMIMP_Id == institutionroleprevlg.IVRMIMP_Id &&
                                                institutionroleprevlg.IVRMRT_Id == data.roleId &&
                                                page.IVRMP_Id == instModulePage.IVRMP_Id &&
                                                page.IVRMP_Id == institutionwisemenupagemapp.IVRMP_Id &&
                                               institutewisemastermenu.IVRMMMI_Id == institutionwisemenupagemapp.IVRMMMI_Id &&
                                               page.IVRMP_TemplateFlag == true && staffloginpreviledge.Id == data.userId && page.IVRMP_BrowerUrl == pagename.Trim())
                                        select new Institution_Module_PageDTO
                                        {
                                            pageId = page.IVRMP_Id,
                                            IVRMMP_PageName = institutionwisemenupagemapp.IVRMMMPMI_PageDisplayName,
                                        }
                                ).Distinct().OrderBy(t => t.IVRMIMP_PageOrder).ToList();
                        data.pageList = pageList.ToArray();

                        if (pageList.Count > 0)
                        {
                            data.usertype = "A";
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return data;
        }

        [Route("getgateway")]
        public FileDescriptionDTO getgateway([FromBody] FileDescriptionDTO dto)//[FromBody]
        {
            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            CommonDTO cmnds = new CommonDTO();

            if (vr_id != 0)
            {
                cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                dto.MI_Id = cmnds.IVRM_MI_Id;
            }

            return pre.getgateway(dto);

        }

        [Route("GetpaymentDetails")]
        public FileDescriptionDTO GetpaymentDetails([FromBody] FileDescriptionDTO dto)//[FromBody]
        {
            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            CommonDTO cmnds = new CommonDTO();

            if (vr_id != 0)
            {
                cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                dto.MI_Id = cmnds.IVRM_MI_Id;
            }

            return pre.GetpaymentDetails(dto);

        }

        [Route("paymentsave")]
        public FileDescriptionDTO paymentsave([FromBody] FileDescriptionDTO dto)//[FromBody]
        {
            FileDescriptionDTO reg = new FileDescriptionDTO();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            List<MasterAcademic> allyear = new List<MasterAcademic>();
            List<MasterAcademic> allyearget = new List<MasterAcademic>();

            var EMAILOTPSTATUS = HttpContext.Session.GetString("EmailOTPStatus");

            var MOBILEOTPSTATUS = HttpContext.Session.GetString("MobileOTPStatus");

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;



            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            CommonDTO cmnds = new CommonDTO();

            if (vr_id != 0)
            {
                cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                reg.MI_Id = cmnds.IVRM_MI_Id;
            }
            else
            {
                reg.MI_Id = Convert.ToInt64(dto.Intid);
            }

            //CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

            allyearget = (from a in _context.AcademicYear
                          where (a.MI_Id == reg.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == reg.MI_Id)
                          select new MasterAcademic
                          {
                              ASMAY_Id = a.ASMAY_Id,
                              ASMAY_Year = a.ASMAY_Year
                          }
                        ).ToList();

            allyear = (from a in _context.AcademicYear
                       where (a.MI_Id == reg.MI_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                       select new MasterAcademic
                       {
                           ASMAY_Id = a.ASMAY_Id,
                           ASMAY_Year = a.ASMAY_Year,
                           ASMAY_Order = a.ASMAY_Order
                       }
                     ).OrderByDescending(d => d.ASMAY_Order).ToList();
            if (allyear.Count > 0 || dto.Alumni == "Alumni" || dto.Special == "Special")
            {

                int asmayid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
                reg.ASMAY_Id = dto.ASMAY_Id;

                // reg.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
                reg.Entry_Date = DateTime.Now;
                reg.Machine_Ip_Address = remoteIpAddress.ToString();
                reg.Email_id = dto.Email_id;
                reg.Mobileno = dto.Mobileno;
                reg.Password = dto.Password;
                reg.Username = dto.Username;
                reg.admission = dto.admission;
                reg.admittedyer = dto.admittedyer;
                reg.leftclassid = dto.leftclassid;
                reg.classidadmitted = dto.classidadmitted;
                reg.leftyear = dto.leftyear;
                reg.Name = dto.Name;
                reg.Special = dto.Special;
                reg.CreatedDate = DateTime.Now;
                reg.UpdatedDate = DateTime.Now;
                reg.Alumni = dto.Alumni;
                reg.courseadmittted = dto.courseadmittted;
                reg.courseleft = dto.courseleft;
                reg.branchadmittted = dto.branchadmittted;
                reg.branchleft = dto.branchleft;
                reg.semadmittted = dto.semadmittted;
                reg.semleft = dto.semleft;

                reg.ReceiptNo = dto.ReceiptNo;
                reg.orderId = dto.orderId;
                reg.Donation_Amount = dto.Donation_Amount;
                reg.paygtw = dto.paygtw;


                if (dto.onlinepreadminfilepath != null)
                {
                    reg.profile_pic_path = dto.onlinepreadminfilepath;
                }
                else
                {
                    reg.profile_pic_path = "images\\uploads\\Profile_Pics\\a_1c6e77cd-1ba4-45eb-959a-19b80a5c0dfc.jpg";
                }

                reg = registr.alu_regdata(reg);
                if (reg.returnMsg == "created")
                {
                    string confirmationLink = Url.Action("ConfirmEmail", "Login", new
                    {
                        userid = reg.User_Id,
                        token = reg.token
                    }, protocol: HttpContext.Request.Scheme);
                    reg.confirmationLink = confirmationLink;

                    var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _context.mstConfig.Where(d => d.MI_Id.Equals(reg.MI_Id) && d.ASMAY_Id.Equals(Acdemic_preadmission)).ToList();


                    if (mstConfig.FirstOrDefault().ISPAC_RegMailFlag == 1 && (dto.Special == "" || dto.Special == null) && dto.Alumni != "Alumni")
                    {
                        Email Email = new Email(_context);
                        string s = Email.sendmailreg(reg.MI_Id, dto.Email_id, "REG", reg.User_Id, reg.Password);
                    }

                    if (mstConfig.FirstOrDefault().ISPAC_RegSMSFlag == 1 && (dto.Special == "" || dto.Special == null) && dto.Alumni != "Alumni")
                    {
                        SMS sms = new SMS(_context);
                        string sm = sms.sendSmsreg(reg.MI_Id, Convert.ToInt64(dto.Mobileno), "REG", reg.User_Id, reg.Password).Result;
                    }


                    if (dto.Special == "Special")
                    {
                        Email Email = new Email(_context);
                        string s = Email.sendmail(reg.MI_Id, dto.Email_id, "SPECIALREG", reg.User_Id);

                        SMS sms = new SMS(_context);
                        string sm = sms.sendSms(reg.MI_Id, Convert.ToInt64(dto.Mobileno), "SPECIALREG", reg.User_Id).Result;
                    }

                    if (dto.Alumni == "Alumni")
                    {
                        SMS sms = new SMS(_context);
                        string sm = sms.sendSmsreg(reg.MI_Id, Convert.ToInt64(dto.Mobileno), "ALUMNIREG", reg.User_Id, reg.Password).Result;

                        Email Email = new Email(_context);
                        string s = Email.sendmailreg(reg.MI_Id, dto.Email_id, "ALUMNIREG", reg.User_Id, reg.Password);
                    }


                    dto.returnval = "true";


                }
                else
                {

                }

            }
            else
            {
                reg.returnMsg = "No";
            }
            return dto;


        }


        [Route("SaveRemaindersRemarks")]
        public LoginDTO SaveRemaindersRemarks([FromBody]LoginDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            HttpContext.Session.SetInt32("PaymentNootificationGeneral", 1);
            return pre.SaveRemaindersRemarks(data);
        }
    }
}
