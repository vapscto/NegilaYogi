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
    public class PercentagewiseDetailsReportController : Controller
    {
        PercentagewiseDetailsReportDelegates _delobj = new PercentagewiseDetailsReportDelegates();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public PercentagewiseDetailsReportDTO getdetails(PercentagewiseDetailsReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        
        [HttpPost]

        [Route("onselectAcdYear")]
        public PercentagewiseDetailsReportDTO onselectAcdYear([FromBody]PercentagewiseDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectCategory")]
        public PercentagewiseDetailsReportDTO onselectCategory([FromBody]PercentagewiseDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectCategory(data);
        }

        [Route("onselectclass")]
        public PercentagewiseDetailsReportDTO onselectclass([FromBody]PercentagewiseDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }

        [Route("onselectSection")]
        public PercentagewiseDetailsReportDTO onselectSection([FromBody]PercentagewiseDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectSection(data);
        }
        [Route("onreport")]
        public PercentagewiseDetailsReportDTO onreport([FromBody]PercentagewiseDetailsReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreport(data);
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
