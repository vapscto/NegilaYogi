using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class MarksEntryReportController : Controller
    {
        MarksEntryReportDelegate delg = new MarksEntryReportDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("Getdetails/{id:int}")]
        public MarksEntryReportDTO Getdetails (int id)
        {
            MarksEntryReportDTO data = new MarksEntryReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delg.Getdetails(data);
        }

        [Route("get_class")]
        public MarksEntryReportDTO get_class([FromBody] MarksEntryReportDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delg.get_class(data);
        }
        [Route("get_section")]
        public MarksEntryReportDTO get_section([FromBody] MarksEntryReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delg.get_section(data);
        }
        [Route("get_exam")]
        public MarksEntryReportDTO get_exam([FromBody] MarksEntryReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delg.get_exam(data);
        }

        [Route("get_report")]
        public MarksEntryReportDTO get_report([FromBody] MarksEntryReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delg.get_report(data);
        }

        [Route("get_markspublishreport")]
        public MarksEntryReportDTO get_markspublishreport([FromBody] MarksEntryReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delg.get_markspublishreport(data);
        }
    }
}
