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
    public class ExamTTsessionmasterFacadeController : Controller
    {
        ExamTTsessionmasterInterface _exmttint;
        public ExamTTsessionmasterFacadeController(ExamTTsessionmasterInterface inter)
        {
            _exmttint = inter;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public ExamTTsessionmasterDTO Getdetails([FromBody] ExamTTsessionmasterDTO data)
        {
            return _exmttint.Getdetails(data);
        }
        [Route("savedetails")]
        public ExamTTsessionmasterDTO savedetails([FromBody] ExamTTsessionmasterDTO data)
        {
            return _exmttint.savedetails(data);
        }
        [Route("editdetails")]
        public ExamTTsessionmasterDTO editdetails([FromBody] ExamTTsessionmasterDTO data)
        {
            return _exmttint.editdetails(data);
        }
        [Route("deactivate")]
        public ExamTTsessionmasterDTO deactivate([FromBody] ExamTTsessionmasterDTO data)
        {
            return _exmttint.deactivate(data);
        }
        

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
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
