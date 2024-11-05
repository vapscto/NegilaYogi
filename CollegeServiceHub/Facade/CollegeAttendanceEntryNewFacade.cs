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
    public class CollegeAttendanceEntryNewFacade : Controller
    {

        public CollegeAttendanceEntryNewInterface _attEntryForm;

        public CollegeAttendanceEntryNewFacade(CollegeAttendanceEntryNewInterface attEntryForm)
        {
            _attEntryForm = attEntryForm;
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


        [Route("getalldetails")]
        public CollegeMultiHoursAttendanceEntryDTO Getalldetails([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getalldetails(data);
        }

        [Route("getsubjectslist")]
        public CollegeMultiHoursAttendanceEntryDTO getsubjectslist([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getsubjectslist(data);
        }
        [Route("getStudentdata")]
        public CollegeMultiHoursAttendanceEntryDTO getStudentdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getStudentdata(data);
        }

        [Route("getBatchdata")]
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getBatchdata(data);
        }
        [Route("saveatt")]
        public CollegeMultiHoursAttendanceEntryDTO saveatt([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.networkip = remoteIpAddress.ToString();
            return _attEntryForm.saveatt(data);
        }
        [Route("getsaveddatepreview")]
        public CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getsaveddatepreview(data);
        }
    }
}
