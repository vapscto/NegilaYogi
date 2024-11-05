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
    public class VikasaLUController : Controller
    {
        VikasaLUDelegates _lureport = new VikasaLUDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public VikasaLUReportDTO getdetails(VikasaLUReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _lureport.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        [Route("onselectAcdYear")]
        public VikasaLUReportDTO onselectAcdYear([FromBody]VikasaLUReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _lureport.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public VikasaLUReportDTO onselectclass([FromBody]VikasaLUReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _lureport.onselectclass(data);
        }

        [Route("onselectSection")]
        public VikasaLUReportDTO onselectSection([FromBody]VikasaLUReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _lureport.onselectSection(data);
        }

        [Route("onreport")]
        public VikasaLUReportDTO onreport([FromBody]VikasaLUReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _lureport.onreport(data);
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
