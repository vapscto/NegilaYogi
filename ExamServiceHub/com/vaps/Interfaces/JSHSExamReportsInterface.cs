using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface JSHSExamReportsInterface
    {
        JSHSExamReportsDTO Getdetails(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_classes(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_sections(JSHSExamReportsDTO data);        
        JSHSExamReportsDTO get_students_category_grade(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_Exam_grade(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_Exam_group(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_exam(JSHSExamReportsDTO data);
        JSHSExamReportsDTO GetStudentDetails(JSHSExamReportsDTO data);
        JSHSExamReportsDTO StudentDetailspramotion(JSHSExamReportsDTO data);
        // Reports Function
        JSHSExamReportsDTO get_individualtermreport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_reportdetails(JSHSExamReportsDTO data);
        JSHSExamReportsDTO getss_reportdetails(JSHSExamReportsDTO data);        
        JSHSExamReportsDTO get_cumulativereportdetails(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_termcumulative_reportdetails(JSHSExamReportsDTO data);        
        JSHSExamReportsDTO get_multipleexam_reportdetails(JSHSExamReportsDTO data);
        Task<JSHSProgressCardReportDTO> saveddata(JSHSProgressCardReportDTO data);
        JSHSExamReportsDTO getixtermreport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO get_individualtermxreport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO getmultiple_exam_cumulative_report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO getmultiple_exam_progress_report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO stjamesexamreport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO stjamesexamconsolidatedreport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO GetStjamesNurReport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO promotionreportnurjr(JSHSExamReportsDTO data);
        JSHSExamReportsDTO promotionreportstdiiv(JSHSExamReportsDTO data);
        JSHSExamReportsDTO bghspromotionreportix(JSHSExamReportsDTO data);
        JSHSExamReportsDTO bcehspromotionreportix(JSHSExamReportsDTO data);
        JSHSExamReportsDTO nds_6_8_report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO St_Thomos_Ix2023(JSHSExamReportsDTO data);
        JSHSExamReportsDTO Pramotion_report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO Stthomos_III_V_Report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO PromotionReportI_IV(JSHSExamReportsDTO data);
        
         JSHSExamReportsDTO PromotionReportTBSchool(JSHSExamReportsDTO data);
        //getmultiple_exam_cumulative_Calcutta
        JSHSExamReportsDTO getmultiple_exam_cumulative_Calcutta(JSHSExamReportsDTO data);
        //TBSReportSchool
        JSHSExamReportsDTO TBSReportSchool(JSHSExamReportsDTO data);
        //Sarvodaya_Report
        JSHSExamReportsDTO Sarvodaya_Report(JSHSExamReportsDTO data);
        //Sarvodaya_ReportSenior
        JSHSExamReportsDTO Sarvodaya_ReportSenior(JSHSExamReportsDTO data);
        JSHSExamReportsDTO nds_9_report(JSHSExamReportsDTO data);
        //nds_9_Newreport
        JSHSExamReportsDTO nds_9_Newreport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO nds_1_5_report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO nds_JrSrKG_report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BGHS_IIV_20202021(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BGHS_Nurjr_20202021(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BGHS_GetMultiple_Exam_Progress_Report(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BGHS_IX_20202021(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BISProgressCardReport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BISPromotionCardReport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BISFianlPromotionCardReport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO GetBISExamWiseProgressCardReport(JSHSExamReportsDTO data);
        JSHSExamReportsDTO BBHS_IX_20202021(JSHSExamReportsDTO data);
        JSHSExamReportsDTO TBSPrimarySchool(JSHSExamReportsDTO data);
    }
}
