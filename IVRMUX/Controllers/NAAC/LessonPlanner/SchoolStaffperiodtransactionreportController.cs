using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class SchoolStaffperiodtransactionreportController : Controller
    {
        public SchoolStaffperiodtransactionreportDelegate _delg = new SchoolStaffperiodtransactionreportDelegate();
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
        public SchoolStaffperiodtransactionreportDTO Getdetailstransaction(int id)
        {
            SchoolStaffperiodtransactionreportDTO data = new SchoolStaffperiodtransactionreportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetailstransaction(data);
        }

        [Route("onselectAcdYear")]
        public SchoolStaffperiodtransactionreportDTO onselectAcdYear([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public SchoolStaffperiodtransactionreportDTO onselectclass([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onselectclass(data);
        }
        [Route("onchangesection")]
        public SchoolStaffperiodtransactionreportDTO onchangesection([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesection(data);
        }
        [Route("getreport")]
        public SchoolStaffperiodtransactionreportDTO getreport([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getreport(data);
        }
        [Route("getdevationreport")]
        public SchoolStaffperiodtransactionreportDTO getdevationreport([FromBody] SchoolStaffperiodtransactionreportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getdevationreport(data);
        }


    }
}
