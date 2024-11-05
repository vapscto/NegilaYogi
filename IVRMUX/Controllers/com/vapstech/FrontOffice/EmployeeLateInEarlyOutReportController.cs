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
    public class EmployeeLateInEarlyOutReportController : Controller
    {
        EmployeeLateInEarlyOutReportDelegate od = new EmployeeLateInEarlyOutReportDelegate();

        // GET: api/Academic/5
        [Route("getalldetails/{id:int}")]
        public EmployeeLateInEarlyOutReportDTO getinitialdropdowns(int id)
        {
            EmployeeLateInEarlyOutReportDTO data = new EmployeeLateInEarlyOutReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.getdata(data);
        }

        [HttpPost]
        [Route("get_departments")]
        public EmployeeLateInEarlyOutReportDTO get_departments([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departments(data);
        }

        [Route("get_designation")]
        public EmployeeLateInEarlyOutReportDTO get_designation([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designation(data);
        }

        [Route("get_employee")]
        public EmployeeLateInEarlyOutReportDTO get_employee([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employee(data);
        }

        [HttpPost]
        [Route("getrpt")]
        public EmployeeLateInEarlyOutReportDTO getrpt([FromBody] EmployeeLateInEarlyOutReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrpt(data);
        }
    }
}
