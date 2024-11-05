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
    public class ExamTTSmsEmailFacadeController : Controller
    {
        private ExamTTSmsEmailInterface _inter;

        public ExamTTSmsEmailFacadeController (ExamTTSmsEmailInterface obj)
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
        public ExamTTSmsEmailDTO Getdetails([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onselectAcdYear")]
        public ExamTTSmsEmailDTO onselectAcdYear([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public ExamTTSmsEmailDTO onselectclass([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.onselectclass(data);
        }

        [Route("onselectSection")]
        public ExamTTSmsEmailDTO onselectSection([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.onselectSection(data);
        }

        [Route("getStudentsTeachers")]
        public ExamTTSmsEmailDTO getStudentsTeachers([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.getStudentsTeachers(data);
        }

        [Route("generate")]
        public ExamTTSmsEmailDTO generate([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.generate(data);
        }

        [Route("sendmail")]
        public ExamTTSmsEmailDTO sendmail([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.sendmail(data);
        }
        [Route("getsubjectgroup")]
        public ExamTTSmsEmailDTO getsubjectgroup([FromBody] ExamTTSmsEmailDTO data)
        {
            return _inter.getsubjectgroup(data);
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
