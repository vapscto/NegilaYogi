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
    public class statewisestudentadmissionFacadeController : Controller
    {
        private statewisestudentadmissionInterface _inter;

        public statewisestudentadmissionFacadeController(statewisestudentadmissionInterface obj)
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
        [Route("onselectAcdYear")]
        public statewisestudentadmissionDTO onselectAcdYear([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public statewisestudentadmissionDTO onselectCourse([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.onselectCourse(data);
        }

        [Route("onselectBranch")]
        public statewisestudentadmissionDTO onselectBranch([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.onselectBranch(data);
        }

        [Route("getdetails")]
        public statewisestudentadmissionDTO Getdetails([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onreport")]
        public Task<statewisestudentadmissionDTO> onreport([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.onreport(data);
        }
        [Route("onreportcountry")]
        public Task<statewisestudentadmissionDTO> onreportcountry([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.onreportcountry(data);
        }
        [Route("onreportreligionruralurban")]
        public Task<statewisestudentadmissionDTO> onreportreligionruralurban([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.onreportreligionruralurban(data);
        }
        [Route("CategoryCasteWiseStudentReport")]
        public Task<statewisestudentadmissionDTO> CategoryCasteWiseStudentReport([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.CategoryCasteWiseStudentReport(data);
        }
        [Route("onreportbirthday")]
        public Task<statewisestudentadmissionDTO> onreportbirthday([FromBody] statewisestudentadmissionDTO data)
        {
            return _inter.onreportbirthday(data);
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
