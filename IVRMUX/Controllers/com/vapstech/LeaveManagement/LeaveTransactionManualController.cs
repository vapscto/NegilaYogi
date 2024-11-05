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
    public class LeaveTransactionManualController : Controller
    {
        LeaveTransactionManualDelegate ltmd = new LeaveTransactionManualDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getLeavetransm/{id:int}")]
        public LeaveCreditDTO getLeavetransm(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.getLeavetransm(lv);
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

        [Route("get_employee")]
        public LeaveCreditDTO get_employee([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_employee(ltd);
        }

        [Route("get_Emp_lop")]
        public LeaveCreditDTO get_Emp_lop([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_Emp_lop(ltd);
        }
        [Route("saveDATA")]
        public LeaveCreditDTO saveDATA([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.saveDATA(ltd);
        }
      
        [Route("Deletedetails")]
        public LeaveCreditDTO Delete([FromBody] LeaveCreditDTO ltd)
        {
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.DeleteMasterRecord(ltd);

        }
    }
}
