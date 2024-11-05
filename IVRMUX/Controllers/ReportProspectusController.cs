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

    public class ReportProspectusController : Controller
    {

        // GET: /<controller>/
        ReportProspectusDelegates ReportProspectusDelegates = new ReportProspectusDelegates();

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

            return ReportProspectusDelegates.get_intial_data(data);
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

            return ReportProspectusDelegates.GetData(MMD);
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

            return ReportProspectusDelegates.SendSms(MMD);
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

            return ReportProspectusDelegates.SendMail(MMD);
        }

        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            return ReportProspectusDelegates.ExportToExcle(MMD);
        }

        [Route("searchprospectus/")]
        public WrittenTestMarksBindDataDTO searchprospectus([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            return ReportProspectusDelegates.searchprospectus(MMD);
        }
    }
}
