using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class BBKVCustomReportController : Controller
    {
        BBKVCustomReportDelegate _tcreport = new BBKVCustomReportDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails/{id:int}")]
        public BBKVCustomReportDTO getdetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.getdetails(id);
        }

        [Route("getnameregno")]
        public BBKVCustomReportDTO getnameregno([FromBody] BBKVCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.getnameregno(data);
        }

        [Route("stdnamechange")]
        public BBKVCustomReportDTO stdnamechange([FromBody]BBKVCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.stdnamechange(data);
        }
        [Route("onclicktcperortemo")]
        public BBKVCustomReportDTO onclicktcperortemo([FromBody] BBKVCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.onclicktcperortemo(data);
        }

        [Route("getTcdetails")]
        public BBKVCustomReportDTO getTcdetails([FromBody] BBKVCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.getTcdetails(data);
        }
        [Route("getTcdetailsJNS")]
        public BBKVCustomReportDTO getTcdetailsJNS([FromBody] BBKVCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.getTcdetailsJNS(data);
        }
        [Route("get_JSHSTcdetails")]
        public BBKVCustomReportDTO get_JSHSTcdetails([FromBody] BBKVCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.get_JSHSTcdetails(data);
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
