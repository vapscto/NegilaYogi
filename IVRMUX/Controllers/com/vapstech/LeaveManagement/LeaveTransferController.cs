using corewebapi18072016.Delegates.com.vapstech.HRMS;
using corewebapi18072016.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.LeaveManagement
{


    [Route("api/[controller]")]
    public class LeaveTransferController : Controller
    {
    //    MasterLeaveYearDelegate del = new MasterLeaveYearDelegate();
        LeaveTransferDelegate lcd = new LeaveTransferDelegate();
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


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public LeaveCreditDTO getalldetails([FromBody] LeaveCreditDTO lv)
        {
            //  HR_Master_LeaveYearDTO dto = new HR_Master_LeaveYearDTO();

           
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

           // dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return lcd.onloadgetdetails(lv);
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
            test.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.Save_Details(test);
        }

        [Route("SaveDetails11")]
        public LeaveCreditDTO SaveDetails11([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.SaveDetails11(test);
        }

        [Route("leavecarryforward")]
        public LeaveCreditDTO leavecarryforward([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.leavecarryforward(test);
        }
        [Route("deletepages/{id:int}")]
        public LeaveCreditDTO deletepages(int id)
        {
            return lcd.deletepages(id);
        }

    }
}