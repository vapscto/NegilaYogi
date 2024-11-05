using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MarksReportController : Controller
    {

        MarksReportDelegate SCR = new MarksReportDelegate();
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
        public MarksReportDTO getdetails (int id)
        {
            MarksReportDTO data = new MarksReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.mid = mid;
            int yearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.yearid = yearid;
            return SCR.getdeatils(data);
        }

        [Route("Getreportdetails")]
        public MarksReportDTO Getreportdetails([FromBody]MarksReportDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mid = mid;
            int yearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            rep.yearid = yearid;
            return SCR.Getreportdetails(rep);
        }
        [Route("Getreportdetailssrkvs")]
        public MarksReportDTO Getreportdetailssrkvs([FromBody]MarksReportDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mid = mid;
            int yearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            rep.yearid = yearid;
            return SCR.Getreportdetailssrkvs(rep);
        }
        [HttpPost]
        [Route("schedulelist")]
        public MarksReportDTO schedulelist([FromBody] MarksReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.mid = mid;

            int yearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.yearid = yearid;


            return SCR.schedulelist(data);
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
