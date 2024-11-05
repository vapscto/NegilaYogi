using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentYearlyAttendanceFacadeController : Controller
    {
        public StudentYearlyAttendanceInterface _AttenRpt;
        public StudentYearlyAttendanceFacadeController(StudentYearlyAttendanceInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
        }
        // load initial dropdown
        [Route("getinitialdata/{mi_id:int}")]
        public Task<StudentAttendanceReportDTO> getinitialdata(int mi_id)
        {
            return _AttenRpt.getInitailData(mi_id);
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("searchdata")]
        public Task<StudentAttendanceReportDTO> searchdata([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getserdata(data);
        }

        [Route("getclass")]
        public StudentAttendanceReportDTO getclass([FromBody]StudentAttendanceReportDTO dto)
        {
            return _AttenRpt.getclass(dto);
        }
    }
}
