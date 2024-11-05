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

    public class EmployeeLogReportController : Controller
    {
        EmployeeLogReportDelegate od = new EmployeeLogReportDelegate();

        // [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeLogReportDTO getinitialdropdowns(int id)
        {
            EmployeeLogReportDTO data = new EmployeeLogReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeLogReportDTO get_departments([FromBody] EmployeeLogReportDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departments(data);
        }

        [Route("get_designation")]
        public EmployeeLogReportDTO get_designation([FromBody] EmployeeLogReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designation(data);
        }
        [Route("get_employee")]
        public EmployeeLogReportDTO get_employee([FromBody] EmployeeLogReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employee(data);
        }
        [HttpPost]
        [Route("getrpt")]
        public EmployeeLogReportDTO getrpt([FromBody] EmployeeLogReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrpt(data);
        }

        [HttpPost]
        [Route("getsiglerpt")]
        public EmployeeLogReportDTO getsiglerpt([FromBody] EmployeeLogReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getsiglerpt(data);
        }
    }
}
