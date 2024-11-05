using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeAttendanceAbsentSMSFacade : Controller
    {


        public CollegeAttendanceAbsentSMSInterface _inter;

        public CollegeAttendanceAbsentSMSFacade(CollegeAttendanceAbsentSMSInterface obj)
        {
            _inter = obj;
        }


        [Route("getdetails")]
        public CollegeDailyAttendanceDTO getdetails([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.getdetails(data);
        }


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
        [Route("getAttendetails")]
        public Task<CollegeDailyAttendanceDTO> getAttendetails([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.getAttendetails(data);
        }
        [Route("absentsendsms")]
        public Task<CollegeDailyAttendanceDTO> absentsendsms([FromBody] CollegeDailyAttendanceDTO data)
        {
            return _inter.absentsendsms(data);
        }

    }
}
