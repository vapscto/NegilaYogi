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
    public class CollegeAttendanceAbsentSMSController : Controller
    {

        CollegeAttendanceAbsentSMSDelegate _delobj = new CollegeAttendanceAbsentSMSDelegate();
        

        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public CollegeDailyAttendanceDTO getdetails(int id)
        {
            CollegeDailyAttendanceDTO data = new CollegeDailyAttendanceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
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

        [Route("getAttendetails")]
        public CollegeDailyAttendanceDTO getAttendetails([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
         
            return _delobj.getAttendetails(data);
        }

        [Route("absentsendsms")]
        public CollegeDailyAttendanceDTO absentsendsms([FromBody]CollegeDailyAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.absentsendsms(data);
        }

    }
}
