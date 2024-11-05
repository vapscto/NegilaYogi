using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamLoginPrivilegesReportFacadeController : Controller
    {
        private ExamLoginPrivilegesReportInterface _inter;

        public ExamLoginPrivilegesReportFacadeController(ExamLoginPrivilegesReportInterface obj)
        {
            _inter = obj;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values

        [HttpPost]
        [Route("getdetails")]
        public ExamLoginPrivilegesReportDTO Getdetails([FromBody] ExamLoginPrivilegesReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onselectAcdYear")]
        public ExamLoginPrivilegesReportDTO onselectAcdYear([FromBody] ExamLoginPrivilegesReportDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onchangecategory")]
        public ExamLoginPrivilegesReportDTO onchangecategory([FromBody] ExamLoginPrivilegesReportDTO data)
        {
            return _inter.onchangecategory(data);
        }

        [Route("onselectclass")]
        public ExamLoginPrivilegesReportDTO onselectclass([FromBody] ExamLoginPrivilegesReportDTO data)
        {
            return _inter.onselectclass(data);
        }
        [Route("onchangesection")]
        public ExamLoginPrivilegesReportDTO onchangesection([FromBody] ExamLoginPrivilegesReportDTO data)
        {
            return _inter.onchangesection(data);
        }

        [Route("onreport")]
        public ExamLoginPrivilegesReportDTO onreport([FromBody] ExamLoginPrivilegesReportDTO data)
        {
            return _inter.onreport(data);
        }
              
    }
}
