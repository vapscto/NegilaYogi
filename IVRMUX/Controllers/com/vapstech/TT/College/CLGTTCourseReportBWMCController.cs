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
    public class CLGTTCourseReportBWMCController : Controller
    {
        CLGTTCourseReportBWMCDelegate objdelegate = new CLGTTCourseReportBWMCDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getdetails")]
        public CLGTTCourseReportBWMCDTO Get([FromQuery] int id)
        {
            CLGTTCourseReportBWMCDTO data = new CLGTTCourseReportBWMCDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }
        [HttpPost]
        [Route("getclass_catg")]
        public CLGTTCourseReportBWMCDTO getclass_catg([FromBody] CLGTTCourseReportBWMCDTO data)
        {
          
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclass_catg(data);
        }
        [HttpPost]
        [Route("getrpt")]
        public CLGTTCourseReportBWMCDTO getrpt([FromBody] CLGTTCourseReportBWMCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getrpt(data);
        }
        
    }
}
