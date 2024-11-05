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
    public class LeaveApprovalController : Controller
    {
        LeaveApprovalDelegate lcd = new LeaveApprovalDelegate();
        // GET: api/values
        [HttpGet]

        [HttpGet]
        [Route("getApprovalStatus/{id:int}")]
        public LeaveCreditDTO getApprovalStatus(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getApprovalStatus(lv);
        }

        [Route("get_status")]
        public LeaveCreditDTO get_status([FromBody] LeaveCreditDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.get_status(data);
        }

        [Route("reject_status")]
        public LeaveCreditDTO reject_status([FromBody] LeaveCreditDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.reject_status(data);
        }

        [HttpGet]
        [Route("getApprovedLeave/{id:int}")]
        public LeaveCreditDTO getApprovedLeave(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getApprovedLeave(lv);
        }
        [HttpPost]
        [Route("Viewleavebalancehistory")]
        public LeaveCreditDTO Viewleavebalancehistory([FromBody] LeaveCreditDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.Viewleavebalancehistory(data);
        }

        //onduty approval
        [HttpGet]
        [Route("getRequestStatus/{id:int}")]
        public LeaveCreditDTO getRequestStatus(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getRequestStatus(lv);
        }

        [Route("get_approvestatus")]
        public LeaveCreditDTO get_approvestatus([FromBody] LeaveCreditDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.get_approvestatus(data);
        }

        //PERIODWISEAPPROVAL/////////////////////////////////////////////////////////////////////
        [Route("getperiodApprovalStatus/{id:int}")]
        public LeaveCreditDTO getperiodApprovalStatus(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getperiodApprovalStatus(lv);
        }
        [Route("periodleavestatus")]
        public LeaveCreditDTO periodleavestatus([FromBody] LeaveCreditDTO data)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.periodleavestatus(data);
        }

    }
}