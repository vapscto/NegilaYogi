using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.StudentMentorMapping.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.StudentMentorMapping.FacadeController
{
    [Route("api/[controller]")]
    public class CollegestudentmentormappingFacadeController : Controller
    {
        public CollegestudentmentormappingInterface _interface; 

        public CollegestudentmentormappingFacadeController(CollegestudentmentormappingInterface _inter)
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

        [Route("Getdetails")]
        public CollegestudentmentormappingDTO Getdetails([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.Getdetails(data);
        }
        [Route("onchangeyear")]
        public CollegestudentmentormappingDTO onchangeyear([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.onchangeyear(data);
        }
        [Route("getbranch")]
        public CollegestudentmentormappingDTO getbranch([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.getbranch(data);
        }
        [Route("getsemester")]
        public CollegestudentmentormappingDTO getsemester([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.getsemester(data);
        }
        [Route("getsection")]
        public CollegestudentmentormappingDTO getsection([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.getsection(data);
        }
        [Route("getemployee")]
        public CollegestudentmentormappingDTO getemployee([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.getemployee(data);
        }

        [Route("getstudentdata")]
        public CollegestudentmentormappingDTO getstudentdata([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.getstudentdata(data);
        }
        [Route("savedata")]
        public CollegestudentmentormappingDTO savedata([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.savedata(data);
        }
        [Route("viewrecordspopup")]
        public CollegestudentmentormappingDTO viewrecordspopup([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.viewrecordspopup(data);
        }
        [Route("Deletedata")]
        public CollegestudentmentormappingDTO Deletedata([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.Deletedata(data);
        }

        //Report
        [Route("Getreportdetails")]
        public CollegestudentmentormappingDTO Getreportdetails([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.Getreportdetails(data);
        }
        [Route("getreport")]
        public CollegestudentmentormappingDTO getreport([FromBody]CollegestudentmentormappingDTO data)
        {
            return _interface.getreport(data);
        }
    }
}
