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
    public class CollegeStudyCertificateReportController : Controller
    {
        public CollegeStudyCertificateReportDelegate _delg = new CollegeStudyCertificateReportDelegate();
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

        [Route("getdata/{id:int}")]
        public CollegeStudyCertificateReportDTO getdata(int id)
        {
            CollegeStudyCertificateReportDTO data = new CollegeStudyCertificateReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getdata(data);
        }
        [Route("onchangeyear")]
        public CollegeStudyCertificateReportDTO onchangeyear([FromBody] CollegeStudyCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeStudyCertificateReportDTO onchangecourse([FromBody] CollegeStudyCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStudyCertificateReportDTO onchangebranch([FromBody] CollegeStudyCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeStudyCertificateReportDTO onchangesemester([FromBody] CollegeStudyCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesemester(data);
        }

        [Route("searchfilter")]
        public CollegeStudyCertificateReportDTO searchfilter([FromBody] CollegeStudyCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.searchfilter(data);
        }
        [Route("onclickreport")]
        public CollegeStudyCertificateReportDTO onclickreport([FromBody] CollegeStudyCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onclickreport(data);
        }        
    }
}
