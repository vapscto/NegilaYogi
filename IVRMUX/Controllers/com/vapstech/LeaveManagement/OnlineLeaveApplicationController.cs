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
    public class OnlineLeaveApplicationController : Controller
    {
        OnlineLeaveApplicationDelegate lcd = new OnlineLeaveApplicationDelegate();
        // GET: api/values
       
        
        [Route("getonlineLeave/{id:int}")]
        public LeaveCreditDTO getonlineLeave(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            lv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getonlineLeave(lv);
        }

        [Route("save")]
        public LeaveCreditDTO onlineleavesave([FromBody] LeaveCreditDTO test)
        {         
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.savedata(test);
        }

        [Route("saveadminLeave")]
        public LeaveCreditDTO saveadminLeave([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.saveadminLeave(test);
        }

        [Route("getonlineLeavestatus/{id:int}")]
        public LeaveCreditDTO getonlineLeavestatus(int id)
        {
            LeaveCreditDTO test = new LeaveCreditDTO();
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getonlineLeavestatus(test);
        }


        [Route("getSingleEmpLeavestatus/{empid:int}")]
        public LeaveCreditDTO getSingleEmpLeavestatus(int empid)
        {
            LeaveCreditDTO test = new LeaveCreditDTO();
            test.HRME_Id = empid;
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getSingleEmpLeavestatus(test);
        }

        [Route("getemployeeadmin/{id:int}")]
        public LeaveCreditDTO getemployeeadmin(int id)
        {
            LeaveCreditDTO test = new LeaveCreditDTO();
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getemployeeadmin(test);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public LeaveCreditDTO ActiveDeactiveRecord(int id)
        {
            LeaveCreditDTO dto = new LeaveCreditDTO();
            dto.HRELAP_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return lcd.deleterec(dto);
        }

        [Route("requestleave")]
        public LeaveCreditDTO requestleave([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.requestleave(test);
        }



        //--///////////////////////////////periodwiseleave//////////////////////////////////////
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public LeaveCreditDTO Get(int id)
        {
            LeaveCreditDTO lvv = new LeaveCreditDTO();
            lvv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lvv.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.getdetails(lvv);
        }


        [Route("getabsentstaff")]
        public LeaveCreditDTO getabsentstaff([FromBody] LeaveCreditDTO lvv)
        {
            lvv.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lvv.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.getabsentstaff(lvv);
        }
        [Route("get_free_stfdets")]
        public LeaveCreditDTO get_free_stfdets([FromBody] LeaveCreditDTO lvv)
        {
            lvv.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lvv.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.get_free_stfdets(lvv);
        }
        [Route("get_period_alloted")]
        public LeaveCreditDTO get_period_alloted([FromBody] LeaveCreditDTO lvv)
        {
            lvv.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lvv.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.get_period_alloted(lvv);
        }
        [Route("savedetails")]
        public LeaveCreditDTO savedetails([FromBody] LeaveCreditDTO lvv)
        {
            lvv.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lvv.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.savedetails(lvv);
        }

        [Route("updatedetails")]
        public LeaveCreditDTO updatedetails([FromBody] LeaveCreditDTO lvv)
        {
            lvv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            lvv.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.updatedetails(lvv);
        }
    }
}
