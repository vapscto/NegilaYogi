using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeStudentTCReportFacadeController : Controller
    {

        public CollegeStudentTCReportInterface _interface;

        public CollegeStudentTCReportFacadeController(CollegeStudentTCReportInterface _intr)
        {
            _interface = _intr;
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

        [Route("getalldetails")]
        public CollegeStudentTCReportDTO getalldetails([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.getalldetails(data);
        }
        [Route("onchangeyear")]
        public CollegeStudentTCReportDTO onchangeyear([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeStudentTCReportDTO onchangecourse([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStudentTCReportDTO onchangebranch([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeStudentTCReportDTO onchangesemester([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.onchangesemester(data);
        }
        [Route("Getreportdetails")]
        public CollegeStudentTCReportDTO Getreportdetails([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.Getreportdetails(data);
        }
        // TC Custom Report
        [Route("onchangeyeartc")]
        public CollegeStudentTCReportDTO onchangeyeartc([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.onchangeyeartc(data);
        }
        [Route("stdnamechange")]
        public CollegeStudentTCReportDTO stdnamechange([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.stdnamechange(data);
        }
        [Route("getTcdetails")]
        public CollegeStudentTCReportDTO getTcdetails([FromBody] CollegeStudentTCReportDTO data)
        {
            return _interface.getTcdetails(data);
        }       
    }
}
