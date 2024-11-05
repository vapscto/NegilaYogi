using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class CollegeStaffperiodtransactionreportController : Controller
    {
        public CollegeStaffperiodtransactionreportDelegate _delg = new CollegeStaffperiodtransactionreportDelegate();


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

        [Route("Getdetailstransaction/{id:int}")]
        public CollegeStaffperiodtransactionreportDTO Getdetailstransaction(int id)
        {
            CollegeStaffperiodtransactionreportDTO data = new CollegeStaffperiodtransactionreportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetailstransaction(data);
        }

        [Route("onselectAcdYear")]
        public CollegeStaffperiodtransactionreportDTO onselectAcdYear([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public CollegeStaffperiodtransactionreportDTO onselectCourse([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onselectCourse(data);
        }
        [Route("onselectBranch")]
        public CollegeStaffperiodtransactionreportDTO onselectBranch([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onselectBranch(data);
        }
        [Route("getsection")]
        public CollegeStaffperiodtransactionreportDTO getsection([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getsection(data);
        }

        [Route("onchangesection")]
        public CollegeStaffperiodtransactionreportDTO onchangesection([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesection(data);
        }
        [Route("getreport")]
        public CollegeStaffperiodtransactionreportDTO getreport([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getreport(data);
        }
        [Route("getdevationreport")]
        public CollegeStaffperiodtransactionreportDTO getdevationreport([FromBody] CollegeStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getdevationreport(data);
        }
        

    }
}
