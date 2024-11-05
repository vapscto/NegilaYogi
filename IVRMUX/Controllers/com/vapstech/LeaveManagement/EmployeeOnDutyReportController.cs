using System;
using IVRMUX.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.LeaveManagement
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeeOnDutyReportController : Controller
    {
        // GET: /<controller>/
        // GET: api/values

        EmployeeOnDutyReportDeleget ltmd = new EmployeeOnDutyReportDeleget();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeOnDutyReportDTO getalldetails(int id)
        {
            EmployeeOnDutyReportDTO lv = new EmployeeOnDutyReportDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.getalldetails(lv);
        }

        [Route("getEmployeedetailsBySelection")]
        public EmployeeOnDutyReportDTO getEmployeedetailsBySelection([FromBody]EmployeeOnDutyReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.getEmployeedetailsBySelection(dto);
        }
    }
}
