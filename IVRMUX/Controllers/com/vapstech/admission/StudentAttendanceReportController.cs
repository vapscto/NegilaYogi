using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentAttendanceReportController : Controller
    {
        //private static FacadeUrl _config;
        StudentAttendanceReportDelegate sad = new StudentAttendanceReportDelegate();
        //private FacadeUrl fdu = new FacadeUrl();

        [Route("getdatabyselectedtype/{id:int}")]
        public StudentAttendanceReportDTO getDataBySelectedType(int id)
        {
            return sad.getDataBySelectedType(id);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("getdetails")]
        public StudentAttendanceReportDTO getdetails()
        {
            StudentAttendanceReportDTO data = new StudentAttendanceReportDTO();
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getinitialdata(data);
        }

        [HttpPost]
        [Route("getAttendetails")]
        public StudentAttendanceReportDTO getAttendetails([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getserdata(data);
        }

        [Route("getdatatype")]
        public StudentAttendanceReportDTO getdatatype([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getdatatype(data);
        }

        [Route("getreportdiv")]
        public StudentAttendanceReportDTO getreportdiv([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getreportdiv(data);
        }

        [Route("savetmpldatanew")]
        public StudentAttendanceReportDTO savetmpldatanew([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.savetmpldatanew(data);
        }

        [Route("onchangeyear")]
        public StudentAttendanceReportDTO onchangeyear([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.onchangeyear(data);
        }

        [Route("onclasschange")]
        public StudentAttendanceReportDTO onclasschange([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.onclasschange(data);
        }

        [Route("onsectionchange")]
        public StudentAttendanceReportDTO onsectionchange([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.onsectionchange(data);
        }

        [Route("getreport")]
        public StudentAttendanceReportDTO getreport([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getreport(data);
        }

        //Subject Wise Attendance Report

        [Route("LoadData/{id:int}")]
        public StudentAttendanceReportDTO LoadData(int id)
        {
            StudentAttendanceReportDTO data = new StudentAttendanceReportDTO();
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.LoadData(data);
        }

        [Route("OnChangeAcademicYear")]
        public StudentAttendanceReportDTO OnChangeAcademicYear([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnChangeAcademicYear(data);
        }

        [Route("OnChangeClass")]
        public StudentAttendanceReportDTO OnChangeClass([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public StudentAttendanceReportDTO OnChangeSection([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnChangeSection(data);
        }

        [Route("OnReport")]
        public StudentAttendanceReportDTO OnReport([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnReport(data);
        }

        [Route("PeriodWiseReportOverAll")]
        public StudentAttendanceReportDTO PeriodWiseReportOverAll([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.PeriodWiseReportOverAll(data);
        }

        // Attendance Details

        [Route("OnAttendanceLoadData/{id:int}")]
        public StudentAttendanceReportDTO OnAttendanceLoadData(int id)
        {
            StudentAttendanceReportDTO data = new StudentAttendanceReportDTO();
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnAttendanceLoadData(data);
        }

        [Route("OnAttendanceChangeYear")]
        public StudentAttendanceReportDTO OnAttendanceChangeYear([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnAttendanceChangeYear(data);
        }

        [Route("OnAttendanceChangeClass")]
        public StudentAttendanceReportDTO OnAttendanceChangeClass([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnAttendanceChangeClass(data);
        }

        [Route("OnAttendanceChangeSection")]
        public StudentAttendanceReportDTO OnAttendanceChangeSection([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnAttendanceChangeSection(data);
        }


        [Route("GetAttendanceDeletedReport")]
        public StudentAttendanceReportDTO GetAttendanceDeletedReport([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.GetAttendanceDeletedReport(data);
        }

        [Route("getclass")]
        public StudentAttendanceReportDTO getclass([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getclass(data);
        }
        //subjectwise student sms 
        [Route("getstudetails")]
        public StudentAttendanceReportDTO getstudetails([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getstudetails(data);
        }

        [Route("OnsendSMS")]
        public StudentAttendanceReportDTO OnsendSMS([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnsendSMS(data);
        }
        [Route("OnChangeSectionAbsent")]
        public StudentAttendanceReportDTO OnChangeSectionAbsent([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnChangeSectionAbsent(data);
        }

        [Route("OnChangeClassAbsent")]
        public StudentAttendanceReportDTO OnChangeClassAbsent([FromBody] StudentAttendanceReportDTO data)
        {
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.OnChangeClassAbsent(data);
        }

       
    }
}
