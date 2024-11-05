using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CollegeCumulativeAvgBestReportFacadeController : Controller
    {
        public CollegeCumulativeAvgBestReportInterface _interface;

        public CollegeCumulativeAvgBestReportFacadeController(CollegeCumulativeAvgBestReportInterface _int)
        {
            _interface = _int;
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
        public CollegeCumulativeAvgBestReportDTO Getdetails([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.Getdetails(data);
        }
        [Route("onchangeyear")]
        public CollegeCumulativeAvgBestReportDTO onchangeyear([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeCumulativeAvgBestReportDTO onchangecourse([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeCumulativeAvgBestReportDTO onchangebranch([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeCumulativeAvgBestReportDTO onchangesemester([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.onchangesemester(data);
        }
        [Route("onchangesubjectscheme")]
        public CollegeCumulativeAvgBestReportDTO onchangesubjectscheme([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.onchangesubjectscheme(data);
        }
        [Route("onchangeschemetype")]
        public CollegeCumulativeAvgBestReportDTO onchangeschemetype([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.onchangeschemetype(data);
        }
        [Route("Getcmreport")]
        public CollegeCumulativeAvgBestReportDTO Getcmreport([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.Getcmreport(data);
        }
        [Route("getindreport")]
        public CollegeCumulativeAvgBestReportDTO getindreport([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            return _interface.getindreport(data);
        }


    }
}
