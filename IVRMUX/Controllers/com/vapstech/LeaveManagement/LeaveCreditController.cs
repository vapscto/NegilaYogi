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
    public class LeaveCreditController : Controller
    {
        LeaveCreditDelegate lcd = new LeaveCreditDelegate();
        // GET: api/values
        [HttpGet]
        [Route("GetLeaveCredit/{id:int}")]
        public LeaveCreditDTO GetLeaveCredit(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.GetLeaveCredit(lv);
        }


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
        [Route("get_grade")]
        public LeaveCreditDTO get_grade([FromBody] LeaveCreditDTO lvd)
        {
            // LeaveCreditDTO lv = new LeaveCreditDTO();
            lvd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_grade(lvd);
        }

        [Route("get_leavecode")]
        public LeaveCreditDTO get_leavecode([FromBody] LeaveCreditDTO lc)
        {
           // LeaveCreditDTO lc = new LeaveCreditDTO();
            lc.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return lcd.get_leavecode(lc);
        }
        [HttpPost]
        [Route("SaveData")]
        public LeaveCreditDTO SaveData([FromBody] LeaveCreditDTO lvcd)
        {
            lvcd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvcd.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return lcd.SaveData(lvcd);
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
