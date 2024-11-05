using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.LessonPlanner
{
    [Route("api/[controller]")]
    public class LMSStudentController : Controller
    {
        LMSStudentDelegate _delg = new LMSStudentDelegate();


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
        // College
        [Route("Getdetails/{id:int}")]
        public LMSStudentDTO Getdetails(int id)
        {
            LMSStudentDTO data = new LMSStudentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetails(data);
        }
        [Route("onchangesemester")]
        public LMSStudentDTO onchangesemester([FromBody] LMSStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesemester(data);
        }

        [Route("getcollegetopics")]
        public LMSStudentDTO getcollegetopics([FromBody] LMSStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getcollegetopics(data);
        }
        [Route("getcollegedocuments")]
        public LMSStudentDTO getcollegedocuments([FromBody] LMSStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getcollegedocuments(data);
        }
       

        // School 
        [Route("Getdetailsschool/{id:int}")]
        public LMSStudentDTO Getdetailsschool(int id)
        {
            LMSStudentDTO data = new LMSStudentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetailsschool(data);
        }

        [Route("onchangeyear")]
        public LMSStudentDTO onchangeyear([FromBody] LMSStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public LMSStudentDTO onchangeclass([FromBody] LMSStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeclass(data);
        }

        [Route("getschooltopics")]
        public LMSStudentDTO getschooltopics([FromBody] LMSStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getschooltopics(data);
        }
        [Route("getschooldocuments")]
        public LMSStudentDTO getschooldocuments([FromBody] LMSStudentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getschooldocuments(data);
        }


    }
}
