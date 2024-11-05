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
    public class SwimmingAttendanceReportFacadeController : Controller
    {
        public SwimmingAttendanceReportInterface _interface;

        public SwimmingAttendanceReportFacadeController(SwimmingAttendanceReportInterface _inter)
        {
            _interface = _inter;
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

        [Route("loaddata")]
        public SwimmingAttendanceReportDTO loaddata([FromBody] SwimmingAttendanceReportDTO data)
        {
            return _interface.loaddata(data);
        }

        [Route("onchnageyear")]
        public SwimmingAttendanceReportDTO onchnageyear([FromBody] SwimmingAttendanceReportDTO data)
        {
            return _interface.onchnageyear(data);
        }
        [Route("onchangeclass")]
        public SwimmingAttendanceReportDTO onchangeclass([FromBody] SwimmingAttendanceReportDTO data)
        {
            return _interface.onchangeclass(data);
        }
        [Route("search")]
        public SwimmingAttendanceReportDTO search([FromBody] SwimmingAttendanceReportDTO data)
        {
            return _interface.search(data);
        }

    }
}
