using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgCumulativeReportController : Controller
    {
        ClgCumulativeReportDelegates crStr = new ClgCumulativeReportDelegates();
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
        [Route("Getdetails")]
        public ClgCumulativeReportDTO Getdetails(ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }

        [Route("onchangeyear")]
        public ClgCumulativeReportDTO onchangeyear([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public ClgCumulativeReportDTO onchangecourse([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public ClgCumulativeReportDTO onchangebranch([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public ClgCumulativeReportDTO onchangesemester([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangesemester(data);
        }
        [Route("onchangesubjectscheme")]
        public ClgCumulativeReportDTO onchangesubjectscheme([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangesubjectscheme(data);
        }
        [Route("onchangeschemetype")]
        public ClgCumulativeReportDTO onchangeschemetype([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangeschemetype(data);
        }

        [Route("Getcmreport")]
        public ClgCumulativeReportDTO Getcmreport([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getcmreport(data);
        }

        [Route("GetCumulativeReportFormat2")]
        public ClgCumulativeReportDTO GetCumulativeReportFormat2([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.GetCumulativeReportFormat2(data);
        }

        [Route("GetProgresscardReport")]
        public ClgCumulativeReportDTO GetProgresscardReport([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.GetProgresscardReport(data);
        }

        [Route("GetJNUProgressCardReport1")]
        public ClgCumulativeReportDTO GetJNUProgressCardReport1([FromBody] ClgCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.GetJNUProgressCardReport1(data);
        }
        
    }
}
