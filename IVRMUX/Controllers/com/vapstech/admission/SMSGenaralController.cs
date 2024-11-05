using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.admission;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.com.vaps.admission.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SMSGenaralController : Controller
    {
        SMSGenaralDelegate SendStr = new SMSGenaralDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        [Route("Getdetails")]
        public SMSGenaralDTO Getdetails([FromBody]SMSGenaralDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return SendStr.Getdetails(data);
        }

        [Route("Getexam")]
        public SMSGenaralDTO Getexam([FromBody]SMSGenaralDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return SendStr.Getexam(data);
        }



        [Route("Getdepartment/{id}")]
        public SMSGenaralDTO Getdepartment(int id)
        {
            SMSGenaralDTO data = new SMSGenaralDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.Getdepartment(data);
        }


        // POST api/values
        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public SMSGenaralDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]SMSGenaralDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public SMSGenaralDTO get_designation([FromBody]SMSGenaralDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_designation(data);
        }
        [Route("get_employee")]
        public SMSGenaralDTO get_employee([FromBody]SMSGenaralDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_employee(data);
        }

        [Route("savedetail")]
        public SMSGenaralDTO savedetail([FromBody] SMSGenaralDTO message)
        {
            message.Userid= Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            message.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.savedetail(message);
        }

        [Route("GetStudentDetails")]
        public SMSGenaralDTO GetStudentDetails([FromBody] SMSGenaralDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.GetStudentDetails(data);
        }

        [Route("searchstddetails")]
        public SMSGenaralDTO searchstddetails([FromBody] SMSGenaralDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.GetStudentDetails(data);
        }
    }

}
