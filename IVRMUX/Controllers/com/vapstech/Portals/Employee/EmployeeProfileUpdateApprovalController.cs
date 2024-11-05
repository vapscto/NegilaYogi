using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeProfileUpdateApprovalController : Controller
    {
        EmployeeProfileUpdateApprovalDelegate _delg = new EmployeeProfileUpdateApprovalDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddataprofileupdate/{id:int}")]
        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdate(int id)
        {
            EmployeeProfileUpdateApprovalDTO data = new EmployeeProfileUpdateApprovalDTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.loaddataprofileupdate(data);
        }

        [Route("Getcastecatgory")]
        public EmployeeProfileUpdateApprovalDTO Getcastecatgory([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getcastecatgory(data);
        }

        [Route("Getcaste")]
        public EmployeeProfileUpdateApprovalDTO Getcaste([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getcaste(data);
        }

        [Route("SaveData")]
        public EmployeeProfileUpdateApprovalDTO SaveData([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveData(data);
        }


        // Approval Details
        [Route("loaddataprofileupdateapproval/{id:int}")]
        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdateapproval(int id)
        {
            EmployeeProfileUpdateApprovalDTO data = new EmployeeProfileUpdateApprovalDTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.loaddataprofileupdateapproval(data);
        }
        
        [Route("OnChangeOfEmployee")]
        public EmployeeProfileUpdateApprovalDTO OnChangeOfEmployee([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeOfEmployee(data);
        }
        
        [Route("SaveApprovedData")]
        public EmployeeProfileUpdateApprovalDTO SaveApprovedData([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveApprovedData(data);
        }
    }
}