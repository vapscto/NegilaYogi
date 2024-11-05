using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Form16Controller : Controller
    {
        Form16Delegate del = new Form16Delegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public Form16DTO getalldetails(int id)
        {
            Form16DTO dto = new Form16DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public Form16DTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]Form16DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }


        [Route("GenerateEmployeeSalarySlip")]
        public Form16DTO getEmployeedetailsBySelection([FromBody]Form16DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.GenerateEmployeeSalarySlip(dto);
        }

      


        }
}
