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
    public class StudentPerformanceReportController : Controller
    {
        StudentPerformanceReportDelegates _delobj = new StudentPerformanceReportDelegates();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public StudentPerformanceReportDTO getdetails(StudentPerformanceReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }


        // POST api/values
        [HttpPost]

        [Route("onselectCategory")]
        public StudentPerformanceReportDTO onselectCategory([FromBody]StudentPerformanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectCategory(data);
        }

        [Route("onselectclass")]
        public StudentPerformanceReportDTO onselectclass([FromBody]StudentPerformanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }
        [Route("onselectSection")]
        public StudentPerformanceReportDTO onselectSection([FromBody]StudentPerformanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectSection(data);
        }


        [Route("onshow")]
        public StudentPerformanceReportDTO onshow([FromBody]StudentPerformanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onshow(data);
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
