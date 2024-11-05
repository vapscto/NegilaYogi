using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class DatewiseAttendanceReportFacadeController : Controller
    {
        public DatewiseAttendanceReportInterface _intf;

        public DatewiseAttendanceReportFacadeController(DatewiseAttendanceReportInterface adstu)
        {
            _intf = adstu;
        }


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

        [Route("getdata")]
        public DatewiseAttendanceReportDTO getdata([FromBody] DatewiseAttendanceReportDTO data)
        {
            return _intf.getdata(data);
        }

        [Route("onchangeyear")]
        public DatewiseAttendanceReportDTO onchangeyear([FromBody] DatewiseAttendanceReportDTO data)
        {
            return _intf.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public DatewiseAttendanceReportDTO onchangeclass([FromBody] DatewiseAttendanceReportDTO data)
        {
            return _intf.onchangeclass(data);
        }
        [Route("getreport")]
        public DatewiseAttendanceReportDTO getreport([FromBody] DatewiseAttendanceReportDTO data)
        {
            return _intf.getreport(data);
        }
        [Route("getcountreport")]
        public DatewiseAttendanceReportDTO getcountreport([FromBody] DatewiseAttendanceReportDTO data)
        {
            return _intf.getcountreport(data);
        }
        [Route("Reportnew")]
        public DatewiseAttendanceReportDTO Reportnew([FromBody] DatewiseAttendanceReportDTO data)
        {
            return _intf.Reportnew(data);
        }
        
    }
}
