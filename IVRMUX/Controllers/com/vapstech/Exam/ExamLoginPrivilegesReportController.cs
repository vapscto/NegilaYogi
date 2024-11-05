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
    public class ExamLoginPrivilegesReportController : Controller
    {
        ExamLoginPrivilegesReportDelegates _loginreport = new ExamLoginPrivilegesReportDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [HttpGet]
        [Route("getdetails")]
        public ExamLoginPrivilegesReportDTO Get([FromQuery] int id)
        {
            ExamLoginPrivilegesReportDTO data = new ExamLoginPrivilegesReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _loginreport.getdetails(data);
        }
        
        [HttpPost]
        [Route("onselectAcdYear")]
        public ExamLoginPrivilegesReportDTO onselectAcdYear([FromBody]ExamLoginPrivilegesReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _loginreport.onselectAcdYear(data);
        }

        [Route("onchangecategory")]
        public ExamLoginPrivilegesReportDTO onchangecategory([FromBody]ExamLoginPrivilegesReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _loginreport.onchangecategory(data);
        }

        [Route("onselectclass")]
        public ExamLoginPrivilegesReportDTO onselectclass([FromBody]ExamLoginPrivilegesReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _loginreport.onselectclass(data);
        }

        [Route("onchangesection")]
        public ExamLoginPrivilegesReportDTO onchangesection([FromBody]ExamLoginPrivilegesReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _loginreport.onchangesection(data);
        }

        [Route("onreport")]
        public ExamLoginPrivilegesReportDTO onreport([FromBody]ExamLoginPrivilegesReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _loginreport.onreport(data);
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
