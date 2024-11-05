using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HHSTCCustomReportController : Controller
    {
        HHSTCCustomReportDelegate _tcreport = new HHSTCCustomReportDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails/{id:int}")]
        public HHSTCCustomReportDTO getdetails(int id)
        {
           id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.getdetails(id);
        }

        [Route("getnameregno")]
        public HHSTCCustomReportDTO getnameregno([FromBody] HHSTCCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.getnameregno(data);
        }

        [Route("stdnamechange")]
        public HHSTCCustomReportDTO stdnamechange([FromBody]HHSTCCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.stdnamechange(data);
        }
        [Route("onclicktcperortemo")]
        public HHSTCCustomReportDTO onclicktcperortemo([FromBody] HHSTCCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.onclicktcperortemo(data);
        }

        [Route("getTcdetails")]
        public HHSTCCustomReportDTO getTcdetails([FromBody] HHSTCCustomReportDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.getTcdetails(data);
        }
        [Route("Vikasha_getTcdetails")]
        public HHSTCCustomReportDTO Vikasha_getTcdetails([FromBody] HHSTCCustomReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _tcreport.Vikasha_getTcdetails(data);
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
