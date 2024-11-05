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
    public class SchoolStaffperiodtransactionreportFacadeController : Controller
    {
        public SchoolStaffperiodtransactionreportInterface _interface;


        public SchoolStaffperiodtransactionreportFacadeController(SchoolStaffperiodtransactionreportInterface _inter)
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
        public SchoolStaffperiodtransactionreportDTO Getdetailstransaction([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            return _interface.Getdetailstransaction(data);
        }

        [Route("onselectAcdYear")]
        public SchoolStaffperiodtransactionreportDTO onselectAcdYear([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            return _interface.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public SchoolStaffperiodtransactionreportDTO onselectclass([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            return _interface.onselectclass(data);
        }       

        [Route("onchangesection")]
        public SchoolStaffperiodtransactionreportDTO onchangesection([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            return _interface.onchangesection(data);
        }
        [Route("getreport")]
        public SchoolStaffperiodtransactionreportDTO getreport([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            return _interface.getreport(data);
        }

        [Route("getdevationreport")]
        public SchoolStaffperiodtransactionreportDTO getdevationreport([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            return _interface.getdevationreport(data);
        }

    }
}
