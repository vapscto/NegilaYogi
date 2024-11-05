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
    public class CLGTTSubjectWiseReportController : Controller
    {
        CLGTTSubjectWiseReportDelegate objdelegate = new CLGTTSubjectWiseReportDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("getdetails/{id:int}")]
        public CLGTTSubjectWiseReportDTO getdetails(int id)
        {
            CLGTTSubjectWiseReportDTO data = new CLGTTSubjectWiseReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }

        [Route("getbranch")]
        public CLGTTSubjectWiseReportDTO getbranch([FromBody] CLGTTSubjectWiseReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getbranch(data);
        }
        [Route("getsemister")]
        public CLGTTSubjectWiseReportDTO getsemister([FromBody] CLGTTSubjectWiseReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getsemister(data);
        }
        [Route("savedetail")]
        public CLGTTSubjectWiseReportDTO savedetail([FromBody] CLGTTSubjectWiseReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(data);
        }




    }
}
