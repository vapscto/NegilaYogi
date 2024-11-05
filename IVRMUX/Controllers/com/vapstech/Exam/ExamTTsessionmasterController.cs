using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class ExamTTsessionmasterController : Controller
    {
        ExamTTsessionmasterDelegate _exmtt = new ExamTTsessionmasterDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails/{id:int}")]
        public ExamTTsessionmasterDTO Getdetails(int id)
        {
            ExamTTsessionmasterDTO data = new ExamTTsessionmasterDTO();
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmtt.Getdetails(data);
        }

        [Route("savedetails")]
        public ExamTTsessionmasterDTO savedetails([FromBody]ExamTTsessionmasterDTO data)
        {
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmtt.savedetails(data);
        }

        [Route("editdetails")]
        public ExamTTsessionmasterDTO editdetails([FromBody]ExamTTsessionmasterDTO data)
        {
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmtt.editdetails(data);
        }

        [Route("deactivate")]
        public ExamTTsessionmasterDTO deactivate([FromBody]ExamTTsessionmasterDTO data)
        {
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _exmtt.deactivate(data);
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
