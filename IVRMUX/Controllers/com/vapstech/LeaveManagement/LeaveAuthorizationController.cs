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
    public class LeaveAuthorizationController : Controller
    {
        LeaveAuthorizationDelegate lcd = new LeaveAuthorizationDelegate();
        // GET: api/values
        
        
        [Route("getAuthLeave")]
        public LeaveCreditDTO getAuthLeave([FromBody] LeaveCreditDTO lv)
        {
            if (lv.onchangeoronload == "OnLoad")
            {
                lv.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }

            lv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getAuthLeave(lv);
        }

        [Route("saveauthdata")]
        public LeaveCreditDTO saveauthdata([FromBody] LeaveCreditDTO ltd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            //ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ltd.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.saveauthdata(ltd);
        }

        [Route("getauthdata")]
        public LeaveCreditDTO getauthdata([FromBody] LeaveCreditDTO ltd)
        {             
            ltd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.getauthdata(ltd);
        }

        [Route("getemployeelist")]
        public LeaveCreditDTO getemployeelist([FromBody]LeaveCreditDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.getemployeelist(data);
        }

        [Route("editdetails/{id:int}")]
        public LeaveCreditDTO editdetails(int id)
        {
            return lcd.editdetails(id);
        }
        
        [Route("deleteauth")]
        public LeaveCreditDTO deleteauth([FromBody] LeaveCreditDTO id)
        {
           // id.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.deleteauth(id);
        }
    }
}