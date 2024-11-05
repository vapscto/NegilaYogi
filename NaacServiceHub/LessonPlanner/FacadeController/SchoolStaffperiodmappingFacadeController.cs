using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaacServiceHub.com.vaps.LessonPlanner.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.com.vaps.LessonPlanner.FacadeController
{
    [Route("api/[controller]")]
    public class SchoolStaffperiodmappingFacadeController : Controller
    {
        public SchoolStaffperiodmappingInterface _delg; 

        public SchoolStaffperiodmappingFacadeController(SchoolStaffperiodmappingInterface delg)
        {
            _delg = delg;
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
        public SchoolStaffperiodmappingDTO Getdetails([FromBody] SchoolStaffperiodmappingDTO data)
        {           
            return _delg.Getdetails(data);
        }

        [Route("getemployeedetails")]
        public SchoolStaffperiodmappingDTO getemployeedetails([FromBody] SchoolStaffperiodmappingDTO data)
        {           
            return _delg.getemployeedetails(data);
        }
        [Route("onchangeclass")]
        public SchoolStaffperiodmappingDTO onchangeclass([FromBody] SchoolStaffperiodmappingDTO data)
        {           
            return _delg.onchangeclass(data);
        }
        [Route("onchangesection")]
        public SchoolStaffperiodmappingDTO onchangesection([FromBody] SchoolStaffperiodmappingDTO data)
        {           
            return _delg.onchangesection(data);
        }
        [Route("getsearchdetails")]
        public SchoolStaffperiodmappingDTO getsearchdetails([FromBody] SchoolStaffperiodmappingDTO data)
        {           
            return _delg.getsearchdetails(data);
        }
        [Route("savedata")]
        public SchoolStaffperiodmappingDTO savedata([FromBody] SchoolStaffperiodmappingDTO data)
        {           
            return _delg.savedata(data);
        }
        [Route("Getdetailstransaction")]
        public SchoolStaffperiodmappingDTO Getdetailstransaction([FromBody] SchoolStaffperiodmappingDTO data)
        {
            return _delg.Getdetailstransaction(data);
        }
        [Route("getsearchdetailstransaction")]
        public SchoolStaffperiodmappingDTO getsearchdetailstransaction([FromBody] SchoolStaffperiodmappingDTO data)
        {
            return _delg.getsearchdetailstransaction(data);
        }
        [Route("savedatatransaction")]
        public SchoolStaffperiodmappingDTO savedatatransaction([FromBody] SchoolStaffperiodmappingDTO data)
        {
            return _delg.savedatatransaction(data);
        }
    }
}
