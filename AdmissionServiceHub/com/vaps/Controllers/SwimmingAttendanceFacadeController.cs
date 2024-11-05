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
    public class SwimmingAttendanceFacadeController : Controller
    {
        public SwimmingAttendanceInterface _interface;

        public SwimmingAttendanceFacadeController(SwimmingAttendanceInterface _inter)
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
        public SwimmingAttendanceDTO loaddata([FromBody] SwimmingAttendanceDTO data)
        {
            return _interface.loaddata(data);
        }
        [Route("onchnageyear")]
        public SwimmingAttendanceDTO onchnageyear([FromBody]  SwimmingAttendanceDTO data)
        {
            return _interface.onchnageyear(data);
        }
        [Route("onchangeclass")]
        public SwimmingAttendanceDTO onchangeclass([FromBody]  SwimmingAttendanceDTO data)
        {
            return _interface.onchangeclass(data);
        }
        [Route("search")]
        public SwimmingAttendanceDTO search([FromBody]  SwimmingAttendanceDTO data)
        {
            return _interface.search(data);
        }
        [Route("save")]
        public SwimmingAttendanceDTO save([FromBody]  SwimmingAttendanceDTO data)
        {
            return _interface.save(data);
        }
    }
}
