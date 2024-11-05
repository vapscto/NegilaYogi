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
    public class EmployeeServiceRecordReportController : Controller
    {
        EmployeeServiceRecordReportDelegate del = new EmployeeServiceRecordReportDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeServiceRecordReportDTO getalldetails(int id)
        {
            EmployeeServiceRecordReportDTO dto = new EmployeeServiceRecordReportDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [Route("getEmployeedetailsBySelection")]
        public EmployeeServiceRecordReportDTO getEmployeedetailsBySelection([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelection(dto);
        }

        //FilterEmployeeData

        [Route("FilterEmployeeData")]
        public EmployeeServiceRecordReportDTO FilterEmployeeData([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.FilterEmployeeData(dto);
        }
        [Route("get_depts")]
        public EmployeeServiceRecordReportDTO get_depts([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_depts(dto);
        }

        [Route("get_desig")]
        public EmployeeServiceRecordReportDTO get_desig([FromBody]EmployeeServiceRecordReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_desig(dto);
        }

    }
}