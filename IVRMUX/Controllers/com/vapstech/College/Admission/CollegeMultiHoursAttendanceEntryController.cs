using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeMultiHoursAttendanceEntryController : Controller
    {

        CollegeMultiHoursAttendanceEntryDelegate _attForm = new CollegeMultiHoursAttendanceEntryDelegate();

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


        [Route("balgetalldetails/{id:int}")]
        public CollegeMultiHoursAttendanceEntryDTO balgetalldetails(int id)
        {
            CollegeMultiHoursAttendanceEntryDTO data = new CollegeMultiHoursAttendanceEntryDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.balgetalldetails(data);
        }

        [Route("getCoursedata")]
        public CollegeMultiHoursAttendanceEntryDTO getCoursedata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.getCoursedata(data);
        }

        [Route("getBranchdata")]
        public CollegeMultiHoursAttendanceEntryDTO getBranchdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.getBranchdata(data);
        }
        [Route("getSemesterdata")]
        public CollegeMultiHoursAttendanceEntryDTO getSemesterdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.getSemesterdata(data);
        }
        [Route("getSectiondata")]
        public CollegeMultiHoursAttendanceEntryDTO getSectiondata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.getSectiondata(data);
        }
        [Route("getSubjdata")]
        public CollegeMultiHoursAttendanceEntryDTO getSubjdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _attForm.getSubjdata(data);
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
        [Route("getStudentdata")]
        public CollegeMultiHoursAttendanceEntryDTO getStudentdata([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _attForm.getStudentdata(data);
        }

        [Route("saveatt")]
        public CollegeMultiHoursAttendanceEntryDTO saveatt([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _attForm.saveatt(data);
        }


        [Route("delete")]
        public CollegeMultiHoursAttendanceEntryDTO delete([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _attForm.delete(data);
        }

        [Route("getsaveddatepreview")]
        public CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview([FromBody]CollegeMultiHoursAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _attForm.getsaveddatepreview(data);
        }

        


        // POST api/values
        [HttpPost]
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
