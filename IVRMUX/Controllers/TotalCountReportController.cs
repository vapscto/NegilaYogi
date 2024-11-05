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

    public class TotalCountReportController : Controller
    {

        // GET: /<controller>/
        TotalCountReportDelegates TotalCountReportDelegates = new TotalCountReportDelegates();

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
            return TotalCountReportDelegates.get_intial_data(data);
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
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //MMD.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return TotalCountReportDelegates.GetData(MMD);
        }


        [Route("SendSms/")]
        public string SendSms([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return TotalCountReportDelegates.SendSms(MMD);
        }

        [Route("SendMail/")]
        public string SendMail([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return TotalCountReportDelegates.SendMail(MMD);
        }

        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            return TotalCountReportDelegates.ExportToExcle(MMD);
        }
    }
}
