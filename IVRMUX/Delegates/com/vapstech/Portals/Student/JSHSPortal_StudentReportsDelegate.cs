using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class JSHSPortal_StudentReportsDelegate
    {
        CommonDelegate<JSHSPortal_StudentReportsDTO, JSHSPortal_StudentReportsDTO> _comm = new CommonDelegate<JSHSPortal_StudentReportsDTO, JSHSPortal_StudentReportsDTO>();
      CommonDelegate<JSHSPortal_ProgressCardReportDTO, JSHSPortal_ProgressCardReportDTO> _com = new CommonDelegate<JSHSPortal_ProgressCardReportDTO, JSHSPortal_ProgressCardReportDTO>();

        public JSHSPortal_StudentReportsDTO Getdetails_IT(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/Getdetails_IT");
        }
        public JSHSPortal_StudentReportsDTO get_Terms_IT(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_Terms_IT");
        }
        public JSHSPortal_StudentReportsDTO get_reportdetails_IT(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_reportdetails_IT");
        }
        public JSHSPortal_StudentReportsDTO get_Exam_grade_pc(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_Exam_grade_pc");
        }
        public JSHSPortal_ProgressCardReportDTO saveddata_pc(JSHSPortal_ProgressCardReportDTO data)
        {
            return _com.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/saveddata_pc");
        }
        //===========================================================================================================
        public JSHSPortal_StudentReportsDTO get_sections(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_sections");
        }
       
       
        public JSHSPortal_StudentReportsDTO get_exam(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_exam");
        }

        // Reports Function
        
        public JSHSPortal_StudentReportsDTO get_reportdetails(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_reportdetails");
        }
        public JSHSPortal_StudentReportsDTO getss_reportdetails(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/getss_reportdetails");
        }       
        public JSHSPortal_StudentReportsDTO get_cumulativereportdetails(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_cumulativereportdetails");
        }
        public JSHSPortal_StudentReportsDTO get_termcumulative_reportdetails(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_termcumulative_reportdetails");
        }             
        public JSHSPortal_StudentReportsDTO get_multipleexam_reportdetails(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_multipleexam_reportdetails");
        }
       
        public JSHSPortal_StudentReportsDTO getixtermreport(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/getixtermreport");
        }
        public JSHSPortal_StudentReportsDTO get_individualtermxreport(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/get_individualtermxreport");
        }
        public JSHSPortal_StudentReportsDTO getmultiple_exam_cumulative_report(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/getmultiple_exam_cumulative_report");
        }
        public JSHSPortal_StudentReportsDTO getmultiple_exam_progress_report(JSHSPortal_StudentReportsDTO data)
        {
            return _comm.POSTPORTALData(data, "JSHSPortal_StudentReportsFacade/getmultiple_exam_progress_report");
        }
    }
}
