using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentAttendanceEntryController : Controller
    {
        //
        private StudentAttendanceEntryDelegate _stuAttDel = new StudentAttendanceEntryDelegate();
        //
        [Route("loadinitialdata/{id:int}")]
        public StudentAttendanceEntryDTO GetInitialData(int id)
        {
            StudentAttendanceEntryDTO data = new StudentAttendanceEntryDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            if (id == 0)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            else
            {
                data.ASMAY_Id = id;
            }
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _stuAttDel.GetInitialData(data);
        }

        [Route("onchangeyeargetclass")]
        public StudentAttendanceEntryDTO onchangeyeargetclass([FromBody] StudentAttendanceEntryDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _stuAttDel.GetInitialData(data);
        }

        [Route("getstudentdata")]
        public StudentAttendanceEntryDTO GetStudentData([FromBody]StudentAttendanceEntryDTO attdto)
        {
            attdto.username = Convert.ToString(HttpContext.Session.GetString("UserName"));           
            attdto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            attdto.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            attdto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            attdto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _stuAttDel.GetStudentData(attdto);
        }

        [HttpPost]
        public StudentAttendanceEntryDTO SaveStudentAttendanceEntry([FromBody]StudentAttendanceEntryDTO attdtoo)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            attdtoo.ASA_Network_IP = remoteIpAddress.ToString();
            attdtoo.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            attdtoo.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            attdtoo.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            attdtoo.ASA_Att_EntryType = "Present";

            return _stuAttDel.SaveStudentAttendance(attdtoo);
        }

        [Route("getmonthclassheld")]
        public StudentAttendanceEntryDTO getmonthclassheld([FromBody]StudentAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _stuAttDel.getmonthclassheld(data);
        }

        [Route("getdatewiseatt")]
        public StudentAttendanceEntryDTO getdatewiseatt([FromBody] StudentAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _stuAttDel.getdatewiseatt(data);
        }

        [Route("year")]
        public StudentAttendanceEntryDTO year([FromBody]StudentAttendanceEntryDTO attdto)
        {
            attdto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _stuAttDel.year(attdto);
        }

        [Route("getbatchlist")]
        public StudentAttendanceEntryDTO getbatchlist([FromBody]StudentAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _stuAttDel.getbatchlist(data);
        }

        [Route("getstdlistperiod")]
        public StudentAttendanceEntryDTO getstdlistperiod([FromBody] StudentAttendanceEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _stuAttDel.getstdlistperiod(data);
        }


        [Route("Deleteattendance")]
        public StudentAttendanceEntryDTO Deleteattendance([FromBody]StudentAttendanceEntryDTO attdtoo)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            attdtoo.ASA_Network_IP = remoteIpAddress.ToString();
            attdtoo.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            attdtoo.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            attdtoo.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            attdtoo.ASA_Att_EntryType = "Present";
            return _stuAttDel.Deleteattendance(attdtoo);
        }

        [Route("ViewAttendanceDetailsStaffWise")]
        public StudentAttendanceEntryDTO ViewAttendanceDetailsStaffWise([FromBody]StudentAttendanceEntryDTO attdtoo)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            attdtoo.ASA_Network_IP = remoteIpAddress.ToString();
            attdtoo.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            attdtoo.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            attdtoo.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _stuAttDel.ViewAttendanceDetailsStaffWise(attdtoo);
        }

        [Route("AttendanceDeleteRecordWise")]
        public StudentAttendanceEntryDTO AttendanceDeleteRecordWise([FromBody]StudentAttendanceEntryDTO attdtoo)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            attdtoo.ASA_Network_IP = remoteIpAddress.ToString();
            attdtoo.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            attdtoo.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            attdtoo.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _stuAttDel.AttendanceDeleteRecordWise(attdtoo);
        }

        //

        [Route("studentattendanceinsert")]
        public StudentAttendanceEntryDTO studentattendanceinsert([FromBody]StudentAttendanceEntryDTO attdtoo)
        {
            attdtoo.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            attdtoo.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           
            return _stuAttDel.studentattendanceinsert(attdtoo);
        }

    }
}