using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class JSHSExamReportsFacadeController : Controller
    {
        public JSHSExamReportsInterface _interface;

        public JSHSExamReportsFacadeController(JSHSExamReportsInterface _inter)
        {
            _interface = _inter;
        }

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

        [Route("Getdetails")]
        public JSHSExamReportsDTO Getdetails([FromBody]JSHSExamReportsDTO data)
        {           
            return _interface.Getdetails(data);
        }

        [Route("get_classes")]
        public JSHSExamReportsDTO get_classes([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_classes(data);
        }

        [Route("get_sections")]
        public JSHSExamReportsDTO get_sections([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_sections(data);
        }

        [Route("get_students_category_grade")]
        public JSHSExamReportsDTO get_students([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_students_category_grade(data);
        }

        [Route("get_Exam_grade")]
        public JSHSExamReportsDTO get_Exam_grade([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_Exam_grade(data);
        }

        [Route("get_Exam_group")]
        public JSHSExamReportsDTO get_Exam_group([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_Exam_group(data);
        }

        [Route("get_exam")]
        public JSHSExamReportsDTO get_exam([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_exam(data);
        }

        [Route("GetStudentDetails")]
        public JSHSExamReportsDTO GetStudentDetails([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.GetStudentDetails(data);
        }
        //StudentDetailspramotion
        [Route("StudentDetailspramotion")]
        public JSHSExamReportsDTO StudentDetailspramotion([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.StudentDetailspramotion(data);
        }
        // Reports Function

        [Route("get_individualtermreport")]
        public JSHSExamReportsDTO get_individualtermreport([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_individualtermreport(data);
        }

        [Route("get_reportdetails")]
        public JSHSExamReportsDTO get_reportdetails([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_reportdetails(data);
        }

        [Route("getss_reportdetails")]
        public JSHSExamReportsDTO getss_reportdetails([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.getss_reportdetails(data);
        }
      
        [Route("get_cumulativereportdetails")]
        public JSHSExamReportsDTO get_cumulativereportdetails([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_cumulativereportdetails(data);
        }

        [Route("get_termcumulative_reportdetails")]
        public JSHSExamReportsDTO get_termcumulative_reportdetails([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_termcumulative_reportdetails(data);
        }      

        [Route("get_multipleexam_reportdetails")]
        public JSHSExamReportsDTO get_multipleexam_reportdetails([FromBody]JSHSExamReportsDTO data)
        {
            return _interface.get_multipleexam_reportdetails(data);
        }

        [Route("saveddata")]
        public async Task<JSHSProgressCardReportDTO> saveddata([FromBody] JSHSProgressCardReportDTO data)
        {
            return await _interface.saveddata(data);
        }

        [Route("get_individualtermxreport")]
        public JSHSExamReportsDTO get_individualtermxreport([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.get_individualtermxreport(data);
        }

        [Route("getixtermreport")]
        public JSHSExamReportsDTO getixtermreport([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.getixtermreport(data);
        }

        [Route("getmultiple_exam_cumulative_report")]
        public JSHSExamReportsDTO getmultiple_exam_cumulative_report([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.getmultiple_exam_cumulative_report(data);
        }

        [Route("getmultiple_exam_progress_report")]
        public JSHSExamReportsDTO getmultiple_exam_progress_report([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.getmultiple_exam_progress_report(data);
        }

        [Route("stjamesexamreport")]
        public JSHSExamReportsDTO stjamesexamreport([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.stjamesexamreport(data);
        }

        [Route("stjamesexamconsolidatedreport")]
        public JSHSExamReportsDTO stjamesexamconsolidatedreport([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.stjamesexamconsolidatedreport(data);
        }

        [Route("GetStjamesNurReport")]
        public JSHSExamReportsDTO GetStjamesNurReport([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.GetStjamesNurReport(data);
        }
        
        [Route("promotionreportnurjr")]
        public JSHSExamReportsDTO promotionreportnurjr([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.promotionreportnurjr(data);
        }

        [Route("promotionreportstdiiv")]
        public JSHSExamReportsDTO promotionreportstdiiv([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.promotionreportstdiiv(data);
        }

        [Route("bcehspromotionreportix")]
        public JSHSExamReportsDTO bcehspromotionreportix([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.bcehspromotionreportix(data);
        }

        [Route("bghspromotionreportix")]
        public JSHSExamReportsDTO bghspromotionreportix([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.bghspromotionreportix(data);
        }

        [Route("nds_6_8_report")]
        public JSHSExamReportsDTO nds_6_8_report([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.nds_6_8_report(data);
        }

        [Route("nds_9_report")]
        public JSHSExamReportsDTO nds_9_report([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.nds_9_report(data);
        }
        //nds_9_Newreport
        [Route("nds_9_Newreport")]
        public JSHSExamReportsDTO nds_9_Newreport([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.nds_9_Newreport(data);
        }
        [Route("nds_1_5_report")]
        public JSHSExamReportsDTO nds_1_5_report([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.nds_1_5_report(data);
        }

        [Route("nds_JrSrKG_report")]
        public JSHSExamReportsDTO nds_JrSrKG_report([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.nds_JrSrKG_report(data);
        }

        [Route("BGHS_IIV_20202021")]
        public JSHSExamReportsDTO BGHS_IIV_20202021([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BGHS_IIV_20202021(data);
        }

        [Route("BGHS_Nurjr_20202021")]
        public JSHSExamReportsDTO BGHS_Nurjr_20202021([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BGHS_Nurjr_20202021(data);
        }

        [Route("BGHS_GetMultiple_Exam_Progress_Report")]
        public JSHSExamReportsDTO BGHS_GetMultiple_Exam_Progress_Report([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BGHS_GetMultiple_Exam_Progress_Report(data);
        }

        [Route("BGHS_IX_20202021")]
        public JSHSExamReportsDTO BGHS_IX_20202021([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BGHS_IX_20202021(data);
        }

        //BIS Progress Card Report
        [Route("BISProgressCardReport")]
        public JSHSExamReportsDTO BISProgressCardReport([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BISProgressCardReport(data);
        }

        [Route("BISPromotionCardReport")]
        public JSHSExamReportsDTO BISPromotionCardReport([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BISPromotionCardReport(data);
        }

        [Route("BISFianlPromotionCardReport")]
        public JSHSExamReportsDTO BISFianlPromotionCardReport([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BISFianlPromotionCardReport(data);
        }

        [Route("GetBISExamWiseProgressCardReport")]
        public JSHSExamReportsDTO GetBISExamWiseProgressCardReport([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.GetBISExamWiseProgressCardReport(data);
        }

        //BBHS IXth Class Promotion Progress Card  2020-2021
        [Route("BBHS_IX_20202021")]
        public JSHSExamReportsDTO BBHS_IX_20202021([FromBody] JSHSExamReportsDTO data)
        {           
            return _interface.BBHS_IX_20202021(data);
        }
        //St_Thomos_Ix2023
        [Route("St_Thomos_Ix2023")]
        public JSHSExamReportsDTO St_Thomos_Ix2023([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.St_Thomos_Ix2023(data);
        }
        //Pramotion_report
        [Route("Pramotion_report")]
        public JSHSExamReportsDTO Pramotion_report([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.Pramotion_report(data);
        }
        //Stthomos_III_V_Report
        [Route("Stthomos_III_V_Report")]
        public JSHSExamReportsDTO Stthomos_III_V_Report([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.Stthomos_III_V_Report(data);
        }
        //PromotionReportI_IV
        [Route("PromotionReportI_IV")]
        public JSHSExamReportsDTO PromotionReportI_IV([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.PromotionReportI_IV(data);
        }
        //PromotionReportTBSchool
        [Route("PromotionReportTBSchool")]
        public JSHSExamReportsDTO PromotionReportTBSchool([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.PromotionReportTBSchool(data);
        }
        //getmultiple_exam_cumulative_Calcutta
        [Route("getmultiple_exam_cumulative_Calcutta")]
        public JSHSExamReportsDTO getmultiple_exam_cumulative_Calcutta([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.getmultiple_exam_cumulative_Calcutta(data);
        }
        //TBSReportSchool
        [Route("TBSReportSchool")]
        public JSHSExamReportsDTO TBSReportSchool([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.TBSReportSchool(data);
        }
        //Sarvodaya_Report
        [Route("Sarvodaya_Report")]
        public JSHSExamReportsDTO Sarvodaya_Report([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.Sarvodaya_Report(data);
        }
        //Sarvodaya_ReportSenior
        [Route("Sarvodaya_ReportSenior")]
        public JSHSExamReportsDTO Sarvodaya_ReportSenior([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.Sarvodaya_ReportSenior(data);
        }
        //TBSPrimarySchool
        [Route("TBSPrimarySchool")]
        public JSHSExamReportsDTO TBSPrimarySchool([FromBody] JSHSExamReportsDTO data)
        {
            return _interface.TBSPrimarySchool(data);
        }
    }
}
