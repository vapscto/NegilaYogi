using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class SwimmingAttendanceReportController : Controller
    {
        public SwimmingAttendanceReportDelegate _delg = new SwimmingAttendanceReportDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("loaddata/{id:int}")]
        public SwimmingAttendanceReportDTO loaddata(int id)
        {
            SwimmingAttendanceReportDTO data = new SwimmingAttendanceReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }

        [Route("onchnageyear")]
        public SwimmingAttendanceReportDTO onchnageyear([FromBody] SwimmingAttendanceReportDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchnageyear(data);
        }
        [Route("onchangeclass")]
        public SwimmingAttendanceReportDTO onchangeclass([FromBody] SwimmingAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeclass(data);
        }
        [Route("search")]
        public SwimmingAttendanceReportDTO search([FromBody] SwimmingAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.search(data);
        }    
    }
}
