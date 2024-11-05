using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgAttendanceEntryFacadeController : Controller
    {
        public ClgAttendanceEntryInterface _attEntryForm;

        public ClgAttendanceEntryFacadeController(ClgAttendanceEntryInterface attEntryForm)
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
        public ClgAttendanceEntryDTO getalldetails([FromBody] ClgAttendanceEntryDTO data)
        {
            return _attEntryForm.getalldetails(data);
        }

        //[Route("getCoursedata")]
        //public ClgAttendanceEntryDTO getCoursedata([FromBody] ClgAttendanceEntryDTO data)
        //{
        //    return _attEntryForm.getCoursedata(data);
        //}
        [Route("getBranchdata")]
        public ClgAttendanceEntryDTO getBranchdata([FromBody] ClgAttendanceEntryDTO data)
        {
            return _attEntryForm.getBranchdata(data);

        }
        [Route("getSemesterdata")]
        public ClgAttendanceEntryDTO getSemesterdata([FromBody] ClgAttendanceEntryDTO data)
        {
            return _attEntryForm.getSemesterdata(data);
        }

        [Route("getSectiondata")]
        public ClgAttendanceEntryDTO getSectiondata([FromBody] ClgAttendanceEntryDTO data)
        {
            return _attEntryForm.getSectiondata(data);
        }

        [Route("getSubjdata")]
        public ClgAttendanceEntryDTO getSubjdata([FromBody] ClgAttendanceEntryDTO data)
        {
            return _attEntryForm.getSubjdata(data);
        }

        [Route("getBatchdata")]
        public ClgAttendanceEntryDTO getBatchdata([FromBody] ClgAttendanceEntryDTO data)
        {
            return _attEntryForm.getBatchdata(data);
        }
        [Route("getStudentdata")]
        public ClgAttendanceEntryDTO getStudentdata([FromBody] ClgAttendanceEntryDTO data)
        {
            return _attEntryForm.getStudentdata(data);
        }
        [Route("saveatt")]
        public ClgAttendanceEntryDTO saveatt([FromBody] ClgAttendanceEntryDTO data)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.networkip = remoteIpAddress.ToString();
            return _attEntryForm.saveatt(data);
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
