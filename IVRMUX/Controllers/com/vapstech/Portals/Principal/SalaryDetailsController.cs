using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Principal;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Principal
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SalaryDetailsController : Controller
    {
        SalaryDetailsDelegate objdelegate1 = new SalaryDetailsDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public SalaryDetailsDTO getalldetails(int id)
        {
            SalaryDetailsDTO dto = new SalaryDetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate1.onloadgetdetails(dto);
        }
        [Route("Getdepartment/{id}")]
        public SalaryDetailsDTO Getdepartment(int id)
        {
            SalaryDetailsDTO dto = new SalaryDetailsDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate1.Getdepartment(dto);
        }
        [Route("get_designation")]
        public SalaryDetailsDTO get_designation([FromBody]SalaryDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate1.get_designation(dto);
        }
        [Route("get_department")]
        public SalaryDetailsDTO get_department([FromBody]SalaryDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate1.get_department(dto);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public SalaryDetailsDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]SalaryDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate1.GetEmployeeDetailsByLeaveYearAndMonth(dto);
        }

        [HttpPost]
        [Route("GenerateEmployeeSalarySlip")]
        public SalaryDetailsDTO getEmployeedetailsBySelection([FromBody]SalaryDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate1.GenerateEmployeeSalarySlip(dto);
        }
        [Route("get_employee")]
        public SalaryDetailsDTO get_employee([FromBody]SalaryDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return objdelegate1.get_employee(dto);
        }
    }
}
