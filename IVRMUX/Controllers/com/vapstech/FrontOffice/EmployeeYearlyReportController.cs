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


    public class EmployeeYearlyReportController : Controller
    {
        EmployeeYearlyReportDelegate od = new EmployeeYearlyReportDelegate();
        
       // [HttpGet]
        [Route("getalldetails/{id:int}")]
        public EmployeeYearlyReportDTO getinitialdropdowns(int id)
        {
            EmployeeYearlyReportDTO data = new EmployeeYearlyReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeYearlyReportDTO get_departments([FromBody] EmployeeYearlyReportDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departments(data);
        }

        [Route("get_designation")]
        public EmployeeYearlyReportDTO get_designation([FromBody] EmployeeYearlyReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designation(data);
        }
        [Route("get_employee")]
        public EmployeeYearlyReportDTO get_employee([FromBody] EmployeeYearlyReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employee(data);
        }
        [HttpPost]
        [Route("getrpt")]
        public EmployeeYearlyReportDTO getrpt([FromBody] EmployeeYearlyReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrpt(data);
        }
    }
}
