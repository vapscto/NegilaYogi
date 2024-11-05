using corewebapi18072016.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.LeaveManagement
{
    [Route("api/[controller]")]
    public class LeaveYearlyReportController : Controller
    {
        LeaveYearlyReportDelegate ltmd = new LeaveYearlyReportDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getleavereport/{id:int}")]
        public LeaveCreditDTO getleavereport(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.getleavereport(lv);
        }
        [HttpPost]
        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO lv)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_departments(lv);
        }

        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO lvd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            lvd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_designation(lvd);
        }

        [Route("get_Employees")]
        public LeaveCreditDTO get_Employees([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_Employees(ltd);
        }

        [HttpPost]
        [Route("get_report")]
        public EmployeeYearlyReportDTO get_report([FromBody] EmployeeYearlyReportDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_report(ltd);
        }


    }
}