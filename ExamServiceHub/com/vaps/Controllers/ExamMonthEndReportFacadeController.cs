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
    public class ExamMonthEndReportFacadeController : Controller
    {
        private ExamMonthEndReportInterface _inter;

        public ExamMonthEndReportFacadeController (ExamMonthEndReportInterface obj)
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
        public ExamMonthEndReportDTO Getdetails([FromBody] ExamMonthEndReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onreport")]
        public ExamMonthEndReportDTO onreport([FromBody] ExamMonthEndReportDTO data)
        {
            return _inter.onreport(data);
        }
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
