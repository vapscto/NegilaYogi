using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class PreLiveMeetingScheduleClgController : Controller
    {
        PreLiveMeetingScheduleClgDelegate objdelegate = new PreLiveMeetingScheduleClgDelegate();

        //GET: api/values
        
        [Route("getempdetails")]
        public PreLiveMeetingScheduleClgDTO getempdetails(PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));


            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.getempdetails(data);
        }

        [Route("ondatechangestudent")]
        public PreLiveMeetingScheduleClgDTO ondatechangestudent([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            //data.ASMAY_Id = 59;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));


            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));



            return objdelegate.ondatechangestudent(data);
        }

        [Route("onschedulecahnge")]
        public PreLiveMeetingScheduleClgDTO onschedulecahnge([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.onschedulecahnge(data);
        }

        [Route("endmainmeeting")]
        public PreLiveMeetingScheduleClgDTO endmainmeeting([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));


            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.endmainmeeting(data);
        }


        [Route("onstartmeeting")]
        public PreLiveMeetingScheduleClgDTO onstartmeeting([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            // data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));


            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.onstartmeeting(data);
        }

        [Route("saveremarks")]
        public PreLiveMeetingScheduleClgDTO saveremarks([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));

            return objdelegate.saveremarks(data);
        }

        [Route("getstudentdetails")]
        public PreLiveMeetingScheduleClgDTO getstudentdetails(PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            // data.ASMAY_Id = 59;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));



            return objdelegate.getstudentdetails(data);
        }
        [Route("joinmeetingStudent")]
        public PreLiveMeetingScheduleClgDTO joinmeetingStudent([FromBody] PreLiveMeetingScheduleClgDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            //data.ASMAY_Id = 59;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));


            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));



            return objdelegate.joinmeetingStudent(data);
        }
    }
}
