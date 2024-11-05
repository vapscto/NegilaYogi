using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class SiblingEmployeeStudentReportController : Controller
    {
        SiblingEmployeeStudentReportDelegate _dleg = new SiblingEmployeeStudentReportDelegate();
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
        public SiblingEmployeeStudentReportDTO getdetails(int id)
        {
            SiblingEmployeeStudentReportDTO data = new SiblingEmployeeStudentReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _dleg.getdetails(data);
        }

        [Route("getreport")]
        public SiblingEmployeeStudentReportDTO getreport([FromBody]SiblingEmployeeStudentReportDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _dleg.getreport(data);
        }
    }
}
