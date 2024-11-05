using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaacServiceHub.LessonPlanner.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.LessonPlanner.FacadeController
{
    [Route("api/[controller]")]
    public class CollegeStaffPeriodMappingFacadeController : Controller
    {
        public CollegeStaffPeriodMappingInterface _interface;

        public CollegeStaffPeriodMappingFacadeController(CollegeStaffPeriodMappingInterface _inter)
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
        public CollegeStaffPeriodMappingDTO Getdetails([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.Getdetails(data);
        }
        [Route("getemployeedetails")]
        public CollegeStaffPeriodMappingDTO getemployeedetails([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.getemployeedetails(data);
        }
        [Route("onchangecourse")]
        public CollegeStaffPeriodMappingDTO onchangecourse([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStaffPeriodMappingDTO onchangebranch([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.onchangebranch(data);
        }
        [Route("onchangesemster")]
        public CollegeStaffPeriodMappingDTO onchangesemster([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.onchangesemster(data);
        }
        [Route("onchangesection")]
        public CollegeStaffPeriodMappingDTO onchangesection([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.onchangesection(data);
        }
        [Route("getsearchdetails")]
        public CollegeStaffPeriodMappingDTO getsearchdetails([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.getsearchdetails(data);
        }
        [Route("savedata")]
        public CollegeStaffPeriodMappingDTO savedata([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.savedata(data);
        }

        // Staff Transaction
        [Route("Getdetailstransaction")]
        public CollegeStaffPeriodMappingDTO Getdetailstransaction([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.Getdetailstransaction(data);
        }
        [Route("getsearchdetailstransaction")]
        public CollegeStaffPeriodMappingDTO getsearchdetailstransaction([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.getsearchdetailstransaction(data);
        }
        [Route("savedatatransaction")]
        public CollegeStaffPeriodMappingDTO savedatatransaction([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            return _interface.savedatatransaction(data);
        }
        
    }
}
