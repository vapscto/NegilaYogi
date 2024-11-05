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
    public class StudentPerformanceReportFacadeController : Controller
    {
        private StudentPerformanceReportInterface _inter;

        public StudentPerformanceReportFacadeController(StudentPerformanceReportInterface obj)
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
        public StudentPerformanceReportDTO Getdetails([FromBody] StudentPerformanceReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onselectCategory")]
        public StudentPerformanceReportDTO onselectCategory([FromBody] StudentPerformanceReportDTO data)
        {
            return _inter.onselectCategory(data);
        }

        [Route("onselectclass")]
        public StudentPerformanceReportDTO onselectclass([FromBody] StudentPerformanceReportDTO data)
        {
            return _inter.onselectclass(data);
        }
        [Route("onselectSection")]
        public StudentPerformanceReportDTO onselectSection([FromBody] StudentPerformanceReportDTO data)
        {
            return _inter.onselectSection(data);
        }

        [Route("onshow")]
        public StudentPerformanceReportDTO onshow([FromBody] StudentPerformanceReportDTO data)
        {
            return _inter.onshow(data);
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
