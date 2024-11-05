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
    public class LeaveOpeningBalanceController : Controller
    {
        LeaveOpeningBalanceDelegate lcd = new LeaveOpeningBalanceDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getLeaveOB/{id:int}")]
        public LeaveCreditDTO getLeaveOB(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.getLeaveOB(lv);
        }
        [HttpPost]
        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO lv)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_departments(lv);
        }

        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO lvd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            lvd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_designation(lvd);
        }
        [Route("get_Employe_ob")]
        public LeaveCreditDTO get_Employe_ob([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_Employe_ob(ltd);
        }
        [Route("get_ob_Details")]
        public LeaveCreditDTO get_ob_Details([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_ob_Details(ltd);
        }



        [Route("SaveDetails")]        
        public LeaveCreditDTO Save_Details([FromBody] LeaveCreditDTO test)
        {
        test.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.Save_Details(test);
        }

        [Route("getdetails/{id:int}")]
        public LeaveCreditDTO getdetail(int id)
        {
            return lcd.getpagedetails(id);
        }

        [Route("deletepages/{id:int}")]
        public LeaveCreditDTO deletepages(int id)
        {
            return lcd.deletepages(id);
        }

    }
}
