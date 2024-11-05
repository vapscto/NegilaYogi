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
    public class CLGTTStaffWiseReportController : Controller
    {
        CLGTTStaffWiseReportDelegate objdelegate = new CLGTTStaffWiseReportDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getdetails")]
        public CLGTTStaffWiseReportDTO Get([FromQuery] int id)
        {
            CLGTTStaffWiseReportDTO data = new CLGTTStaffWiseReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }
       
        [HttpPost]
        [Route("getrpt")]
        public CLGTTStaffWiseReportDTO getrpt([FromBody] CLGTTStaffWiseReportDTO data)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getrpt(data);
        }
        
        [HttpPost]
        [Route("GetStaffDetails")]
        public CLGTTStaffWiseReportDTO GetStaffDetails([FromBody] CLGTTStaffWiseReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.GetStaffDetails(data);
        }

    }
}
