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
    public class CollegeAttendanceEntryNewController : Controller
    {

        CollegeAttendanceEntryNewDelegate _attForm = new CollegeAttendanceEntryNewDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [Route("getalldetails/{id:int}")]
        public CollegeMultiHoursAttendanceEntryDTO getalldetails(int id)
        {
            CollegeMultiHoursAttendanceEntryDTO data = new CollegeMultiHoursAttendanceEntryDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.getalldetails(data);
        }

        [Route("getsubjectslist")]
        public CollegeMultiHoursAttendanceEntryDTO getsubjectslist([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _attForm.getsubjectslist(data);
        }

        [Route("getStudentdata")]
        public CollegeMultiHoursAttendanceEntryDTO getStudentdata([FromBody] CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _attForm.getStudentdata(data);
        }
        
        [Route("getBatchdata")]
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _attForm.getBatchdata(data);
        }

        [Route("saveatt")]
        public CollegeMultiHoursAttendanceEntryDTO saveatt([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.saveatt(data);
        }

        [Route("getsaveddatepreview")]
        public CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.getsaveddatepreview(data);
        }

    }
}
