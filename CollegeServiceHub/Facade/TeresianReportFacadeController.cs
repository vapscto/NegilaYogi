using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class TeresianReportFacadeController : Controller
    {
        private TeresianReportInterface _inter;

        public TeresianReportFacadeController(TeresianReportInterface obj)
        {
            _inter = obj;
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


        [HttpPost]
        [Route("onselectAcdYear")]
        public TeresianReportDTO onselectAcdYear([FromBody] TeresianReportDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public TeresianReportDTO onselectCourse([FromBody] TeresianReportDTO data)
        {
            return _inter.onselectCourse(data);
        }

        [Route("onselectBranch")]
        public TeresianReportDTO onselectBranch([FromBody] TeresianReportDTO data)
        {
            return _inter.onselectBranch(data);
        }

        [Route("getdetails")]
        public TeresianReportDTO Getdetails([FromBody] TeresianReportDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onreport")]
        public Task<TeresianReportDTO> onreport([FromBody] TeresianReportDTO data)
        {
            return _inter.onreport(data);
        }
        [Route("onselectcategory")]
        public TeresianReportDTO onreponselectcategoryort([FromBody] TeresianReportDTO data)
        {
            return _inter.onselectcategory(data);
        }
        


        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
