
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class General_SendSMSController : Controller
    {


        General_SendSMSDelegates SendStr = new General_SendSMSDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public General_SendSMSDTO Getdetails([FromBody]General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return SendStr.Getdetails(data);
        }

        [Route("Getexam")]
        public General_SendSMSDTO Getexam([FromBody]General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        
            return SendStr.Getexam(data);
        }

        

        [Route("Getdepartment/{id}")]
        public General_SendSMSDTO Getdepartment(int id)
        {
            General_SendSMSDTO data = new General_SendSMSDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.Getdepartment(data);
        }


        // POST api/values
        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public General_SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public General_SendSMSDTO get_designation([FromBody]General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_designation(data);
        }
        [Route("get_employee")]
        public General_SendSMSDTO get_employee([FromBody]General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_employee(data);
        }

        [Route("savedetail")]
        public General_SendSMSDTO savedetail([FromBody] General_SendSMSDTO message)
        {

            message.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           message.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return SendStr.savedetail(message);
        }

        [Route("GetStudentDetails")]
        public General_SendSMSDTO GetStudentDetails([FromBody] General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.GetStudentDetails(data);
        }

        [Route("searchstddetails")]
        public General_SendSMSDTO searchstddetails([FromBody] General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.GetStudentDetails(data);
        }
        [Route("SrkvsSerach")]
        public General_SendSMSDTO SrkvsSerach([FromBody] General_SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.SrkvsSerach(data);
        }
    }

}
