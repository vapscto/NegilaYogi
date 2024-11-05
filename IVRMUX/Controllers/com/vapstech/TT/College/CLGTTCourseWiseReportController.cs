using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CLGTTCourseWiseReportController : Controller
    {
        CLGTTCourseWiseReportDelegate objdelegate = new CLGTTCourseWiseReportDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getdetails")]
        public CLGTTCourseWiseReportDTO Get([FromQuery] int id)
        {
            CLGTTCourseWiseReportDTO data = new CLGTTCourseWiseReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }
        [HttpPost]
        [Route("getclass_catg")]
        public CLGTTCourseWiseReportDTO getclass_catg([FromBody] CLGTTCourseWiseReportDTO data)
        {
          
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclass_catg(data);
        }
        [HttpPost]
        [Route("getrpt")]
        public CLGTTCourseWiseReportDTO getrpt([FromBody] CLGTTCourseWiseReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getrpt(data);
        }

        [HttpGet]
        [Route("GetStudentDetails/{id:int}")]
        public CLGTTCourseWiseReportDTO GetStudentDetails(int id)
        {
            CLGTTCourseWiseReportDTO data = new CLGTTCourseWiseReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.GetStudentDetails(data);
        }
    }
}
