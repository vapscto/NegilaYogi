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
    public class CollegeCumulativeAvgBestReportController : Controller
    {
        CollegeCumulativeAvgBestReportDelegate _delg = new CollegeCumulativeAvgBestReportDelegate();
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
        public CollegeCumulativeAvgBestReportDTO Getdetails(int id)
        {
            CollegeCumulativeAvgBestReportDTO data = new CollegeCumulativeAvgBestReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Getdetails(data);
        }
        [Route("onchangeyear")]
        public CollegeCumulativeAvgBestReportDTO onchangeyear([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));           
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeCumulativeAvgBestReportDTO onchangecourse([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeCumulativeAvgBestReportDTO onchangebranch([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeCumulativeAvgBestReportDTO onchangesemester([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onchangesemester(data);
        }
        [Route("onchangesubjectscheme")]
        public CollegeCumulativeAvgBestReportDTO onchangesubjectscheme([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onchangesubjectscheme(data);
        }
        [Route("onchangeschemetype")]
        public CollegeCumulativeAvgBestReportDTO onchangeschemetype([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onchangeschemetype(data);
        }
        [Route("Getcmreport")]
        public CollegeCumulativeAvgBestReportDTO Getcmreport([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Getcmreport(data);
        }
        [Route("getindreport")]
        public CollegeCumulativeAvgBestReportDTO getindreport([FromBody] CollegeCumulativeAvgBestReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getindreport(data);
        }
        
    }
}
