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
    public class CollegedepartmentcoursebranchmappingFacadeController : Controller
    {
        public CollegedepartmentcoursebranchmappingInterface _interface;


        public CollegedepartmentcoursebranchmappingFacadeController(CollegedepartmentcoursebranchmappingInterface _inter)
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
        public CollegedepartmentcoursebranchmappingDTO Getdetails([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.Getdetails(data);
        }

        [Route("getbranch")]
        public CollegedepartmentcoursebranchmappingDTO getbranch([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.getbranch(data);
        }

        [Route("getsemester")]
        public CollegedepartmentcoursebranchmappingDTO getsemester([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.getsemester(data);
        }
        [Route("savedetails")]
        public CollegedepartmentcoursebranchmappingDTO savedetails([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.savedetails(data);
        }
        [Route("viewrecordspopup")]
        public CollegedepartmentcoursebranchmappingDTO viewrecordspopup([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.viewrecordspopup(data);
        }
        [Route("semesterdeactive")]
        public CollegedepartmentcoursebranchmappingDTO semesterdeactive([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.semesterdeactive(data);
        }
        [Route("deactivate")]
        public CollegedepartmentcoursebranchmappingDTO deactivate([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.deactivate(data);
        }

        [Route("Getdetailsreport")]
        public CollegedepartmentcoursebranchmappingDTO Getdetailsreport([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.Getdetailsreport(data);
        }

        [Route("getreport")]
        public CollegedepartmentcoursebranchmappingDTO getreport([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            return _interface.getreport(data);
        }

    }
}
