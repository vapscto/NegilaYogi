using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class CollegeStaffPeriodMappingController : Controller
    {
        public CollegeStaffPeriodMappingDelegate _delg = new CollegeStaffPeriodMappingDelegate();
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

        [Route("Getdetails/{id:int}")]
        public CollegeStaffPeriodMappingDTO  Getdetails (int id)
        {
            CollegeStaffPeriodMappingDTO data = new CollegeStaffPeriodMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetails(data);
        }
        [Route("getemployeedetails")]
        public CollegeStaffPeriodMappingDTO getemployeedetails([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getemployeedetails(data);
        }
        [Route("onchangecourse")]
        public CollegeStaffPeriodMappingDTO onchangecourse([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStaffPeriodMappingDTO onchangebranch([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangebranch(data);
        }
        [Route("onchangesemster")]
        public CollegeStaffPeriodMappingDTO onchangesemster([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesemster(data);
        }
        [Route("onchangesection")]
        public CollegeStaffPeriodMappingDTO onchangesection([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesection(data);
        }
        [Route("getsearchdetails")]
        public CollegeStaffPeriodMappingDTO getsearchdetails([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getsearchdetails(data);
        }

        [Route("savedata")]
        public CollegeStaffPeriodMappingDTO savedata([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedata(data);
        }

        // Staff Transaction
        [Route("Getdetailstransaction/{id:int}")]
        public CollegeStaffPeriodMappingDTO Getdetailstransaction(int id)
        {
            CollegeStaffPeriodMappingDTO data = new CollegeStaffPeriodMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));

            return _delg.Getdetailstransaction(data);
        }
        [Route("getsearchdetailstransaction")]
        public CollegeStaffPeriodMappingDTO getsearchdetailstransaction([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getsearchdetailstransaction(data);
        }
        [Route("savedatatransaction")]
        public CollegeStaffPeriodMappingDTO savedatatransaction([FromBody] CollegeStaffPeriodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedatatransaction(data);
        }
        
    }
}
