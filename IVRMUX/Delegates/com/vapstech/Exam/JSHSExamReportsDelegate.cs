using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;


namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class JSHSExamReportsDelegate
    {
        CommonDelegate<JSHSExamReportsDTO, JSHSExamReportsDTO> _comm = new CommonDelegate<JSHSExamReportsDTO, JSHSExamReportsDTO>();
        CommonDelegate<JSHSProgressCardReportDTO, JSHSProgressCardReportDTO> _com = new CommonDelegate<JSHSProgressCardReportDTO, JSHSProgressCardReportDTO>();

        public JSHSExamReportsDTO Getdetails(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/Getdetails");
        }
        public JSHSExamReportsDTO get_classes(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_classes");
        }
        public JSHSExamReportsDTO get_sections(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_sections");
        }      
        public JSHSExamReportsDTO get_students_category_grade(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_students_category_grade");
        }
        public JSHSExamReportsDTO get_Exam_grade(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_Exam_grade");
        }
        public JSHSExamReportsDTO get_Exam_group(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_Exam_group");
        }
        public JSHSExamReportsDTO get_exam(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_exam");
        }
        public JSHSExamReportsDTO GetStudentDetails(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/GetStudentDetails");
        }
        //StudentDetailspramotion
        public JSHSExamReportsDTO StudentDetailspramotion(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/StudentDetailspramotion");
        }
        // Reports Function
        public JSHSExamReportsDTO get_individualtermreport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_individualtermreport");
        }
        public JSHSExamReportsDTO get_reportdetails(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_reportdetails");
        }
        public JSHSExamReportsDTO getss_reportdetails(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/getss_reportdetails");
        }       
        public JSHSExamReportsDTO get_cumulativereportdetails(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_cumulativereportdetails");
        }
        public JSHSExamReportsDTO get_termcumulative_reportdetails(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_termcumulative_reportdetails");
        }             
        public JSHSExamReportsDTO get_multipleexam_reportdetails(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_multipleexam_reportdetails");
        }
        public JSHSProgressCardReportDTO saveddata(JSHSProgressCardReportDTO data)
        {
            return _com.POSTDataExam(data, "JSHSExamReportsFacade/saveddata");
        }
        public JSHSExamReportsDTO getixtermreport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/getixtermreport");
        }
        public JSHSExamReportsDTO get_individualtermxreport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/get_individualtermxreport");
        }
        public JSHSExamReportsDTO getmultiple_exam_cumulative_report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/getmultiple_exam_cumulative_report");
        }
        public JSHSExamReportsDTO getmultiple_exam_progress_report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/getmultiple_exam_progress_report");
        }
        public JSHSExamReportsDTO stjamesexamreport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/stjamesexamreport");
        }
        public JSHSExamReportsDTO stjamesexamconsolidatedreport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/stjamesexamconsolidatedreport");
        }
        public JSHSExamReportsDTO GetStjamesNurReport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/GetStjamesNurReport");
        }
        public JSHSExamReportsDTO promotionreportnurjr(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/promotionreportnurjr");
        }
        public JSHSExamReportsDTO promotionreportstdiiv(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/promotionreportstdiiv");
        }
        public JSHSExamReportsDTO bghspromotionreportix(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/bghspromotionreportix");
        }
        public JSHSExamReportsDTO bcehspromotionreportix(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/bcehspromotionreportix");
        }
        public JSHSExamReportsDTO nds_6_8_report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/nds_6_8_report");
        }
        public JSHSExamReportsDTO nds_9_report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/nds_9_report");
        }
        //nds_9_Newreport
        public JSHSExamReportsDTO nds_9_Newreport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/nds_9_Newreport");
        }
        public JSHSExamReportsDTO nds_1_5_report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/nds_1_5_report");
        }
        public JSHSExamReportsDTO nds_JrSrKG_report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/nds_JrSrKG_report");
        }

        //BGHS 2020-2021 promotion Progress Card ReportS
        public JSHSExamReportsDTO BGHS_IIV_20202021(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BGHS_IIV_20202021");
        }
        public JSHSExamReportsDTO BGHS_Nurjr_20202021(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BGHS_Nurjr_20202021");
        }
        public JSHSExamReportsDTO BGHS_GetMultiple_Exam_Progress_Report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BGHS_GetMultiple_Exam_Progress_Report");
        }
        public JSHSExamReportsDTO BGHS_IX_20202021(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BGHS_IX_20202021");
        }

        //BIS Progress Card Report
        public JSHSExamReportsDTO BISProgressCardReport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BISProgressCardReport");
        }
        public JSHSExamReportsDTO BISPromotionCardReport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BISPromotionCardReport");
        }
        public JSHSExamReportsDTO BISFianlPromotionCardReport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BISFianlPromotionCardReport");
        }
        public JSHSExamReportsDTO GetBISExamWiseProgressCardReport(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/GetBISExamWiseProgressCardReport");
        }

        //BBHS IXth Class Promotion Progress Card  2020-2021
        public JSHSExamReportsDTO BBHS_IX_20202021(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/BBHS_IX_20202021");
        }
        //St_Thomos_Ix2023
        public JSHSExamReportsDTO St_Thomos_Ix2023(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/St_Thomos_Ix2023");
        }
        //Pramotion_report
        public JSHSExamReportsDTO Pramotion_report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/Pramotion_report");
        }
        //Stthomos_III_V_Report
        public JSHSExamReportsDTO Stthomos_III_V_Report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/Stthomos_III_V_Report");
        }
        //PromotionReportI_IV
        public JSHSExamReportsDTO PromotionReportI_IV(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/PromotionReportI_IV");
        }
        //PromotionReportTBSchool
        public JSHSExamReportsDTO PromotionReportTBSchool(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/PromotionReportTBSchool");
        }
        //TBSReportSchool
        public JSHSExamReportsDTO TBSReportSchool(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/TBSReportSchool");
        }
        //getmultiple_exam_cumulative_Calcutta
        public JSHSExamReportsDTO getmultiple_exam_cumulative_Calcutta(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/getmultiple_exam_cumulative_Calcutta");
        }
        //Sarvodaya_Report
        public JSHSExamReportsDTO Sarvodaya_Report(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/Sarvodaya_Report");
        }
        //Sarvodaya_ReportSenior
        public JSHSExamReportsDTO Sarvodaya_ReportSenior(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/Sarvodaya_ReportSenior");
        }
        //TBSPrimarySchool
        public JSHSExamReportsDTO TBSPrimarySchool(JSHSExamReportsDTO data)
        {
            return _comm.POSTDataExam(data, "JSHSExamReportsFacade/TBSPrimarySchool");
        }
    }
}
