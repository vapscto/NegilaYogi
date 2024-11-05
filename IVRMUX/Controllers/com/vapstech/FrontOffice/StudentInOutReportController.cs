using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [Route("api/[controller]")]
    public class StudentInOutReportController : Controller
    {
        StudentInOutReportDelegate _delg = new StudentInOutReportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [Route("loaddata/{id:int}")]
        public StudentInOutReportDTO loaddata(int id)
        {
            StudentInOutReportDTO data = new StudentInOutReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.loaddata(data);
        }
        //getsection
        [Route("getsection")]
        public StudentInOutReportDTO getsection([FromBody] StudentInOutReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getsection(data);
        }
        //getstudent
        [Route("getstudent")]
        public StudentInOutReportDTO getstudent([FromBody] StudentInOutReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getstudent(data);
        }
        //report
        [Route("report")]
        public StudentInOutReportDTO report([FromBody] StudentInOutReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.report(data);
        }
    }
}
