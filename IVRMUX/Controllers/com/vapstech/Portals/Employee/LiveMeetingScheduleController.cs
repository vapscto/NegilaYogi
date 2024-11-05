using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class LiveMeetingScheduleController : Controller
    {
        LiveMeetingScheduleDelegate objdelegate = new LiveMeetingScheduleDelegate();

        //GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public LiveMeetingScheduleDTO getdetails(LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.getalldetails(data);
        }
        [Route("getempdetails")]
        public LiveMeetingScheduleDTO getempdetails(LiveMeetingScheduleDTO data)
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
        public LiveMeetingScheduleDTO ondatechangestudent([FromBody] LiveMeetingScheduleDTO data)
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

        [Route("endmainmeetingstudent")]
        public LiveMeetingScheduleDTO endmainmeetingstudent([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            //  data.ASMAY_Id = 59;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));


            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));



            return objdelegate.endmainmeetingstudent(data);
        }
        [Route("joinmeetingStudent")]
        public LiveMeetingScheduleDTO joinmeetingStudent([FromBody] LiveMeetingScheduleDTO data)
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

        [Route("getstudentdetails")]
        public LiveMeetingScheduleDTO getstudentdetails(LiveMeetingScheduleDTO data)
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

        [Route("ondatechange")]
        public LiveMeetingScheduleDTO ondatechange([FromBody] LiveMeetingScheduleDTO data)
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

            return objdelegate.ondatechange(data);
        }

        [Route("joinmeeting")]
        public LiveMeetingScheduleDTO joinmeeting([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));


            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.joinmeeting(data);
        }

        [Route("onstartmeeting")]
        public LiveMeetingScheduleDTO onstartmeeting([FromBody] LiveMeetingScheduleDTO data)
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
        [Route("endmainmeeting")]
        public LiveMeetingScheduleDTO endmainmeeting([FromBody] LiveMeetingScheduleDTO data)
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


        [Route("getclass")]
        public LiveMeetingScheduleDTO getclass([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));

            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.getclass(data);
        }
        [Route("getsection")]
        public LiveMeetingScheduleDTO getsection([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));


            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.getsection(data);
        }
        [Route("savedata")]
        public LiveMeetingScheduleDTO savedata([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));


            return objdelegate.savedata(data);
        }
        [Route("editdata")]
        public LiveMeetingScheduleDTO editdata([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));


            return objdelegate.editdata(data);
        }
        [Route("getsubject")]
        public LiveMeetingScheduleDTO getsubject([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.getsubject(data);
        }

        [Route("checkduplicate")]
        public LiveMeetingScheduleDTO checkduplicate([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.checkduplicate(data);
        }

        [HttpPost]
        [Route("deactive")]
        public LiveMeetingScheduleDTO deactive([FromBody] LiveMeetingScheduleDTO data)

        {

            //data.Amst_Id = Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return objdelegate.deactive(data);
        }


        //REPORT

        [Route("getschrptdetailsprofile")]
        public LiveMeetingScheduleDTO getschrptdetailsprofile(LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));

            return objdelegate.getschrptdetailsprofile(data);
        }



        [Route("getschrptdetails")]
        public LiveMeetingScheduleDTO getschrptdetails(LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));

            return objdelegate.getschrptdetails(data);
        }
        [Route("getstudentprofiledata")]
        public LiveMeetingScheduleDTO getstudentprofiledata(LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));

            return objdelegate.getstudentprofiledata(data);
        }

        [Route("getschedulereport")]
        public LiveMeetingScheduleDTO getschedulereport([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.getschedulereport(data);
        }

        [Route("getstaffprofilereport")]
        public LiveMeetingScheduleDTO getstaffprofilereport([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.getstaffprofilereport(data);
        }

        [Route("getstudentprofilereport")]
        public LiveMeetingScheduleDTO getstudentprofilereport([FromBody] LiveMeetingScheduleDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return objdelegate.getstudentprofilereport(data);
        }




    }
}
