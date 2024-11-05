using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeDailyAttendanceFacadeController : Controller
    {
        private CollegeDailyAttendanceInterface _inter;

        public CollegeDailyAttendanceFacadeController(CollegeDailyAttendanceInterface obj)
        {
            _inter = obj;
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

        [Route("getdetails")]
        public CollegeDailyAttendanceDTO Getdetails([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.getdetails(data);
        }

        [HttpPost]
        [Route("onselectAcdYear")]
        public CollegeDailyAttendanceDTO onselectAcdYear([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public CollegeDailyAttendanceDTO onselectCourse([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.onselectCourse(data);
        }

        [Route("onselectBranch")]
        public CollegeDailyAttendanceDTO onselectBranch([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.onselectBranch(data);
        }
        [Route("getsection")]
        public CollegeDailyAttendanceDTO getsection([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.getsection(data);
        }
        [Route("getsubject")]
        public CollegeDailyAttendanceDTO getsubject([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.getsubject(data);
        }

        [Route("onreport")]
        public Task<CollegeDailyAttendanceDTO> onreport([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.onreport(data);
        }
        [Route("onreportpercentage")]
        public Task<CollegeDailyAttendanceDTO> onreportpercentage([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.onreportpercentage(data);
        }
        [Route("getAttendetails")]
        public Task<CollegeDailyAttendanceDTO> getAttendetails([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.getAttendetails(data);

        }
        [Route("GetAttendancedetails")]
        public Task<CollegeDailyAttendanceDTO> GetAttendancedetails([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.GetAttendancedetails(data);

        }

        [Route("getStudentAbsentDetails")]
        public CollegeDailyAttendanceDTO getStudentAbsentDetails([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.getStudentAbsentDetails(data);

        }

        //getStudentDetails
        [Route("absentsendsms")]
        public Task<CollegeDailyAttendanceDTO> absentsendsms([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.absentsendsms(data);
        }
        [Route("onreportshortagepercentage")]
        public Task<CollegeDailyAttendanceDTO> onreportshortagepercentage([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.onreportshortagepercentage(data);
        }
        [Route("onreporttotalattendance")]
        public Task<CollegeDailyAttendanceDTO> onreporttotalattendance([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.onreporttotalattendance(data);
        }
        
        public void Post([FromBody]string value)
        {
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
    }
}
