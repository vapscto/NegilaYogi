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
    public class EmployeeMonthlyReportController : Controller
    {
        EmployeeMonthlyReportDelegate od = new EmployeeMonthlyReportDelegate();
        
       // [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeMonthlyReportDTO getinitialdropdowns(int id)
        {
            EmployeeMonthlyReportDTO data = new EmployeeMonthlyReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeMonthlyReportDTO get_departments([FromBody] EmployeeMonthlyReportDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departments(data);
        }

        [Route("get_designation")]
        public EmployeeMonthlyReportDTO get_designation([FromBody] EmployeeMonthlyReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designation(data);
        }
        [Route("get_employee")]
        public EmployeeMonthlyReportDTO get_employee([FromBody] EmployeeMonthlyReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employee(data);
        }
        [HttpPost]
        [Route("getrpt")]
        public EmployeeMonthlyReportDTO getrpt([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrpt(data);
        }

        [HttpPost]
        [Route("getOTrpt")]
        public EmployeeMonthlyReportDTO getOTrpt([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getOTrpt(data);
        }

        [HttpPost]
        [Route("getrptStJames")]
        public EmployeeMonthlyReportDTO getrptStJames([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrptStJames(data);
        }
        
    }
}
