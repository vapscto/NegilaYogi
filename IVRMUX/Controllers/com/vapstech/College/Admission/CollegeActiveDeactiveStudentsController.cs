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
    public class CollegeActiveDeactiveStudentsController : Controller
    {
        public CollegeActiveDeactiveStudentsDelegate _deg = new CollegeActiveDeactiveStudentsDelegate();


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

        [Route("getdata/{id:int}")]
        public CollegeActiveDeactiveStudentsDTO getdata (int id)
        {
            CollegeActiveDeactiveStudentsDTO data = new CollegeActiveDeactiveStudentsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.getdata(data);
        }
        [Route("onacademicyearchange")]
        public CollegeActiveDeactiveStudentsDTO onacademicyearchange([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.onacademicyearchange(data);
        }
        [Route("oncoursechange")]
        public CollegeActiveDeactiveStudentsDTO oncoursechange([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.oncoursechange(data);
        }
        [Route("onbranchchange")]
        public CollegeActiveDeactiveStudentsDTO onbranchchange([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.onbranchchange(data);
        }
        [Route("onchangesemester")]
        public CollegeActiveDeactiveStudentsDTO onchangesemester([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.onchangesemester(data);
        }
        [Route("search")]
        public CollegeActiveDeactiveStudentsDTO search([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.search(data);
        }
        [Route("savedata")]
        public CollegeActiveDeactiveStudentsDTO savedata([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.savedata(data);
        }
        [Route("getreport")]
        public CollegeActiveDeactiveStudentsDTO getreport([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _deg.getreport(data);
        }
        

    }
}
