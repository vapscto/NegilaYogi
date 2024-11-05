using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamPromotionReportDelegate
    {

        CommonDelegate<ExamPromotionReport_DTO, ExamPromotionReport_DTO> _comm = new CommonDelegate<ExamPromotionReport_DTO, ExamPromotionReport_DTO>();

        public ExamPromotionReport_DTO Getdetails(ExamPromotionReport_DTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionReportFacade/Getdetails");
        }
        public ExamPromotionReport_DTO get_class(ExamPromotionReport_DTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionReportFacade/get_class");
        }
        public ExamPromotionReport_DTO get_section(ExamPromotionReport_DTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionReportFacade/get_section");
        }
        public ExamPromotionReport_DTO get_exam(ExamPromotionReport_DTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionReportFacade/get_exam");
        }
        public ExamPromotionReport_DTO get_reports(ExamPromotionReport_DTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionReportFacade/get_reports");
        }
    }
}
