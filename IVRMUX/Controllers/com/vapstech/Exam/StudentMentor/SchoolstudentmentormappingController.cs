using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam.StudentMentor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.StudentMentor;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam.StudentMentor
{
    [Route("api/[controller]")]
    public class SchoolstudentmentormappingController : Controller
    {
        public SchoolstudentmentormappingDelegate _delg = new SchoolstudentmentormappingDelegate();


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
        public SchoolstudentmentormappingDTO Getdetails(int id)
        {
            SchoolstudentmentormappingDTO data = new SchoolstudentmentormappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }
        [Route("onchangeyear")]
        public SchoolstudentmentormappingDTO onchangeyear([FromBody] SchoolstudentmentormappingDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeyear(data);
        }
        [Route("getsection")]
        public SchoolstudentmentormappingDTO getsection([FromBody] SchoolstudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getsection(data);
        }
        [Route("getemployee")]
        public SchoolstudentmentormappingDTO getemployee([FromBody] SchoolstudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getemployee(data);
        }
        [Route("getstudentdata")]
        public SchoolstudentmentormappingDTO getstudentdata([FromBody] SchoolstudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getstudentdata(data);
        }
        [Route("savedata")]
        public SchoolstudentmentormappingDTO savedata([FromBody] SchoolstudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.savedata(data);
        }
        [Route("viewrecordspopup")]
        public SchoolstudentmentormappingDTO viewrecordspopup([FromBody] SchoolstudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.viewrecordspopup(data);
        }
        [Route("Deletedata")]
        public SchoolstudentmentormappingDTO Deletedata([FromBody] SchoolstudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Deletedata(data);
        }
        [Route("onreport")]
        public SchoolstudentmentormappingDTO onreport([FromBody] SchoolstudentmentormappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onreport(data);
        }
    }
}
