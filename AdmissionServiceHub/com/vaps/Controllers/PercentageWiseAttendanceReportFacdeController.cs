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
    public class PercentageWiseAttendanceReportFacdeController : Controller
    {
        public PercentageWiseAttendanceReportInterface _interface;

        public PercentageWiseAttendanceReportFacdeController(PercentageWiseAttendanceReportInterface _int)
        {
            _interface = _int;
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
        [Route("getloaddata")]
        public PercentageWiseAttendanceReportDTO getloaddata([FromBody]  PercentageWiseAttendanceReportDTO data)
        { 
            return _interface.getloaddata(data);
        }

        [Route("getclass")]
        public PercentageWiseAttendanceReportDTO getclass([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            return _interface.getclass(data);
        }
        [Route("getsection")]
        public PercentageWiseAttendanceReportDTO getsection([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            return _interface.getsection(data);
        }

        [Route("showreport")]
        public PercentageWiseAttendanceReportDTO showreport([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            return _interface.showreport(data);
        }

        [Route("SendAttendanceSMS")]
        public Task<PercentageWiseAttendanceReportDTO> SendAttendanceSMS([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            return _interface.SendAttendanceSMS(data);
        }
    }
}
