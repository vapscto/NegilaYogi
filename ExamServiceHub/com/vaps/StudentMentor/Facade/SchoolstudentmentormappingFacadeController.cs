using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.StudentMentor.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.StudentMentor;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.StudentMentor.Facade
{
    [Route("api/[controller]")]
    public class SchoolstudentmentormappingFacadeController : Controller
    {
        public SchoolstudentmentormappingInterface _interface;

        public SchoolstudentmentormappingFacadeController(SchoolstudentmentormappingInterface _inter)
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
        public SchoolstudentmentormappingDTO Getdetails([FromBody]SchoolstudentmentormappingDTO data)
        {
            return _interface.Getdetails(data);
        }
        [Route("onchangeyear")]
        public SchoolstudentmentormappingDTO onchangeyear([FromBody] SchoolstudentmentormappingDTO data)
        {
            return _interface.onchangeyear(data);
        }
        [Route("getsection")]
        public SchoolstudentmentormappingDTO getsection([FromBody] SchoolstudentmentormappingDTO data)
        {
            return _interface.getsection(data);
        }
        [Route("getemployee")]
        public SchoolstudentmentormappingDTO getemployee([FromBody] SchoolstudentmentormappingDTO data)
        {           
            return _interface.getemployee(data);
        }
        [Route("getstudentdata")]
        public SchoolstudentmentormappingDTO getstudentdata([FromBody] SchoolstudentmentormappingDTO data)
        {           
            return _interface.getstudentdata(data);
        }
        [Route("savedata")]
        public SchoolstudentmentormappingDTO savedata([FromBody] SchoolstudentmentormappingDTO data)
        {           
            return _interface.savedata(data);
        }
        [Route("viewrecordspopup")]
        public SchoolstudentmentormappingDTO viewrecordspopup([FromBody] SchoolstudentmentormappingDTO data)
        {           
            return _interface.viewrecordspopup(data);
        }
        [Route("Deletedata")]
        public SchoolstudentmentormappingDTO Deletedata([FromBody] SchoolstudentmentormappingDTO data)
        {           
            return _interface.Deletedata(data);
        }
        [Route("onreport")]
        public SchoolstudentmentormappingDTO onreport([FromBody] SchoolstudentmentormappingDTO data)
        {           
            return _interface.onreport(data);
        }
    }
}
