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
    public class AttendanceReportFacadeController : Controller
    {
        public AttendanceReportInterface _AttenRpt;
      
        public AttendanceReportFacadeController(AttendanceReportInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // load initial dropdown
        [Route("getinitialdata")]
        public Task<StudentAttendanceReportDTO> getinitialdata([FromBody]StudentAttendanceReportDTO mi_id)
        {
            return _AttenRpt.getInitailData(mi_id);
        }
        [Route("getsection")]
        public Task<StudentAttendanceReportDTO> getsection([FromBody]StudentAttendanceReportDTO mi_id)
        {
            return _AttenRpt.getsection(mi_id);
        }
        [Route("getclass")]
        public Task<StudentAttendanceReportDTO> getclass([FromBody]StudentAttendanceReportDTO mi_id)
        {
            return _AttenRpt.getclass(mi_id);
        }

        [HttpPost]
        [Route("searchdata")]
        public Task<StudentAttendanceReportDTO> searchdata([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getserdata(data);
        }


        [Route("shortageOfAttendanceAlert")]
        public Task<StudentAttendanceReportDTO> shortageOfAttendanceAlert([FromBody]StudentAttendanceReportDTO mi_id)
        {
            return _AttenRpt.shortageOfAttendanceAlert(mi_id);
        }


    }
}
