using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Options;
using CommonLibrary;
using PreadmissionDTOs;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class CLGStudentBuspassFormController : Controller
    {
        CLGStudentBuspassFormDelegate ad = new CLGStudentBuspassFormDelegate();
        private readonly DomainModelMsSqlServerContext _context;

        public CLGStudentBuspassFormController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public CLGStudentBuspassFormDTO getloaddata(int id)
        {
            CLGStudentBuspassFormDTO data = new CLGStudentBuspassFormDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = id;
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ad.getloaddata(data);
        }

        [Route("getloaddataintruction/{id:int}")]
        public CLGStudentBuspassFormDTO getloaddataintruction(int id)
        {
            CLGStudentBuspassFormDTO data = new CLGStudentBuspassFormDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = id;
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            string accountname = "";
            string accesskey = "";
            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

            var datatstu = _context.IVRM_Storage_path_Details.ToList();
            if (datatstu.Count() > 0)
            {
                accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            }


            string html = obj.getHtmlContentFromAzure(accountname, accesskey, "busforminstrn/" + data.MI_Id, "Busform.html", 0);
            data.htmldata = html;
            return ad.getloaddataintruction(data);
        }
        [Route("getstudata")]
        public CLGStudentBuspassFormDTO getstudata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ad.getstudata(sddto);
        }
        [Route("getbuspassdata")]
        public CLGStudentBuspassFormDTO getbuspassdata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            string accountname = "";
            string accesskey = "";

            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

            var datatstu = _context.IVRM_Storage_path_Details.ToList();
            if (datatstu.Count() > 0)
            {
                accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            }


            string html = obj.getHtmlContentFromAzure(accountname, accesskey, "busform/" + sddto.MI_Id, "Busform.html", 0);
            sddto.htmldata = html;


            //string pathToHTMLFile = @"D:\IVRMCODE\SEPTEMBER\SEP23\jshs21092019\IVRMUX\wwwroot\clgbussform.html";
            //sddto.htmldata = System.IO.File.ReadAllText(pathToHTMLFile);
            return ad.getbuspassdata(sddto);
        }
        [Route("getroutedata")]
        public CLGStudentBuspassFormDTO getroutedata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getroutedata(sddto);
        }

        [Route("academicload")]
        public CLGStudentBuspassFormDTO academicload(CLGStudentBuspassFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return ad.academicload(data);
        }
        [Route("getlocationdata")]
        public CLGStudentBuspassFormDTO getlocationdata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getlocationdata(sddto);
        }
        [Route("getlocationdataonly")]
        public CLGStudentBuspassFormDTO getlocationdataonly([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getlocationdataonly(sddto);
        }

        [HttpPost]
        [Route("savedata")]
        public CLGStudentBuspassFormDTO savedata([FromBody]CLGStudentBuspassFormDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // student.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            student.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return ad.savedata(student);
        }


        [Route("paynow")]
        public CLGStudentBuspassFormDTO paynow([FromBody] CLGStudentBuspassFormDTO data)
        {
            CLGStudentBuspassFormDTO dt = new CLGStudentBuspassFormDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return ad.paynow(data);
        }
        [Route("paynow1")]
        public CLGStudentBuspassFormDTO paynow1([FromBody] CLGStudentBuspassFormDTO data)
        {
            CLGStudentBuspassFormDTO dt = new CLGStudentBuspassFormDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return ad.paynow1(data);
        }

        [Route("paynow2")]
        public CLGStudentBuspassFormDTO paynow2([FromBody] CLGStudentBuspassFormDTO data)
        {
            CLGStudentBuspassFormDTO dt = new CLGStudentBuspassFormDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return ad.paynow2(data);
        }


        [Route("Razorpaypaymentresponse/")]
        public ActionResult razorpaymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            response.IVRMOP_MIID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string payid = response.razorpay_payment_id;
            try
            {
                dto = ad.razorgetpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/StudentBuspassForm/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/StudentBuspassForm/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            var sub = "StudentBuspassForm";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();
            //dtoapp = getdashboardpage(dtoapp);
            //if (dtoapp.dashboardpage != null)
            //{
            //    sub = dtoapp.dashboardpage;
            //}
            //else
            //{
            //    sub = "hutchings";
            //}

            try
            {
                dto = ad.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/ 12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            return Redirect(querystring);
        }
    }
}
