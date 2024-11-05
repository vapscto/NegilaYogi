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
    public class OverallDailyAttendanceReportController : Controller
    {
        OverallDailyAttendanceReportDelegate sad = new OverallDailyAttendanceReportDelegate();
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
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
           // int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return sad.getinitialdata(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getAttendetails")]
        public StudentAttendanceReportDTO getAttendetails([FromBody] StudentAttendanceReportDTO data)
        {
           // int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getserdata(data);
        }
        [Route("getoveallattendance")]
        public StudentAttendanceReportDTO getoveallattendance([FromBody] StudentAttendanceReportDTO data)
        {
           // int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getoveallattendance(data);
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
        [Route("getStudentDetails")]
        public StudentAttendanceReportDTO getStudents([FromBody]StudentAttendanceReportDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.miid = mi_id;
            return  sad.getstudentDet(dto);
           
        }

        //getStudentAllDetails
        [Route("getStudentAllDetails")]
        public StudentAttendanceReportDTO getStudentAllDetails([FromBody]StudentAttendanceReportDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.miid = mi_id;
            return sad.getStudentAllDetails(dto);

        }

    }
}
