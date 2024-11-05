using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Student
{
    [Route("api/[controller]")]
    public class JSHSPortal_StudentReportsController : Controller
    {
        JSHSPortal_StudentReportsDelegate _delg = new JSHSPortal_StudentReportsDelegate();


        // GET: api/<controller>
        

        [Route("Getdetails_IT/{id:int}")]
        public JSHSPortal_StudentReportsDTO Getdetails(int id)
        {
            JSHSPortal_StudentReportsDTO data = new JSHSPortal_StudentReportsDTO();
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return _delg.Getdetails_IT(data);
        }

        [Route("get_Terms_IT")]
        public JSHSPortal_StudentReportsDTO get_Terms_IT([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.get_Terms_IT(data);
        }
        [Route("get_reportdetails_IT")]
        public JSHSPortal_StudentReportsDTO get_reportdetails_IT([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delg.get_reportdetails_IT(data);
        }
        [Route("get_Exam_grade_pc")]
        public JSHSPortal_StudentReportsDTO get_Exam_grade_pc([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
           return _delg.get_Exam_grade_pc(data);
        }
        // Individual Exam Wise Progress Card Report
        [Route("saveddata_pc")]
        public JSHSPortal_ProgressCardReportDTO saveddata_pc([FromBody] JSHSPortal_ProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.saveddata_pc(data);
        }

        //==============================================================================================
        [Route("get_sections")]
        public JSHSPortal_StudentReportsDTO get_cls_sections([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_sections(data);
        }
       

       

        [Route("get_exam")]
        public JSHSPortal_StudentReportsDTO get_exam([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_exam(data);
        }

        // Reports Functions

        // Term Wise Report
        

        // Multiple Term Report Details
        [Route("get_reportdetails")]
        public JSHSPortal_StudentReportsDTO get_reportdetails([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_reportdetails(data);
        }

        // Sneha Sagar Term Wise Report
        [Route("getss_reportdetails")]
        public JSHSPortal_StudentReportsDTO getss_reportdetails([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getss_reportdetails(data);
        }

        // Subject with Multiple Exam Wise Report
        [Route("get_cumulativereportdetails")]
        public JSHSPortal_StudentReportsDTO get_cumulativereportdetails([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_cumulativereportdetails(data);
        }

        // Term Wise Cumulative Report
        [Route("get_termcumulative_reportdetails")]
        public JSHSPortal_StudentReportsDTO get_termcumulative_reportdetails([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_termcumulative_reportdetails(data);
        }

        // Multiple Exam Wise Cumulative Report
        [Route("get_multipleexam_reportdetails")]
        public JSHSPortal_StudentReportsDTO get_multipleexam_reportdetails([FromBody]JSHSPortal_StudentReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_multipleexam_reportdetails(data);
        }

       
        // IXTH TERM REPORT 
        [Route("getixtermreport")]
        public JSHSPortal_StudentReportsDTO getixtermreport([FromBody] JSHSPortal_StudentReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getixtermreport(data);
        }

        // XTH TERM REPORT 
        [Route("get_individualtermxreport")]
        public JSHSPortal_StudentReportsDTO get_individualtermxreport([FromBody] JSHSPortal_StudentReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.get_individualtermxreport(data);
        }

        // Multiple Exam Cumulative Report With Subjects Wise Total 
        [Route("getmultiple_exam_cumulative_report")]
        public JSHSPortal_StudentReportsDTO getmultiple_exam_cumulative_report([FromBody] JSHSPortal_StudentReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmultiple_exam_cumulative_report(data);
        }

        // Multiple Exam Progress Report 
        [Route("getmultiple_exam_progress_report")]
        public JSHSPortal_StudentReportsDTO getmultiple_exam_progress_report([FromBody] JSHSPortal_StudentReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmultiple_exam_progress_report(data);
        }
    }
}
