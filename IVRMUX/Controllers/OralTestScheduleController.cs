using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class OralTestScheduleController : Controller
    {


        // GET: /<controller>/
        OralTestScheduleDelegates OralTestScheduleDelegates = new OralTestScheduleDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public StudentDetailsDTO Getdetails(StudentDetailsDTO StudentDetailsDTO)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentDetailsDTO.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            StudentDetailsDTO.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            StudentDetailsDTO.ASMAY_Id = ASMAY_Id;

            return OralTestScheduleDelegates.GetOralTestScheduleData(StudentDetailsDTO);

        }

        [Route("GetStudentdetails/{id:int}")]
        public StudentDetailsDTO GetStudentdetails(int ID)
        {

            return OralTestScheduleDelegates.GetSelectedStudentData(ID);

        }

        [Route("GetCurrentStudentdetails/{id:int}")]
        public StudentDetailsDTO GetCurrentStudentdetails(int ID)
        {

            return OralTestScheduleDelegates.GetSelectedStudentData(ID);

        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public OralTestScheduleDTO GetSelectedRowDetails(int ID)
        {
            HttpContext.Session.SetString("OralTestScheduleID", ID.ToString());
            return OralTestScheduleDelegates.GetSelectedRowDetails(ID);

        }

        [Route("GetRemainStudentdetails/")]
        public OralTestScheduleDTO GetRemainStudentdetails()
        {
            Int32 OralTestScheduleID = 0;
            if (HttpContext.Session.GetString("OralTestScheduleID") != null)
            {
                OralTestScheduleID = Convert.ToInt32(HttpContext.Session.GetString("OralTestScheduleID"));
            }
            return OralTestScheduleDelegates.GetSelectedRowDetails(OralTestScheduleID);

        }

        [HttpPost]
        // public IActionResult Post([FromBody] regis reg)
        public OralTestScheduleDTO OralTestSchedule([FromBody] OralTestScheduleDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.IVRMSTAUL_Id = UserId;
            MMD.userid = UserId;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return OralTestScheduleDelegates.OralTestScheduleData(MMD);
        }


        [Route("OralTestScheduleDeletesStudentData")]
        public OralTestScheduleDTO OralTestScheduleDeletesStudentData([FromBody] OralTestScheduleDTO MMD)
        {

            return OralTestScheduleDelegates.OralTestScheduleDeletesStudentData(MMD);

        }

        [Route("classwisestudent")]
        public StudentDetailsDTO classwisestudent([FromBody] StudentDetailsDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;


            return OralTestScheduleDelegates.classwisestudent(MMD);

        }

        [Route("removestudents")]
        public OralTestScheduleDTO removestudents([FromBody] OralTestScheduleDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return OralTestScheduleDelegates.removestudents(MMD);

        }
        [Route("checkaddparticipants")]
        public OralTestScheduleDTO checkaddparticipants([FromBody] OralTestScheduleDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            MMD.userid = UserId;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return OralTestScheduleDelegates.checkaddparticipants(MMD);

        }
        [Route("ReseOralTestScheduleData")]
        public OralTestScheduleDTO ReseOralTestScheduleData([FromBody] OralTestScheduleDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return OralTestScheduleDelegates.ReseOralTestScheduleData(MMD);

        }

        [HttpDelete]
        [Route("OralTestScheduleDeletesData/{id:int}")]
        public OralTestScheduleDTO OralTestScheduleDeletesData(int ID)
        {

            return OralTestScheduleDelegates.OralTestScheduleDeletesData(ID);
            //reg.status = "sucess";

            // return reg;
        }


    }
}
