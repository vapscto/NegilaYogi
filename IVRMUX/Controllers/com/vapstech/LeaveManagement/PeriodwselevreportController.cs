using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;


namespace IVRMUX.Controllers.com.vapstech.LeaveManagement
{

    [Route("api/[controller]")]
    public class PeriodwselevreportController : Controller
    {
        PeriodwseleavReportDelegate od = new PeriodwseleavReportDelegate();

        // [HttpGet]
        [Route("getalldetails/{id:int}")]
        public LeaveCreditDTO getinitialdropdowns(int id)
        {
            LeaveCreditDTO data = new LeaveCreditDTO();        
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departments(data);
        }

        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designation(data);
        }
        [Route("get_employee")]
        public LeaveCreditDTO get_employee([FromBody] LeaveCreditDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employee(data);
        }
        [HttpPost]
        [Route("getrpt")]
        public LeaveCreditDTO getrpt([FromBody] LeaveCreditDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrpt(data);
        }

        [HttpPost]
        [Route("getsiglerpt")]
        public LeaveCreditDTO getsiglerpt([FromBody] LeaveCreditDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getsiglerpt(data);
        }
    }
}
