using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.Extensions.Options;
using DataAccessMsSqlServerProvider;



using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Dynamic;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using CommonLibrary;



namespace corewebapi18072016.Controllers
{
  
    [Route("api/[controller]")]
    public class BuspassFormController : Controller
    {
        private static FacadeUrl _config;
        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        private FacadeUrl fdu = new FacadeUrl();
        private readonly DomainModelMsSqlServerContext _context;
     

        BuspassFormDelegate ad = new BuspassFormDelegate();

        public BuspassFormController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _config = settings.Value;
            new StudentApplicationDelegate(_config);
            fdu = _config; 
            _context = context;
        }

        [HttpGet]
        [Route("getloaddata")]
        public StudentHelthcertificateDTO getloaddata(StudentHelthcertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));


            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return ad.getloaddata(data);
        }
        [Route("getstudata")]
        public StudentHelthcertificateDTO getstudata([FromBody]StudentHelthcertificateDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return ad.getstudata(sddto);
        }
        [Route("getbuspassdata")]
        public StudentHelthcertificateDTO getbuspassdata([FromBody]StudentHelthcertificateDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getbuspassdata(sddto);
        }
        [Route("getroutedata")]
        public StudentHelthcertificateDTO getroutedata([FromBody]StudentHelthcertificateDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getroutedata(sddto);
        }
        [Route("getlocationdata")]
        public StudentHelthcertificateDTO getlocationdata([FromBody]StudentHelthcertificateDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getlocationdata(sddto);
        }
        [HttpPost]
        [Route("savedata")]
        public StudentHelthcertificateDTO savedata([FromBody]StudentHelthcertificateDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savedata(student);
        }

        [Route("paynow")]
        public StudentHelthcertificateDTO paynow([FromBody] StudentHelthcertificateDTO data)
        {
            StudentHelthcertificateDTO dt = new StudentHelthcertificateDTO();

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
            var sub = "Buspassform";
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
