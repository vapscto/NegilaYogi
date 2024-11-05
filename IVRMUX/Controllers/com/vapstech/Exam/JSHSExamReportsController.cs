using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class JSHSExamReportsController : Controller
    {
        JSHSExamReportsDelegate _delg = new JSHSExamReportsDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("Getdetails/{id:int}")]
        public JSHSExamReportsDTO Getdetails(int id)
        {
            JSHSExamReportsDTO data = new JSHSExamReportsDTO();
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }

        [Route("get_classes")]
        public JSHSExamReportsDTO get_classes([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_classes(data);
        }

        [Route("get_sections")]
        public JSHSExamReportsDTO get_cls_sections([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_sections(data);
        }     

        [Route("get_students_category_grade")]
        public JSHSExamReportsDTO get_students([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_students_category_grade(data);
        }

        [Route("get_Exam_grade")]
        public JSHSExamReportsDTO get_Exam_grade([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_Exam_grade(data);
        }

        [Route("get_Exam_group")]
        public JSHSExamReportsDTO get_Exam_group([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_Exam_group(data);
        }

        [Route("get_exam")]
        public JSHSExamReportsDTO get_exam([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_exam(data);
        }

        [Route("GetStudentDetails")]
        public JSHSExamReportsDTO GetStudentDetails([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.GetStudentDetails(data);
        }
        //StudentDetailspramotion
        [Route("StudentDetailspramotion")]
        public JSHSExamReportsDTO StudentDetailspramotion([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.StudentDetailspramotion(data);
        }

        // Term Wise Report
        [Route("get_individualtermreport")]
        public JSHSExamReportsDTO get_individualtermreport([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_individualtermreport(data);
        }

        // Multiple Term Report Details
        [Route("get_reportdetails")]
        public JSHSExamReportsDTO get_reportdetails([FromBody]JSHSExamReportsDTO data)
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
        public JSHSExamReportsDTO getss_reportdetails([FromBody]JSHSExamReportsDTO data)
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
        public JSHSExamReportsDTO get_cumulativereportdetails([FromBody]JSHSExamReportsDTO data)
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
        public JSHSExamReportsDTO get_termcumulative_reportdetails([FromBody]JSHSExamReportsDTO data)
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
        public JSHSExamReportsDTO get_multipleexam_reportdetails([FromBody]JSHSExamReportsDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.get_multipleexam_reportdetails(data);
        }

        // Individual Exam Wise Progress Card Report
        [Route("saveddata")]
        public JSHSProgressCardReportDTO saveddata([FromBody] JSHSProgressCardReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.saveddata(data);
        }

        // IXTH TERM REPORT 
        [Route("getixtermreport")]
        public JSHSExamReportsDTO getixtermreport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getixtermreport(data);
        }

        // XTH TERM REPORT 
        [Route("get_individualtermxreport")]
        public JSHSExamReportsDTO get_individualtermxreport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.get_individualtermxreport(data);
        }

        // Multiple Exam Cumulative Report With Subjects Wise Total 
        [Route("getmultiple_exam_cumulative_report")]
        public JSHSExamReportsDTO getmultiple_exam_cumulative_report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmultiple_exam_cumulative_report(data);
        }

        // Multiple Exam Progress Report 
        [Route("getmultiple_exam_progress_report")]
        public JSHSExamReportsDTO getmultiple_exam_progress_report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmultiple_exam_progress_report(data);
        }

        // stjames  Exam Progress Report 
        [Route("stjamesexamreport")]
        public JSHSExamReportsDTO stjamesexamreport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.stjamesexamreport(data);
        }   
        
        // stjames Exam Consolidated Report 
        [Route("stjamesexamconsolidatedreport")]
        public JSHSExamReportsDTO stjamesexamconsolidatedreport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.stjamesexamconsolidatedreport(data);
        }   
        
        // stjames Exam Nursery Report 
        [Route("GetStjamesNurReport")]
        public JSHSExamReportsDTO GetStjamesNurReport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetStjamesNurReport(data);
        }  
        
        // BGHS Promotion Report Nursery and Junior 1
        [Route("promotionreportnurjr")]
        public JSHSExamReportsDTO promotionreportnurjr([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.promotionreportnurjr(data);
        }
        
        // BGHS Promotion Report Class i to iv
        [Route("promotionreportstdiiv")]
        public JSHSExamReportsDTO promotionreportstdiiv([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.promotionreportstdiiv(data);
        }        
        
        // BGHS Promotion Report Class IX
        [Route("bghspromotionreportix")]
        public JSHSExamReportsDTO bghspromotionreportix([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.bghspromotionreportix(data);
        } 
        
        // BCEHS Promotion Report Class IX
        [Route("bcehspromotionreportix")]
        public JSHSExamReportsDTO bcehspromotionreportix([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.bcehspromotionreportix(data);
        }

        // NDS Progress Card Report
        [Route("nds_6_8_report")]
        public JSHSExamReportsDTO nds_6_8_report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.nds_6_8_report(data);
        }

        [Route("nds_9_report")]
        public JSHSExamReportsDTO nds_9_report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.nds_9_report(data);
        }
        //nds_9_Newreport
        [Route("nds_9_Newreport")]
        public JSHSExamReportsDTO nds_9_Newreport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.nds_9_Newreport(data);
        }
        [Route("nds_1_5_report")]
        public JSHSExamReportsDTO nds_1_5_report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.nds_1_5_report(data);
        }

        [Route("nds_JrSrKG_report")]
        public JSHSExamReportsDTO nds_JrSrKG_report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.nds_JrSrKG_report(data);
        }

        //BGHS 2020-2021 promotion Progress Card Report

        [Route("BGHS_IIV_20202021")]
        public JSHSExamReportsDTO BGHS_IIV_20202021([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BGHS_IIV_20202021(data);
        }

        [Route("BGHS_Nurjr_20202021")]
        public JSHSExamReportsDTO BGHS_Nurjr_20202021([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BGHS_Nurjr_20202021(data);
        }

        [Route("BGHS_GetMultiple_Exam_Progress_Report")]
        public JSHSExamReportsDTO BGHS_GetMultiple_Exam_Progress_Report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BGHS_GetMultiple_Exam_Progress_Report(data);
        }

        [Route("BGHS_IX_20202021")]
        public JSHSExamReportsDTO BGHS_IX_20202021([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BGHS_IX_20202021(data);
        }

        //BIS Progress Card Report

        [Route("BISProgressCardReport")]
        public JSHSExamReportsDTO BISProgressCardReport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BISProgressCardReport(data);
        }

        [Route("BISPromotionCardReport")]
        public JSHSExamReportsDTO BISPromotionCardReport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BISPromotionCardReport(data);
        }

        [Route("BISFianlPromotionCardReport")]
        public JSHSExamReportsDTO BISFianlPromotionCardReport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BISFianlPromotionCardReport(data);
        }

        [Route("GetBISExamWiseProgressCardReport")]
        public JSHSExamReportsDTO GetBISExamWiseProgressCardReport([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetBISExamWiseProgressCardReport(data);
        }

        //BBHS IXth Class Promotion Progress Card  2020-2021

        [Route("BBHS_IX_20202021")]
        public JSHSExamReportsDTO BBHS_IX_20202021([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.BBHS_IX_20202021(data);
        }
        //St_Thomos_Ix2023
        [Route("St_Thomos_Ix2023")]
        public JSHSExamReportsDTO St_Thomos_Ix2023([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.St_Thomos_Ix2023(data);
        }
        //Pramotion_report
        [Route("Pramotion_report")]
        public JSHSExamReportsDTO Pramotion_report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Pramotion_report(data);
        }
        //Stthomos_III_V_Report
        [Route("Stthomos_III_V_Report")]
        public JSHSExamReportsDTO Stthomos_III_V_Report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Stthomos_III_V_Report(data);
        }
        //PromotionReportI_IV
        [Route("PromotionReportI_IV")]
        public JSHSExamReportsDTO PromotionReportI_IV([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.PromotionReportI_IV(data);
        }
        //PromotionReportTBSchool
        [Route("PromotionReportTBSchool")]
        public JSHSExamReportsDTO PromotionReportTBSchool([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.PromotionReportTBSchool(data);
        }
        //TBSReportSchool
        [Route("TBSReportSchool")]
        public JSHSExamReportsDTO TBSReportSchool([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.TBSReportSchool(data);
        }
        //getmultiple_exam_cumulative_Calcutta
        [Route("getmultiple_exam_cumulative_Calcutta")]
        public JSHSExamReportsDTO getmultiple_exam_cumulative_Calcutta([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmultiple_exam_cumulative_Calcutta(data);
        }
        //Sarvodaya_Report
        [Route("Sarvodaya_Report")]
        public JSHSExamReportsDTO Sarvodaya_Report([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Sarvodaya_Report(data);
        }
        //Sarvodaya_ReportSenior
        [Route("Sarvodaya_ReportSenior")]
        public JSHSExamReportsDTO Sarvodaya_ReportSenior([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Sarvodaya_ReportSenior(data);
        }
        //TBSPrimarySchool
        [Route("TBSPrimarySchool")]
        public JSHSExamReportsDTO TBSPrimarySchool([FromBody] JSHSExamReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.TBSPrimarySchool(data);
        }
    }
}
