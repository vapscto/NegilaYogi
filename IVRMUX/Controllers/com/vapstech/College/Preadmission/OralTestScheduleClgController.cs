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
    public class OralTestScheduleClgController : Controller

    {

        OralTestScheduleClgDelegate OralTestScheduleDelegates = new OralTestScheduleClgDelegate();


        [Route("Getdetails/")]
        public DocumentViewClgDTO Getdetails(DocumentViewClgDTO StudentDetailsDTO)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentDetailsDTO.mi_id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            StudentDetailsDTO.user_id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            StudentDetailsDTO.asmay_id = ASMAY_Id;

            return OralTestScheduleDelegates.GetOralTestScheduleData(StudentDetailsDTO);

        }
        [Route("coursewisestudent")]
        public DocumentViewClgDTO classwisestudent([FromBody] DocumentViewClgDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mi_id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.user_id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.asmay_id = ASMAY_Id;


            return OralTestScheduleDelegates.coursewisestudent(MMD);

        }

        [HttpPost]
        // public IActionResult Post([FromBody] regis reg)
        public DocumentViewClgDTO OralTestSchedule([FromBody] DocumentViewClgDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mi_id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.IVRMSTAUL_Id = UserId;
            MMD.user_id = UserId;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.asmay_id = ASMAY_Id;

            return OralTestScheduleDelegates.OralTestScheduleData(MMD);
        }

        [Route("GetRemainStudentdetails/")]
        public DocumentViewClgDTO GetRemainStudentdetails()
        {
            Int32 OralTestScheduleID = 0;
            if (HttpContext.Session.GetString("OralTestScheduleID") != null)
            {
                OralTestScheduleID = Convert.ToInt32(HttpContext.Session.GetString("OralTestScheduleID"));
            }
            return OralTestScheduleDelegates.GetSelectedRowDetails(OralTestScheduleID);

        }

        [HttpDelete]
        [Route("OralTestScheduleDeletesData/{id:int}")]
        public DocumentViewClgDTO OralTestScheduleDeletesData(int ID)
        {

            return OralTestScheduleDelegates.OralTestScheduleDeletesData(ID);
            //reg.status = "sucess";

            // return reg;
        }

        [Route("checkaddparticipants")]
        public DocumentViewClgDTO checkaddparticipants([FromBody] DocumentViewClgDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.mi_id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.user_id = UserId;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return OralTestScheduleDelegates.checkaddparticipants(MMD);

        }


        [Route("scheduleGetreportdetails")]
        public DocumentViewClgDTO scheduleGetreportdetails([FromBody]DocumentViewClgDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mi_id = mid;

            return OralTestScheduleDelegates.scheduleGetreportdetails(rep);
        }

        [Route("remarksGetreportdetails")]
        public DocumentViewClgDTO remarksGetreportdetails([FromBody]DocumentViewClgDTO rep)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            rep.mi_id = mid;

            return OralTestScheduleDelegates.remarksGetreportdetails(rep);
        }
    }
}
