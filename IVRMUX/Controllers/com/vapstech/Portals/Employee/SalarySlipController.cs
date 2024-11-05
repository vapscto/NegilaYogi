using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using PreadmissionDTOs.com.vaps.HRMS;

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class SalarySlipController : Controller 
    {

        SalarySlipDelegats del = new SalarySlipDelegats();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Employee_SalaryDTO getalldetails(int id)
        {
            HR_Employee_SalaryDTO dto = new HR_Employee_SalaryDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]HR_Employee_SalaryDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }

        [Route("GenerateEmployeeSalarySlip")]
        public HR_Employee_SalaryDTO getEmployeedetailsBySelection([FromBody]HR_Employee_SalaryDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.GenerateEmployeeSalarySlip(dto);
        }


          
     


    }
}
