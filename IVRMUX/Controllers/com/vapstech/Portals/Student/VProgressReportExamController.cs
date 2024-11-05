using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using corewebapi18072016.Delegates.com.vapstech.Portals;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals
{
    [Route("api/[controller]")]
    public class VProgressReportExamController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        VProgressReportExamDelegates crStr = new VProgressReportExamDelegates();


        [Route("Getdetails")]
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return crStr.Getdetails(data);
        }


        [Route("savedetails")]
        public VikasaSubjectwiseCumulativeReportDTO savedetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return crStr.showdetails(data);
        }

        [Route("get_exam")]
        public VikasaSubjectwiseCumulativeReportDTO get_exam([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {
        
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.MI_Id = mid;
            return crStr.get_exam(data);
        }
        [Route("get_Category")]
        public VikasaSubjectwiseCumulativeReportDTO get_Category([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {        
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.MI_Id = mid;
            return crStr.get_Category(data);
        }
        [Route("aggregativereport")]
        public VikasaSubjectwiseCumulativeReportDTO aggregativereport([FromBody]VikasaSubjectwiseCumulativeReportDTO data)
        {        
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.MI_Id = mid;
            return crStr.aggregativereport(data);
        }
    }
}
