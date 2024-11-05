using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class DailyFeeCollReportController : Controller
    {
        FeeDailyCollectionReportDelegate FGD = new FeeDailyCollectionReportDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public DailyCollectionReportDTO Get(int id)
        {
            DailyCollectionReportDTO dt = new DailyCollectionReportDTO();

            //id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dt.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.yearid = ASMAY_Id;

            return FGD.getdetails(dt);
        }
        // POST api/values


        [HttpPost]
        [Route("getdata")]
        public DailyCollectionReportDTO getdata([FromBody]DailyCollectionReportDTO data)
        {
            data.mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FGD.getdata(data);
        }



        [HttpPost]
        [Route("getgroupmappedheads")]
        public DailyCollectionReportDTO getgroupheaddetails([FromBody]DailyCollectionReportDTO data)
        {
           data.mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           //data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.getgroupheaddetails(data);
        }

        [HttpPost]
        [Route("getgroupheadsid")]
        public DailyCollectionReportDTO getgroupheadsid([FromBody]DailyCollectionReportDTO data)
        {

            return FGD.getgroupheadsid(data);
        }

        [HttpPost]
        [Route("Getreportdetails/")]
        public DailyCollectionReportDTO Getreportdetails([FromBody] DailyCollectionReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         // MMD.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return  FGD.Getreportdetails(MMD);
        }

        [HttpPost]
        [Route("FeeAccountDetailsReport")]
        public DailyCollectionReportDTO FeeAccountDetailsReport([FromBody] DailyCollectionReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.FeeAccountDetailsReport(MMD);
        }

        //UserWisereportdetails
        [HttpPost]
        [Route("UserWisereportdetails/")]
        public DailyCollectionReportDTO UserWisereportdetails([FromBody] DailyCollectionReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.UserWisereportdetails(MMD);
        }

        //Report VVVKS
        [HttpPost]
        [Route("getreport/")]
        public DailyCollectionReportDTO getreport([FromBody] DailyCollectionReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.getreport(MMD);
        }
    }
}
