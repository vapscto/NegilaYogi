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
    public class CollegegeneralsmsController : Controller
    {
        CollegegeneralsmsDelegate SendStr = new CollegegeneralsmsDelegate();
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

        [HttpPost]
        [Route("Getdetails")]
        public CollegegeneralsmsDTO Getdetails([FromBody]CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return SendStr.Getdetails(data);
        }

        [Route("Getexam")]
        public CollegegeneralsmsDTO Getexam([FromBody]CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return SendStr.Getexam(data);
        }



        [Route("Getdepartment/{id}")]
        public CollegegeneralsmsDTO Getdepartment(int id)
        {
            CollegegeneralsmsDTO data = new CollegegeneralsmsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.Getdepartment(data);
        }


        // POST api/values
        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public CollegegeneralsmsDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public CollegegeneralsmsDTO get_designation([FromBody]CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_designation(data);
        }
        [Route("get_employee")]
        public CollegegeneralsmsDTO get_employee([FromBody]CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_employee(data);
        }

        [Route("savedetail")]
        public CollegegeneralsmsDTO savedetail([FromBody] CollegegeneralsmsDTO message)
        {
            message.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            message.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.savedetail(message);
        }

        [Route("GetStudentDetails")]
        public CollegegeneralsmsDTO GetStudentDetails([FromBody] CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.GetStudentDetails(data);
        }

        [Route("searchstddetails")]
        public CollegegeneralsmsDTO searchstddetails([FromBody] CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.GetStudentDetails(data);
        }

        [Route("onSelectyear")]
        public CollegegeneralsmsDTO onSelectyear([FromBody] CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.onSelectyear(data);
        }
        [Route("onselectedcourse")]
        public CollegegeneralsmsDTO onselectedcourse([FromBody] CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.onselectedcourse(data);
        }
        [Route("onselectbranch")]
        public CollegegeneralsmsDTO onselectbranch([FromBody] CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.onselectbranch(data);
        }

        [Route("onselectsemister")]
        public CollegegeneralsmsDTO onselectsemister([FromBody] CollegegeneralsmsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.onselectsemister(data);
        }      

    }
}
