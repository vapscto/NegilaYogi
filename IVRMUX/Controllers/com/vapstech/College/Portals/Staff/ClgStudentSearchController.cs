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
    public class ClgStudentSearchController : Controller
    {
        ClgStudentSearchDelegate clgdelegate = new ClgStudentSearchDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgPortalAttendanceDTO getloaddata(ClgPortalAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                     
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getloaddata(data);
        }
        [Route("getcoursedata")]
        public ClgPortalAttendanceDTO getcoursedata([FromBody]ClgPortalAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.getcoursedata(data);
        }
        [Route("getbranchdata")]
        public ClgPortalAttendanceDTO getbranchdata([FromBody]ClgPortalAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return clgdelegate.getbranchdata(data);
        }
        [Route("getsemdata")]
        public ClgPortalAttendanceDTO getsemdata([FromBody]ClgPortalAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return clgdelegate.getsemdata(data);
        }

        [Route("getstudent")]
        public ClgPortalAttendanceDTO getstudent([FromBody]ClgPortalAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return clgdelegate.getstudent(data);
        }


        
        [Route("getreport")]
        public ClgPortalAttendanceDTO getreport([FromBody]ClgPortalAttendanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return clgdelegate.getreport(data);
        }

    }
}
