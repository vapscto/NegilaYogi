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
    public class ScheduleReportController : Controller
    {

        ScheduleReportDelegate SCR = new ScheduleReportDelegate();
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
        public ScheduleReportDTO getdetails (int id)
        {
            ScheduleReportDTO data = new ScheduleReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.mid = mid;
            return SCR.getdeatils(data);
        }

        [Route("Getreportdetails")]
        public ScheduleReportDTO Getreportdetails([FromBody]ScheduleReportDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mid = mid;

            return SCR.Getreportdetails(rep);
        }

        [Route("scheduleGetreportdetails")]
        public ScheduleReportDTO scheduleGetreportdetails([FromBody]ScheduleReportDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mid = mid;

            return SCR.scheduleGetreportdetails(rep);
        }
        [Route("remarksGetreportdetails")]
        public ScheduleReportDTO remarksGetreportdetails([FromBody]ScheduleReportDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mid = mid;

            return SCR.remarksGetreportdetails(rep);
        }

        [Route("schedulelist")]
        public ScheduleReportDTO schedulelist([FromBody] ScheduleReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.mid = mid;

            int yearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.yearid = yearid;


            return SCR.schedulelist(data);
        }

        [Route("SendMail/")]
        public string SendMail([FromBody] ScheduleReportDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mid = mid;
            return SCR.SendMail(MMD);
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
