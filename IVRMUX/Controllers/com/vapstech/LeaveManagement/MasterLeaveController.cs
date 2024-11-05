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
    public class MasterLeaveController : Controller
    {
        MasterLeaveDelegate lcd = new MasterLeaveDelegate();
        // GET: api/values
        [HttpGet]

        [Route("GetLeave")]
        public MasterLeaveDTO GetLeave(int id)
        {
            MasterLeaveDTO lv = new MasterLeaveDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.GetLeave(lv);
        }

        [HttpPost]
        [Route("savedetail1")]
        public MasterLeaveDTO savedetail1([FromBody] MasterLeaveDTO categorypage)
        {
       
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.saveData(categorypage);
        }

        [HttpPost]
        [Route("validateordernumber")]
        public MasterLeaveDTO validateordernumber([FromBody] MasterLeaveDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.validateordernumber(categorypage);
        }

        [HttpPost]
        [Route("deactivate")]
        public MasterLeaveDTO deactivate([FromBody] MasterLeaveDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.deactivate(categorypage);
        }

        [HttpPost]
        [Route("SearchByColumn")]
        public MasterLeaveDTO SearchByColumn([FromBody] MasterLeaveDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.deactivate(categorypage);
        }
        [HttpPost]
        [Route("deletepages")]
        public MasterLeaveDTO deletepages([FromBody] MasterLeaveDTO categorypage)
        {
            return lcd.deletepages(categorypage);
        }



        [Route("getdetails/{id:int}")]
        public MasterLeaveDTO getdetail(int id)
        {
            return lcd.getpagedetails(id);
        }

    }
}