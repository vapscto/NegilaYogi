using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class Baldwin_Subj_G_F_ReportController : Controller
    {
        Baldwin_Subj_G_F_ReportDelegates del_fr = new Baldwin_Subj_G_F_ReportDelegates();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public Baldwin_Subj_G_F_ReportDTO Getdetails(Baldwin_Subj_G_F_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.Getdetails(data);
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
        [Route("get_classes")]
        public Baldwin_Subj_G_F_ReportDTO get_classes([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_classes(data);
        }
        [Route("get_sections")]
        public Baldwin_Subj_G_F_ReportDTO get_sections([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_sections(data);
        }
        [Route("get_students")]
        public Baldwin_Subj_G_F_ReportDTO get_students([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_students(data);
        }
        [Route("get_report")]
        public Baldwin_Subj_G_F_ReportDTO get_report([FromBody] Baldwin_Subj_G_F_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_report(data);
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
