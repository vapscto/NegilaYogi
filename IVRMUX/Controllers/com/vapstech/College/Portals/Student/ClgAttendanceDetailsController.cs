using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgAttendanceDetailsController : Controller
    {
        ClgAttendanceDetailsDelegate clgdelegate = new ClgAttendanceDetailsDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgPortalAttendanceDTO getloaddata(ClgPortalAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                     
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getloaddata(data);
        }

        [Route("getAttdata")]
        public ClgPortalAttendanceDTO getAttdata([FromBody]ClgPortalAttendanceDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.getAttdata(sddto);
        }

    }
}
