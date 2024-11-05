
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SendSMSController : Controller
    {
        SendSMSDelegates SendStr = new SendSMSDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{id}")]
        public string Getdetails(int id)
        {
            return "value";
        }
        [Route("Getdetails")]
        public SendSMSDTO Getdetails([FromBody]SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return SendStr.Getdetails(data);
        }

        [Route("Getdepartment/{id}")]
        public SendSMSDTO Getdepartment(int id)
        {
            SendSMSDTO data = new SendSMSDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return SendStr.Getdepartment(data);
        }


        // POST api/values
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
          //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return SendStr.get_designation(data);
        }
        [Route("get_employee")]
        public SendSMSDTO get_employee([FromBody]SendSMSDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
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

    }

}
