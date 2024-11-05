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
    public class CollegeDocumentReportController : Controller
    {
        CollegeDocumentReportDelegate _delg = new CollegeDocumentReportDelegate();
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

        [Route("getdetails/{id:int}")]
        public CollegeDocumentReportDTO getdetails(int id)
        {
            CollegeDocumentReportDTO data = new CollegeDocumentReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getdetails(data);
        }

        [Route("onchangeyear")]
        public CollegeDocumentReportDTO onchangeyear([FromBody] CollegeDocumentReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeyear(data);
        }

        [Route("onchangecourse")]
        public CollegeDocumentReportDTO onchangecourse([FromBody] CollegeDocumentReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangecourse(data);
        }

        [Route("onchangebranch")]
        public CollegeDocumentReportDTO onchangebranch([FromBody] CollegeDocumentReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangebranch(data);
        }

        [Route("onchangesemester")]
        public CollegeDocumentReportDTO onchangesemester([FromBody] CollegeDocumentReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesemester(data);
        }

        [Route("onchangesection")]
        public CollegeDocumentReportDTO onchangesection([FromBody] CollegeDocumentReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesection(data);
        }

        [Route("getreportdetails")]
        public CollegeDocumentReportDTO getreportdetails([FromBody] CollegeDocumentReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getreportdetails(data);
        }

        //documeent view
        [HttpGet]
        [Route("getdetails_view/{id:int}")]
        public CollegeDocumentReportDTO getdetails_view(int id)
        {
            CollegeDocumentReportDTO data = new CollegeDocumentReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getdetails_view(data);
        }


        [HttpPost]
        [Route("getclgstudata_view")]
        public CollegeDocumentReportDTO getclgstudata([FromBody] CollegeDocumentReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delg.getclgstudata_view(data);
        }
    }
}
