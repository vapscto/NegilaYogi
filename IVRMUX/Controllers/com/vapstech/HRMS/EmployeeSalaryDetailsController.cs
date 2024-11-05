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
    public class EmployeeSalaryDetailsController : Controller
    {
        EmployeeSalaryDetailsDelegate del = new EmployeeSalaryDetailsDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Employee_EarningsDeductionsDTO getalldetails(int id)
            {
            HR_Employee_EarningsDeductionsDTO dto = new HR_Employee_EarningsDeductionsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
            }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
            {
            return "value";
            }

        // POST api/values
        [HttpPost]
        public HR_Employee_EarningsDeductionsDTO Post([FromBody]HR_Employee_EarningsDeductionsDTO dto)
            {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
            }


     
        //getEmployeeSalaryDetails
        [Route("getEmployeeSalaryDetails")]
        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetails([FromBody] HR_Employee_EarningsDeductionsDTO dto)
            {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeeSalaryDetails(dto);
            }


        //getEmployeeSalaryDetails
        [Route("getEmployeeSalaryDetailsByHead")]
        public HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetailsByHead([FromBody] HR_Employee_EarningsDeductionsDTO dto)
            {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeeSalaryDetailsByHead(dto);
            }

        [Route("GetEmployeeDetailsBySelected")]
        public HR_Employee_EarningsDeductionsDTO GetEmployeeDetailsBySelected([FromBody] HR_Employee_EarningsDeductionsDTO dto)
            {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.GetEmployeeDetailsBySelected(dto);
            }

        [Route("get_depts")]
        public HR_Employee_EarningsDeductionsDTO get_depts([FromBody]HR_Employee_EarningsDeductionsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_depts(dto);
        }

        [Route("get_desig")]
        public HR_Employee_EarningsDeductionsDTO get_desig([FromBody]HR_Employee_EarningsDeductionsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_desig(dto);
        }

    }
}
