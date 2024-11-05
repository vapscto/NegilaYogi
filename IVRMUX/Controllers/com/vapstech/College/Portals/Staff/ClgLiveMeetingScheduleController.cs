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
    public class ClgLiveMeetingScheduleController : Controller
    {
        ClgLiveMeetingScheduleDelegate clgdelegate = new ClgLiveMeetingScheduleDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgLiveMeetingScheduleDTO getloaddata(ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                     
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.getloaddata(data);
        }
        [Route("getcoursedata")]
        public ClgLiveMeetingScheduleDTO getcoursedata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.getcoursedata(data);
        }
        [Route("getbranchdata")]
        public ClgLiveMeetingScheduleDTO getbranchdata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.getbranchdata(data);
        }
        [Route("getsemdata")]
        public ClgLiveMeetingScheduleDTO getsemdata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.getsemdata(data);
        }

        [Route("getsection")]
        public ClgLiveMeetingScheduleDTO getsection([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.getsection(data);
        }

        [Route("editdata")]
        public ClgLiveMeetingScheduleDTO editdata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.editdata(data);
        }


        
        [Route("savedata")]
        public ClgLiveMeetingScheduleDTO savedata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.savedata(data);
        }

        [Route("deactive")]
        public ClgLiveMeetingScheduleDTO deactive([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.deactive(data);
        }
        //STAFF PROFILE
        [Route("getempdetails")]
        public ClgLiveMeetingScheduleDTO getempdetails(ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.getempdetails(data);
        }

        [Route("ondatechange")]
        public ClgLiveMeetingScheduleDTO ondatechange([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.ondatechange(data);
        }
        [Route("onstartmeeting")]
        public ClgLiveMeetingScheduleDTO onstartmeeting([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.onstartmeeting(data);
        }

        [Route("endmainmeeting")]
        public ClgLiveMeetingScheduleDTO endmainmeeting([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.endmainmeeting(data);
        }
        [Route("joinmeeting")]
        public ClgLiveMeetingScheduleDTO joinmeeting([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.joinmeeting(data);
        }
        //STUDENT PROFILE

        [Route("getstudentdetails")]
        public ClgLiveMeetingScheduleDTO getstudentdetails(ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.getstudentdetails(data);
        }

        [Route("endmainmeetingstudent")]
        public ClgLiveMeetingScheduleDTO endmainmeetingstudent([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.endmainmeetingstudent(data);
        }

        [Route("joinmeetingStudent")]
        public ClgLiveMeetingScheduleDTO joinmeetingStudent([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.joinmeetingStudent(data);
        }
        [Route("ondatechangestudent")]
        public ClgLiveMeetingScheduleDTO ondatechangestudent([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.ondatechangestudent(data);
        }

        [Route("getschrptdetails")]
        public ClgLiveMeetingScheduleDTO getschrptdetails( ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.getschrptdetails(data);
        }
        [Route("getschrptdetailsprofile")]
        public ClgLiveMeetingScheduleDTO getschrptdetailsprofile( ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return clgdelegate.getschrptdetailsprofile(data);
        }

        [Route("getschedulereport")]
        public ClgLiveMeetingScheduleDTO getschedulereport([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.getschedulereport(data);
        }
        [Route("getstaffprofilereport")]
        public ClgLiveMeetingScheduleDTO getstaffprofilereport([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.getstaffprofilereport(data);
        }

        [Route("getstudentprofiledata")]
        public ClgLiveMeetingScheduleDTO getstudentprofiledata( ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return clgdelegate.getstudentprofiledata(data);
        }
        [Route("getstudentprofilereport")]
        public ClgLiveMeetingScheduleDTO getstudentprofilereport([FromBody] ClgLiveMeetingScheduleDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.getstudentprofilereport(data);
        }


    }
}
