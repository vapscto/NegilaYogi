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
    public class CollegeDailyAttendanceController : Controller
    {
        CollegeDailyAttendanceDelegate _delobj = new CollegeDailyAttendanceDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public CollegeDailyAttendanceDTO getdetails(CollegeDailyAttendanceDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        [Route("onreport")]
        public CollegeDailyAttendanceDTO onreport([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreport(data);
        }

        [Route("onselectAcdYear")]
        public CollegeDailyAttendanceDTO onselectAcdYear([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));            
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Flag1 = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectCourse")]
        public CollegeDailyAttendanceDTO onselectCourse([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Flag1 = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delobj.onselectCourse(data);
        }

        [Route("onselectBranch")]
        public CollegeDailyAttendanceDTO onselectBranch([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Flag1 = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delobj.onselectBranch(data);
        }

        [Route("getsection")]
        public CollegeDailyAttendanceDTO getsection([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Flag1 = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delobj.getsection(data);
        }

        [Route("getsubject")]
        public CollegeDailyAttendanceDTO getsubject([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Flag1 = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delobj.getsubject(data);
        }
       

        [Route("onreportpercentage")]
        public CollegeDailyAttendanceDTO onreportpercentage([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreportpercentage(data);
        }
        [Route("getAttendetails")]
        public CollegeDailyAttendanceDTO getAttendetails([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getAttendetails(data);
        }
        [Route("GetAttendancedetails")]
        public CollegeDailyAttendanceDTO GetAttendancedetails([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));


            //data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
             data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
           
            return _delobj.GetAttendancedetails(data);
        }
        [Route("getStudentAbsentDetails")]
        public CollegeDailyAttendanceDTO getStudentAbsentDetails([FromBody]CollegeDailyAttendanceDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.MI_Id = mi_id;
            return _delobj.getStudentAbsentDetails(dto);

        }




        [Route("absentsendsms")]
        public CollegeDailyAttendanceDTO absentsendsms([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.absentsendsms(data);
        }
        [Route("onreportshortagepercentage")]
        public CollegeDailyAttendanceDTO onreportshortagepercentage([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreportshortagepercentage(data);
        }
        [Route("onreporttotalattendance")]
        public CollegeDailyAttendanceDTO onreporttotalattendance([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onreporttotalattendance(data);
        }
        
        public void Post([FromBody]string value)
        {
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
