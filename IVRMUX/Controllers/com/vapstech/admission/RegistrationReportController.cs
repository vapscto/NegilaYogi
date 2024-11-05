using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class RegistrationReportController : Controller
    {

        // GET: /<controller>/
        RegistrationReportDelegates RegistrationReportDelegates = new RegistrationReportDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("get_intial_data/{id:int}")]
        public WrittenTestMarksBindDataDTO get_intial_data(int id)
        {
            WrittenTestMarksBindDataDTO data = new WrittenTestMarksBindDataDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return RegistrationReportDelegates.get_intial_data(data);


        }

        //[Route("Getdetails/{id:int}")]
        //public WrittenTestMarksBindDataDTO GetdetailsOLD(int ID)
        //{
        //    return TotalCountReportDelegates.GetData(ID);
        //}

        [HttpPost]
        [Route("Getdetails/")]
        public WrittenTestMarksBindDataDTO Getdetails([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            return RegistrationReportDelegates.GetData(MMD);
        }


        [Route("Getdetailsforpre/")]
        public WrittenTestMarksBindDataDTO Getdetailsforpre([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            MMD.MI_Id = mid;
            return RegistrationReportDelegates.Getdetailsforpre(MMD);
        }


        [Route("SendSms/")] 
        public string SendSms([FromBody] WrittenTestMarksBindDataDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return RegistrationReportDelegates.SendSms(MMD);
        }

        [Route("SendMail/")]
        public string SendMail([FromBody] WrittenTestMarksBindDataDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return RegistrationReportDelegates.SendMail(MMD);
        }

        [HttpPost]
        [Route("smssend")]
        public WrittenTestMarksBindDataDTO smssend([FromBody] WrittenTestMarksBindDataDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return RegistrationReportDelegates.smssend(data);
        }
        [HttpPost]
        [Route("avtivedeactive")]
        public WrittenTestMarksBindDataDTO avtivedeactive([FromBody] WrittenTestMarksBindDataDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return RegistrationReportDelegates.avtivedeactive(data);
        }


        [HttpPost]
        [Route("emailsend")]
        public WrittenTestMarksBindDataDTO emailsend([FromBody] WrittenTestMarksBindDataDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return RegistrationReportDelegates.emailsend(data);
        }



        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            return RegistrationReportDelegates.ExportToExcle(MMD);
        }

        [Route("searchprospectus/")]
        public WrittenTestMarksBindDataDTO searchprospectus([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            return RegistrationReportDelegates.searchprospectus(MMD);
        }
    }
}
