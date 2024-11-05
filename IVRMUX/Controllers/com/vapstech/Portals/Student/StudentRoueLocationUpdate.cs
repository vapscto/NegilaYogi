using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Options;

namespace corewebapi18072016.Controllers
{
    //[ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentRoueLocationUpdateController : Controller
    {
        StudentRoueLocationUpdateDelegate ad = new StudentRoueLocationUpdateDelegate();
        private readonly DomainModelMsSqlServerContext _context;

        public StudentRoueLocationUpdateController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("getloaddata")]
        public StudentBuspassFormDTO getloaddata(StudentBuspassFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ad.getloaddata(data);
        }

        [Route("getloaddataintruction")]
        public StudentBuspassFormDTO getloaddataintruction (StudentBuspassFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
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
            return ad.getloaddataintruction (data);
        }
        [Route("getstudata")]
        public StudentBuspassFormDTO getstudata([FromBody]StudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));                 
            sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return ad.getstudata(sddto);
        }
        [Route("getbuspassdata")]
        public StudentBuspassFormDTO getbuspassdata([FromBody]StudentBuspassFormDTO sddto)
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
            return ad.getbuspassdata(sddto);
        }
        [Route("getbuspassdataupdate")]
        public StudentBuspassFormDTO getbuspassdataupdate([FromBody]StudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return ad.getbuspassdataupdate(sddto);
        }
        [Route("getroutedata")]
        public StudentBuspassFormDTO getroutedata([FromBody]StudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getroutedata(sddto);
        }
        [Route("getlocationdata")]
        public StudentBuspassFormDTO getlocationdata([FromBody]StudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getlocationdata(sddto);
        }
        [Route("getlocationdataonly")]
        public StudentBuspassFormDTO getlocationdataonly([FromBody]StudentBuspassFormDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getlocationdataonly(sddto);
        }

        [HttpPost]
        [Route("savedata")]
        public StudentBuspassFormDTO savedata([FromBody]StudentBuspassFormDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // student.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            student.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return ad.savedata(student);
        }


        [Route("paynow")]
        public StudentBuspassFormDTO paynow([FromBody] StudentBuspassFormDTO data)
        {
            StudentBuspassFormDTO dt = new StudentBuspassFormDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return ad.paynow(data);
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


        [HttpPost]
        [Route("searchfilter")]
        public StudentBuspassFormDTO searchfilter([FromBody]StudentBuspassFormDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.searchfilter(student);
        }



    }
}
