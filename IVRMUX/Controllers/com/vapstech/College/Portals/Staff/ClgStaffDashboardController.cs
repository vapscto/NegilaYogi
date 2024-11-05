using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class  ClgStaffDashboardController : Controller
    {
         ClgStaffDashboardDelegate clgdelegate = new  ClgStaffDashboardDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgStaffDashboardDTO getloaddata(ClgStaffDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));     
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.PaymentNootificationCollegeStaff = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationCollegeStaff"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.getloaddata(data);
        }
       
    }
}
