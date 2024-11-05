using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgStudentDashboardController : Controller
    {
        ClgStudentDashboardDelegate clgdelegate = new ClgStudentDashboardDelegate();

        [HttpGet]
        [Route("Getdetails")]      
        public ClgStudentDashboardDTO Getdetails(ClgStudentDashboardDTO data)
        {     
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));          
            data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.Getdetails(data);
        }
        [Route("ViewStudentProfile")]
        public ClgStudentDashboardDTO ViewStudentProfile([FromBody]ClgStudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            if (data.student_staffflag == "Student")
            {
                data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMCST_Id"));
            }
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
           

            return clgdelegate.ViewStudentProfile(data);
        }

        [Route("onclick_syllabus")]
        public ClgStudentDashboardDTO onclick_syllabus([FromBody]ClgStudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
          
            data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMCST_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
         
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return clgdelegate.onclick_syllabus(data);
        }

        [Route("onclick_notice")]
        public ClgStudentDashboardDTO onclick_notice([FromBody]ClgStudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            //data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return clgdelegate.onclick_notice(data);
        }

        [Route("onclick_noticeboard_seen")]
        public ClgStudentDashboardDTO onclick_noticeboard_seen([FromBody]ClgStudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            //data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            HttpContext.Session.SetInt32("Feecheck", 1);
            HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return clgdelegate.onclick_noticeboard_seen(data);
        }


        [Route("ViewMonthWiseAttendance")]
        public ClgStudentDashboardDTO ViewMonthWiseAttendance([FromBody]ClgStudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.student_staffflag == "Student")
            {
                data.AMCST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            }
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.ViewMonthWiseAttendance(data);
        }
    }
}
