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
    public class ExamTTSmsEmailController : Controller
    {
        ExamTTSmsEmailDelegates _delobj = new ExamTTSmsEmailDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ExamTTSmsEmailDTO getdetails(ExamTTSmsEmailDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        // POST api/values
        [HttpPost]

        [Route("onselectAcdYear")]
        public ExamTTSmsEmailDTO onselectAcdYear([FromBody]ExamTTSmsEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public ExamTTSmsEmailDTO onselectclass([FromBody]ExamTTSmsEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }

        [Route("onselectSection")]
        public ExamTTSmsEmailDTO onselectSection([FromBody]ExamTTSmsEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectSection(data);
        }

        [Route("getStudentsTeachers")]
        public ExamTTSmsEmailDTO getStudentsTeachers([FromBody]ExamTTSmsEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getStudentsTeachers(data);
        }

        [Route("generate")]
        public ExamTTSmsEmailDTO generate([FromBody]ExamTTSmsEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.generate(data);
        }
       
        [Route("sendmail")]
        public ExamTTSmsEmailDTO sendmail([FromBody]ExamTTSmsEmailDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.sendmail(data);
        }
        [Route("getsubjectgroup")]
        public ExamTTSmsEmailDTO getsubjectgroup([FromBody]ExamTTSmsEmailDTO data)
        {          

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));          

            return _delobj.getsubjectgroup(data);
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
