using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class OverallDailyAttendanceAbsentSMSController : Controller
    {
        OverallDailyAttendanceAbsentSMSDelegate sad = new OverallDailyAttendanceAbsentSMSDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("{id}")]
        [Route("getdetails")]
        public OveralldailyattendanceabsentsmsDTO getdetails()
        {
            OveralldailyattendanceabsentsmsDTO data = new OveralldailyattendanceabsentsmsDTO();
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return sad.getinitialdata(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getAttendetails")]
        public OveralldailyattendanceabsentsmsDTO getAttendetails([FromBody] OveralldailyattendanceabsentsmsDTO data)
        {
            // int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getserdata(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [Route("getStudentDetails")]
        public OveralldailyattendanceabsentsmsDTO getStudents([FromBody]OveralldailyattendanceabsentsmsDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.miid = mi_id;
            return sad.getstudentDet(dto);

        }
        [Route("sendsms")]
        public OveralldailyattendanceabsentsmsDTO sendsms([FromBody]OveralldailyattendanceabsentsmsDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.miid = mi_id;
            return sad.sendsms(dto);
        }
        [Route("sendemail")]
        public OveralldailyattendanceabsentsmsDTO sendemail([FromBody]OveralldailyattendanceabsentsmsDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.miid = mi_id;
            return sad.sendemail(dto);
        }

        [Route("smartcardatt")]
        public OveralldailyattendanceabsentsmsDTO smartcardatt([FromBody]OveralldailyattendanceabsentsmsDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.miid = mi_id;
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            dto.ASA_Network_IP = remoteIpAddress.ToString();
            return sad.smartcardatt(dto);
        }

        [Route("createuser")]
        public OveralldailyattendanceabsentsmsDTO createuser([FromBody]OveralldailyattendanceabsentsmsDTO dto)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.miid = mi_id;          
            return sad.createuser(dto);
        }

    }
}
