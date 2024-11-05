using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class ProbationaryReportController : Controller
    {
        ProbationaryReportDelegate del = new ProbationaryReportDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeProfileReportDTO getalldetails(int id)
        {
            EmployeeProfileReportDTO dto = new EmployeeProfileReportDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getalldetails(dto);
        }
        [HttpPost]
        [Route("getProbationaryReport")]
        public EmployeeProfileReportDTO getProbationaryReport([FromBody]EmployeeProfileReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getProbationaryReport(dto);
        }
        [Route("get_departments")]
        public EmployeeProfileReportDTO get_departments([FromBody]EmployeeProfileReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));          
            return del.get_departments(dto);
        }
        [Route("get_designation")]
        public EmployeeProfileReportDTO get_designation([FromBody]EmployeeProfileReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));           
            return del.get_designation(dto);
        }
    }
}
