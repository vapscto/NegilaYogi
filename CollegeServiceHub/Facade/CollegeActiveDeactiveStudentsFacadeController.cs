using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeActiveDeactiveStudentsFacadeController : Controller
    {
        public CollegeActiveDeactiveStudentsInterface _intf;

        public CollegeActiveDeactiveStudentsFacadeController(CollegeActiveDeactiveStudentsInterface intf)
        {
            _intf = intf;
        }

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

        [Route("getdata")]
        public CollegeActiveDeactiveStudentsDTO getdata([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {          
            return _intf.getdata(data);
        }
        [Route("onacademicyearchange")]
        public CollegeActiveDeactiveStudentsDTO onacademicyearchange([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {           
            return _intf.onacademicyearchange(data);
        }
        [Route("oncoursechange")]
        public CollegeActiveDeactiveStudentsDTO oncoursechange([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {           
            return _intf.oncoursechange(data);
        }
        [Route("onbranchchange")]
        public CollegeActiveDeactiveStudentsDTO onbranchchange([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {           
            return _intf.onbranchchange(data);
        }
        [Route("onchangesemester")]
        public CollegeActiveDeactiveStudentsDTO onchangesemester([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {           
            return _intf.onchangesemester(data);
        }
        [Route("search")]
        public CollegeActiveDeactiveStudentsDTO search([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {           
            return _intf.search(data);
        }
        [Route("savedata")]
        public CollegeActiveDeactiveStudentsDTO savedata([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            return _intf.savedata(data);
        }
        [Route("getreport")]
        public CollegeActiveDeactiveStudentsDTO getreport([FromBody] CollegeActiveDeactiveStudentsDTO data)
        {
            return _intf.getreport(data);
        }
        
    }
}
