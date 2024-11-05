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
    [Route("api/[controller]")]
    public class DocumentViewReportAdmController : Controller
    {
        public DocumentViewReportAdmDelegate _report = new DocumentViewReportAdmDelegate();
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

        [Route("getdetails/{id:int}")]
        public DocumentViewReportAdmDTO getdetails (int id)
        {
            id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _report.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("getstudent")]
        public DocumentViewReportAdmDTO getstudent([FromBody] DocumentViewReportAdmDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _report.getstudent(data);
        }
        [Route("getreport")]
        public DocumentViewReportAdmDTO getreport([FromBody] DocumentViewReportAdmDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _report.getreport(data);
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
