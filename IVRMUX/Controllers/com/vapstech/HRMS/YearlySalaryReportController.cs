using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class YearlySalaryReportController : Controller
    {
        EmployeeYearlyReportDelegate del = new EmployeeYearlyReportDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getBasicData/{id:int}")]
        public EmployeeYearlyReportDTO getalldetails(int id)
        {
            EmployeeYearlyReportDTO dto = new EmployeeYearlyReportDTO();
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
        [Route("filterEmployeedetailsBySelection")]
        public EmployeeYearlyReportDTO filterEmployeedetailsBySelection([FromBody]EmployeeYearlyReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.filterEmployeedetailsBySelection(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public EmployeeYearlyReportDTO getEmployeedetailsBySelection([FromBody]EmployeeYearlyReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public EmployeeYearlyReportDTO get_depts([FromBody]EmployeeYearlyReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_depts(dto);
        }

        [Route("get_desig")]
        public EmployeeYearlyReportDTO get_desig([FromBody]EmployeeYearlyReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_desig(dto);
        }

        [Route("reportBetweenDatesBySelection")]
        public EmployeeYearlyReportDTO reportBetweenDatesBySelection([FromBody]EmployeeYearlyReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.reportBetweenDatesBySelection(dto);
        }
    }
}
