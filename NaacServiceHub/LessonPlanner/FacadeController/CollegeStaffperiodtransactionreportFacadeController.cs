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
    public class CollegeStaffperiodtransactionreportFacadeController : Controller
    {
        public CollegeStaffperiodtransactionreportInterface _interface;


        public CollegeStaffperiodtransactionreportFacadeController(CollegeStaffperiodtransactionreportInterface _inter)
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

        [Route("Getdetailstransaction")]
        public CollegeStaffperiodtransactionreportDTO Getdetailstransaction([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {          
            return _interface.Getdetailstransaction(data);
        }

        [Route("onselectAcdYear")]
        public CollegeStaffperiodtransactionreportDTO onselectAcdYear([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            return _interface.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public CollegeStaffperiodtransactionreportDTO onselectCourse([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            return _interface.onselectCourse(data);
        }
        [Route("onselectBranch")]
        public CollegeStaffperiodtransactionreportDTO onselectBranch([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            return _interface.onselectBranch(data);
        }
        [Route("getsection")]
        public CollegeStaffperiodtransactionreportDTO getsection([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            return _interface.getsection(data);
        }

        [Route("onchangesection")]
        public CollegeStaffperiodtransactionreportDTO onchangesection([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            return _interface.onchangesection(data);
        }
        [Route("getreport")]
        public CollegeStaffperiodtransactionreportDTO getreport([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            return _interface.getreport(data);
        }

        [Route("getdevationreport")]
        public CollegeStaffperiodtransactionreportDTO getdevationreport([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            return _interface.getdevationreport(data);
        }

    }
}
