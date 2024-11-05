using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeStudentTCReportController : Controller
    {
        public CollegeStudentTCReportDelegate _delg = new CollegeStudentTCReportDelegate();
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

        [Route("getalldetails/{id:long}")]
        public CollegeStudentTCReportDTO getalldetails (long id)
        {
            CollegeStudentTCReportDTO data = new CollegeStudentTCReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getalldetails(data);
        }
        [Route("onchangeyear")]
        public CollegeStudentTCReportDTO onchangeyear([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeStudentTCReportDTO onchangecourse([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStudentTCReportDTO onchangebranch([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeStudentTCReportDTO onchangesemester([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesemester(data);
        }
        [Route("Getreportdetails")]
        public CollegeStudentTCReportDTO Getreportdetails([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getreportdetails(data);
        }

        // TC Custom Report
        [Route("onchangeyeartc")]
        public CollegeStudentTCReportDTO onchangeyeartc([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeyeartc(data);
        }
        [Route("stdnamechange")]
        public CollegeStudentTCReportDTO stdnamechange([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.stdnamechange(data);
        }
        [Route("getTcdetails")]
        public CollegeStudentTCReportDTO getTcdetails([FromBody] CollegeStudentTCReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getTcdetails(data);
        }       

    }
}
