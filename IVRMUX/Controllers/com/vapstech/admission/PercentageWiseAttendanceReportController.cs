using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class PercentageWiseAttendanceReportController : Controller
    {
        PercentageWiseAttendanceReportDelegate _delg = new PercentageWiseAttendanceReportDelegate();

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

        [Route("getloaddata/{id:int}")]
        public PercentageWiseAttendanceReportDTO getloaddata(int id)
        {
            PercentageWiseAttendanceReportDTO data = new PercentageWiseAttendanceReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getloaddata(data);
        }

        [Route("getclass")]
        public PercentageWiseAttendanceReportDTO getclass([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getclass(data);
        }
        [Route("getsection")]
        public PercentageWiseAttendanceReportDTO getsection([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getsection(data);
        }

        [Route("showreport")]
        public PercentageWiseAttendanceReportDTO showreport([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.showreport(data);
        }

        [Route("SendAttendanceSMS")]
        public PercentageWiseAttendanceReportDTO SendAttendanceSMS([FromBody]  PercentageWiseAttendanceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SendAttendanceSMS(data);
        }
    }
}
