using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Portals.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Portals.Principal
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CLG_PrincipleSMS_SendController : Controller
    {
        CLG_PrincipleSMS_SendDelegate SendStr = new CLG_PrincipleSMS_SendDelegate();

        [Route("Getdetails")]
        public SendSMSDTO Getdetails([FromBody]SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return SendStr.Getdetails(data);
        }
        [Route("Getdepartment/{id}")]
        public SendSMSDTO Getdepartment(int id)
        {
            SendSMSDTO data = new SendSMSDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.Getdepartment(data);
        }
        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public SendSMSDTO get_designation([FromBody]SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return SendStr.get_designation(data);
        }
        [Route("get_employee")]
        public SendSMSDTO get_employee([FromBody]SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return SendStr.get_employee(data);
        }
        [Route("savedetail")]
        public SendSMSDTO savedetail([FromBody] SendSMSDTO message)
        {
            message.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            message.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.savedetail(message);
        }
        [Route("GetStudentDetails")]
        public SendSMSDTO GetStudentDetails([FromBody] SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.GetStudentDetails(data);
        }
        [Route("getCourse")]
        public SendSMSDTO getCourse([FromBody] SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.getCourse(data);
        }
        [Route("getBranch")]
        public SendSMSDTO getBranch([FromBody] SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.getBranch(data);
        }
        [Route("getSemister")]
        public SendSMSDTO getSemister([FromBody] SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SendStr.getSemister(data);
        }
    }
}
