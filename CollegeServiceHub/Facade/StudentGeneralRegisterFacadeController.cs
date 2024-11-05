using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class StudentGeneralRegisterFacadeController : Controller
    {
        private StudentGeneralRegisterInterface _inter;

        public StudentGeneralRegisterFacadeController (StudentGeneralRegisterInterface obj)
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
        public StudentGeneralRegisterDTO onselectAcdYear([FromBody] StudentGeneralRegisterDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public StudentGeneralRegisterDTO onselectCourse([FromBody] StudentGeneralRegisterDTO data)
        {
            return _inter.onselectCourse(data);
        }

        [Route("onselectBranch")]
        public StudentGeneralRegisterDTO onselectBranch([FromBody] StudentGeneralRegisterDTO data)
        {
            return _inter.onselectBranch(data);
        }

        [Route("getdetails")]
        public StudentGeneralRegisterDTO Getdetails([FromBody] StudentGeneralRegisterDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onreport")]
        public Task<StudentGeneralRegisterDTO> onreport([FromBody] StudentGeneralRegisterDTO data)
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
