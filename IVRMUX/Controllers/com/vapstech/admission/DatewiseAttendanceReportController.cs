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
    public class DatewiseAttendanceReportController : Controller
    {
        DatewiseAttendanceReportDelegate _delgate = new DatewiseAttendanceReportDelegate();
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

        [Route("getdata/{id:int}")]
        public DatewiseAttendanceReportDTO getdata(int id)
        {
            DatewiseAttendanceReportDTO data = new DatewiseAttendanceReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getdata(data);
        }

        [Route("onchangeyear")]
        public DatewiseAttendanceReportDTO onchangeyear([FromBody] DatewiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public DatewiseAttendanceReportDTO onchangeclass([FromBody] DatewiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.onchangeclass(data);
        }
        [Route("getreport")]
        public DatewiseAttendanceReportDTO getreport([FromBody] DatewiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getreport(data);
        }
        [Route("getcountreport")]
        public DatewiseAttendanceReportDTO getcountreport([FromBody] DatewiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getcountreport(data);
        }
        [Route("Reportnew")]
        public DatewiseAttendanceReportDTO Reportnew([FromBody] DatewiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.Reportnew(data);
        }
        
    }
}
