using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class SchoolStaffperiodmappingController : Controller
    {
        SchoolStaffperiodmappingDelegate _delg = new SchoolStaffperiodmappingDelegate();
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
        public SchoolStaffperiodmappingDTO Getdetails(int id)
        {
            SchoolStaffperiodmappingDTO data = new SchoolStaffperiodmappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }

        [Route("getemployeedetails")]
        public SchoolStaffperiodmappingDTO getemployeedetails([FromBody] SchoolStaffperiodmappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getemployeedetails(data);
        }
        [Route("onchangeclass")]
        public SchoolStaffperiodmappingDTO onchangeclass([FromBody] SchoolStaffperiodmappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeclass(data);
        }
        [Route("onchangesection")]
        public SchoolStaffperiodmappingDTO onchangesection([FromBody] SchoolStaffperiodmappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesection(data);
        }
        [Route("getsearchdetails")]
        public SchoolStaffperiodmappingDTO getsearchdetails([FromBody] SchoolStaffperiodmappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getsearchdetails(data);
        }
        [Route("savedata")]
        public SchoolStaffperiodmappingDTO savedata([FromBody] SchoolStaffperiodmappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedata(data);
        }

        [Route("Getdetailstransaction/{id:int}")]
        public SchoolStaffperiodmappingDTO Getdetailstransaction(int id)
        {
            SchoolStaffperiodmappingDTO data = new SchoolStaffperiodmappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.Getdetailstransaction(data);
        }
        [Route("getsearchdetailstransaction")]
        public SchoolStaffperiodmappingDTO getsearchdetailstransaction([FromBody] SchoolStaffperiodmappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getsearchdetailstransaction(data);
        }
        [Route("savedatatransaction")]
        public SchoolStaffperiodmappingDTO savedatatransaction([FromBody] SchoolStaffperiodmappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedatatransaction(data);
        }

    }
}
