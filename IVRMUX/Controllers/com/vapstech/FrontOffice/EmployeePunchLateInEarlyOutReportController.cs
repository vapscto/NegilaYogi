using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeePunchLateInEarlyOutReportController : Controller
    {
        EmployeePunchLateInEarlyOutReportDelegate od = new EmployeePunchLateInEarlyOutReportDelegate();

        // GET: api/Academic/5
        [Route("getalldetails/{id:int}")]
        public EmployeeInOutReportDTO getinitialdropdowns(int id)
        {
            EmployeeInOutReportDTO data = new EmployeeInOutReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.getdata(data);
        }

        [HttpPost]
        [Route("get_departments")]
        public EmployeeInOutReportDTO get_departments([FromBody] EmployeeInOutReportDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departments(data);
        }

        [Route("get_designation")]
        public EmployeeInOutReportDTO get_designation([FromBody] EmployeeInOutReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designation(data);
        }

        [Route("get_employee")]
        public EmployeeInOutReportDTO get_employee([FromBody] EmployeeInOutReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employee(data);
        }

        [HttpPost]
        [Route("getrpt")]
        public EmployeeInOutReportDTO getrpt([FromBody] EmployeeInOutReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrpt(data);
        }
    }
}
