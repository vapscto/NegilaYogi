using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.LessonPlanner.Interface;
using PreadmissionDTOs.NAAC.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.LessonPlanner.FacadeController
{
    [Route("api/[controller]")]
    public class LMSStudentFacadeController : Controller
    {
        public LMSStudentInterface _interface; 

        public LMSStudentFacadeController(LMSStudentInterface _inter)
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

        //College

        [HttpPost("Getdetails")]
        public LMSStudentDTO Getdetails([FromBody] LMSStudentDTO data)
        {
            return _interface.Getdetails(data);
        }
        [HttpPost("onchangesemester")]
        public LMSStudentDTO onchangesemester([FromBody] LMSStudentDTO data)
        {
            return _interface.onchangesemester(data);
        }

        [HttpPost("getcollegetopics")]
        public LMSStudentDTO getcollegetopics([FromBody] LMSStudentDTO data)
        {
            return _interface.getcollegetopics(data);
        }
        [HttpPost("getcollegedocuments")]
        public LMSStudentDTO getcollegedocuments([FromBody] LMSStudentDTO data)
        {
            return _interface.getcollegedocuments(data);
        }

        // School 
        [HttpPost("Getdetailsschool")]
        public LMSStudentDTO Getdetailsschool([FromBody] LMSStudentDTO data)
        {
            return _interface.Getdetailsschool(data);
        }

        [HttpPost("onchangeyear")]
        public LMSStudentDTO onchangeyear([FromBody] LMSStudentDTO data)
        {
            return _interface.onchangeyear(data);
        }

        [HttpPost("onchangeclass")]
        public LMSStudentDTO onchangeclass([FromBody] LMSStudentDTO data)
        {
            return _interface.onchangeclass(data);
        }

        [HttpPost("getschooltopics")]
        public LMSStudentDTO getschooltopics([FromBody] LMSStudentDTO data)
        {
            return _interface.getschooltopics(data);
        }

        [HttpPost("getschooldocuments")]
        public LMSStudentDTO getschooldocuments([FromBody] LMSStudentDTO data)
        {
            return _interface.getschooldocuments(data);
        }
    }
}
