using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeMultiHoursAttendanceEntryFacadeController : Controller
    {
        public CollegeMultiHoursAttendanceEntryInterface _attEntryForm;

        public CollegeMultiHoursAttendanceEntryFacadeController(CollegeMultiHoursAttendanceEntryInterface attEntryForm)
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
        public CollegeMultiHoursAttendanceEntryDTO getalldetails([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getalldetails(data);
        }

        [Route("balgetalldetails")]
        public CollegeMultiHoursAttendanceEntryDTO bal_getalldetails([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.bal_getalldetails(data);
        }

        [Route("getCoursedata")]
        public CollegeMultiHoursAttendanceEntryDTO getCoursedata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getCoursedata(data);
        }
        [Route("getBranchdata")]
        public CollegeMultiHoursAttendanceEntryDTO getBranchdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getBranchdata(data);

        }
        [Route("getSemesterdata")]
        public CollegeMultiHoursAttendanceEntryDTO getSemesterdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getSemesterdata(data);
        }

        [Route("getSectiondata")]
        public CollegeMultiHoursAttendanceEntryDTO getSectiondata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getSectiondata(data);
        }

        [Route("getSubjdata")]
        public CollegeMultiHoursAttendanceEntryDTO getSubjdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getSubjdata(data);
        }

        [Route("getBatchdata")]
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getBatchdata(data);
        }
        [Route("getStudentdata")]
        public CollegeMultiHoursAttendanceEntryDTO getStudentdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getStudentdata(data);
        }
        [Route("saveatt")]
        public CollegeMultiHoursAttendanceEntryDTO saveatt([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.networkip = remoteIpAddress.ToString();
            return _attEntryForm.saveatt(data);
        }
        [Route("delete")]
        public CollegeMultiHoursAttendanceEntryDTO delete([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.networkip = remoteIpAddress.ToString();
            return _attEntryForm.delete(data);
        }
        [Route("autoscheduler")]
        public Task<CollegeMultiHoursAttendanceEntryDTO> autoscheduler([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {   
            return _attEntryForm.autoscheduler(data);
        }
        [Route("getsaveddatepreview")]
        public CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _attEntryForm.getsaveddatepreview(data);
        }


        // POST api/values
        [HttpPost]
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
