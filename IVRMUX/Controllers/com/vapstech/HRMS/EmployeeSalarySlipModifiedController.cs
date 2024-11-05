using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.HRMS;


namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeeSalarySlipModifiedController : Controller
    {
        EmployeeSalarySlipModifiedDelegate del = new EmployeeSalarySlipModifiedDelegate();
        //EmployeeSalarySlipGenerationDelegate del = new EmployeeSalarySlipGenerationDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Employee_SalaryModifiedDTO getalldetails(int id)
        {
            HR_Employee_SalaryModifiedDTO dto = new HR_Employee_SalaryModifiedDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public HR_Employee_SalaryModifiedDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.HRES_Month = ConcatMonth(dto.MonthList);
            return del.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }


        [Route("GenerateEmployeeSalarySlip")]
        public HR_Employee_SalaryModifiedDTO getEmployeedetailsBySelection([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.GenerateEmployeeSalarySlip(dto);
        }

        [NonAction]
        public string ConcatMonth(string[] value)
        {
            string ConcatValue=string.Empty;
            for (int i = 0; i < value.Length; i ++)
            {
                if (ConcatValue == string.Empty) { ConcatValue = "'" +  value[i].Trim().ToString() + "'"; }
                else
                { ConcatValue = ConcatValue + ",'" + value[i].Trim().ToString() + "'"; }
            }
            return ConcatValue;
        }

        [Route("SendEmailSMS")]
        public HR_Employee_SalaryModifiedDTO SendEmailSMS([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.SendEmailSMS(dto);
        }


        [Route("get_depts")]
        public HR_Employee_SalaryModifiedDTO get_depts([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_depts(dto);
        }

        //[Route("get_Months")]
        //public HR_Employee_SalaryModifiedDTO get_Months ([FromBody]HR_Employee_SalaryModifiedDTO dto)
        //{
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        //    // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return del.get_Months(dto);
        //}

        [Route("get_desig")]
        public HR_Employee_SalaryModifiedDTO get_desig([FromBody]HR_Employee_SalaryModifiedDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_desig(dto);
        }
    }
}