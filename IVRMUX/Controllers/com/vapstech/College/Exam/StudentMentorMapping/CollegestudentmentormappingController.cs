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
    public class CollegestudentmentormappingController : Controller
    {
        public CollegestudentmentormappingDelegate _delg = new CollegestudentmentormappingDelegate();

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
        public CollegestudentmentormappingDTO Getdetails(int id)
        {
            CollegestudentmentormappingDTO data = new CollegestudentmentormappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }
        [Route("onchangeyear")]
        public CollegestudentmentormappingDTO onchangeyear([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeyear(data);
        }
        [Route("getbranch")]
        public CollegestudentmentormappingDTO getbranch([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getbranch(data);
        }
        [Route("getsemester")]
        public CollegestudentmentormappingDTO getsemester([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getsemester(data);
        }
        [Route("getsection")]
        public CollegestudentmentormappingDTO getsection([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getsection(data);
        }
        [Route("getemployee")]
        public CollegestudentmentormappingDTO getemployee([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getemployee(data);
        }
        [Route("getstudentdata")]
        public CollegestudentmentormappingDTO getstudentdata([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getstudentdata(data);
        }
        [Route("savedata")]
        public CollegestudentmentormappingDTO savedata([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedata(data);
        }
        [Route("viewrecordspopup")]
        public CollegestudentmentormappingDTO viewrecordspopup([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewrecordspopup(data);
        }
        [Route("Deletedata")]
        public CollegestudentmentormappingDTO Deletedata([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Deletedata(data);
        }

        // Report
        [Route("Getreportdetails/{id:int}")]
        public CollegestudentmentormappingDTO Getreportdetails(int id)
        {
            CollegestudentmentormappingDTO data = new CollegestudentmentormappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getreportdetails(data);
        }
        [Route("getreport")]
        public CollegestudentmentormappingDTO getreport([FromBody]CollegestudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getreport(data);
        }
        

    }
}
