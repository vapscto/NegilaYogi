using corewebapi18072016.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.LeaveManagement
{
    [Route("api/[controller]")]
    public class LeaveReportController : Controller
    {
        LeaveReportDelegate ltmd = new LeaveReportDelegate();
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
        [Route("get_report")]
        public LeaveCreditDTO get_report([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_report(ltd);
        }
        //period//////////////////////////////////////////////////////////////////////

        [HttpGet]
        [Route("periodgetleavereport/{id:int}")]
        public LeaveCreditDTO periodgetleavereport(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.periodgetleavereport(lv);
        }
        [HttpPost]
        [Route("periodget_departments")]
        public LeaveCreditDTO periodget_departments([FromBody] LeaveCreditDTO lv)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.periodget_departments(lv);
        }

        [Route("periodget_designation")]
        public LeaveCreditDTO periodget_designation([FromBody] LeaveCreditDTO lvd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            lvd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.periodget_designation(lvd);
        }

        [Route("periodget_Employees")]
        public LeaveCreditDTO periodget_Employees([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.periodget_Employees(ltd);
        }
        [Route("periodget_report")]
        public LeaveCreditDTO periodget_report([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.periodget_report(ltd);
        }


    }
}