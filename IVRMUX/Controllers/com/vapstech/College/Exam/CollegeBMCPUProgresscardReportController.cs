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
    public class CollegeBMCPUProgresscardReportController : Controller
    {
        public CollegeBMCPUProgresscardReportDelegate _delg = new CollegeBMCPUProgresscardReportDelegate();
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
        public CollegeBMCPUProgresscardReportDTO Getdetails(int id)
        {
            CollegeBMCPUProgresscardReportDTO data = new CollegeBMCPUProgresscardReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }

        [Route("OnAcdyear")]
        public CollegeBMCPUProgresscardReportDTO OnAcdyear(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.OnAcdyear(data);
        }
        [Route("onchangecourse")]
        public CollegeBMCPUProgresscardReportDTO onchangecourse(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeBMCPUProgresscardReportDTO onchangebranch(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeBMCPUProgresscardReportDTO onchangesemester(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesemester(data);
        }
        [Route("onchangesection")]
        public CollegeBMCPUProgresscardReportDTO onchangesection(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesection(data);
        }

        [Route("onchangesubjectscheme")]
        public CollegeBMCPUProgresscardReportDTO onchangesubjectscheme(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesubjectscheme(data);
        }
        [Route("onchangeschemetype")]
        public CollegeBMCPUProgresscardReportDTO onchangeschemetype(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeschemetype(data);
        }       

        [Route("getreport")]
        public CollegeBMCPUProgresscardReportDTO getreport(CollegeBMCPUProgresscardReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getreport(data);
        }
    }
}
