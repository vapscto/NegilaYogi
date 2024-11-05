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
    public class OverallAttendanceFacadeController : Controller
    {
        public OverallDailyAttendanceInterface _AttenRpt;
      
        public OverallAttendanceFacadeController(OverallDailyAttendanceInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
        }
        // GET: api/values
      //  [HttpGet]
        // load initial dropdown
        [Route("getinitialdata")]
        public Task<StudentAttendanceReportDTO> getinitialdata([FromBody]StudentAttendanceReportDTO mi_id)
        {
            return _AttenRpt.getInitailData(mi_id);
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
        [Route("getoveallattendance")]
        public Task<StudentAttendanceReportDTO> getoveallattendance([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getoveallattendance(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [Route("getstudents")]
        public StudentAttendanceReportDTO getStudentDet([FromBody] StudentAttendanceReportDTO dto)
        {
            return _AttenRpt.getStudentDetails(dto);
        }

        //getStudentAllDetails

        [Route("getStudentAllDetails")]
        public StudentAttendanceReportDTO getStudentAllDetails([FromBody] StudentAttendanceReportDTO dto)
        {
            return _AttenRpt.getStudentAllDetails(dto);
        }



    }
}
