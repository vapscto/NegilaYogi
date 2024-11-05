using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AttendanceReportController : Controller
    {
        AttendanceReportDelegate attdel = new AttendanceReportDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("getdetails")]
        public StudentAttendanceReportDTO getdetails()
        {
            StudentAttendanceReportDTO data = new StudentAttendanceReportDTO();
            //    int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return attdel.getinitialdata(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getAttendetails")]
        public StudentAttendanceReportDTO getAttendetails([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
          //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return attdel.getserdata(data);
        }
        [Route("getsection")]
        public StudentAttendanceReportDTO getsection([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return attdel.getsection(data);
        }

        [Route("getclass")]
        public StudentAttendanceReportDTO getclass([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return attdel.getclass(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
