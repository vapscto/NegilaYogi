using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam.StudentMentorMapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam.StudentMentorMapping
{
    [Route("api/[controller]")]
    public class CollegedepartmentcoursebranchmappingController : Controller
    {
        public CollegedepartmentcoursebranchmappingDelegate _delg = new CollegedepartmentcoursebranchmappingDelegate();

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
        public CollegedepartmentcoursebranchmappingDTO Getdetails(int id)
        {
            CollegedepartmentcoursebranchmappingDTO data = new CollegedepartmentcoursebranchmappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }

        [Route("getbranch")]
        public CollegedepartmentcoursebranchmappingDTO getbranch([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getbranch(data);
        }
        [Route("getsemester")]
        public CollegedepartmentcoursebranchmappingDTO getsemester([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getsemester(data);
        }
        [Route("savedetails")]
        public CollegedepartmentcoursebranchmappingDTO savedetails([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }
        [Route("viewrecordspopup")]
        public CollegedepartmentcoursebranchmappingDTO viewrecordspopup([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewrecordspopup(data);
        }
        [Route("semesterdeactive")]
        public CollegedepartmentcoursebranchmappingDTO semesterdeactive([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.semesterdeactive(data);
        }
        [Route("deactivate")]
        public CollegedepartmentcoursebranchmappingDTO deactivate([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }
        [Route("Getdetailsreport/{id:int}")]
        public CollegedepartmentcoursebranchmappingDTO Getdetailsreport(int id)
        {
            CollegedepartmentcoursebranchmappingDTO data = new CollegedepartmentcoursebranchmappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetailsreport(data);
        }
        [Route("getreport")]
        public CollegedepartmentcoursebranchmappingDTO getreport([FromBody]CollegedepartmentcoursebranchmappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getreport(data);
        }
        

    }
}

