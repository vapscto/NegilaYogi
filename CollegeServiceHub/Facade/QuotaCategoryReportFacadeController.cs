using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class QuotaCategoryReportFacadeController : Controller
    {
        public QuotaCategoryReportInterface _inter;

        public QuotaCategoryReportFacadeController (QuotaCategoryReportInterface obj)
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
        public QuotaCategoryReportDTO getdetails([FromBody] QuotaCategoryReportDTO data)
        {
            return _inter.getdetails(data);
        }
        [Route("onselectAcdYear")]
        public QuotaCategoryReportDTO onselectAcdYear([FromBody] QuotaCategoryReportDTO data)
        {
            return _inter.onselectAcdYear(data);
        }
        [Route("onselectCourse")]
        public QuotaCategoryReportDTO onselectCourse([FromBody] QuotaCategoryReportDTO data)
        {
            return _inter.onselectCourse(data);
        }
        [Route("onselectBranch")]
        public QuotaCategoryReportDTO onselectBranch([FromBody] QuotaCategoryReportDTO data)
        {
            return _inter.onselectBranch(data);
        }
        

        [Route("onreport")]
        public QuotaCategoryReportDTO onreport([FromBody] QuotaCategoryReportDTO data)
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
