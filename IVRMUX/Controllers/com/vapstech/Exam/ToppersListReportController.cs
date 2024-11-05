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
    public class ToppersListReportController : Controller
    {
        ToppersListReportDelegates _delobj = new ToppersListReportDelegates();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ToppersListReportDTO getdetails(ToppersListReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("onselectCategory")]
        public ToppersListReportDTO onselectCategory([FromBody]ToppersListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectCategory(data);
        }

        [Route("onselectclass")]
        public ToppersListReportDTO onselectclass([FromBody]ToppersListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }

        [Route("onreport")]
        public ToppersListReportDTO onreport([FromBody]ToppersListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreport(data);
        }
        [Route("get_sec_exam")]
        public ToppersListReportDTO get_sec_exam([FromBody]ToppersListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.get_sec_exam(data);
        }
        [Route("onselectexam")]
        public ToppersListReportDTO onselectexam([FromBody]ToppersListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectexam(data);
        }
        [Route("get_subject")]
        public ToppersListReportDTO get_subject([FromBody]ToppersListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.get_subject(data);
        }

        [Route("sendsms")]
        public ToppersListReportDTO sendsms([FromBody]ToppersListReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.sendsms(data);
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
