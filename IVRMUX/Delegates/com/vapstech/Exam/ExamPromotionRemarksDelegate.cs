using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamPromotionRemarksDelegate
    {
        CommonDelegate<ExamPromotionRemarksDTO, ExamPromotionRemarksDTO> _comm = new CommonDelegate<ExamPromotionRemarksDTO, ExamPromotionRemarksDTO>();

        public ExamPromotionRemarksDTO Getdetails(ExamPromotionRemarksDTO data)
        {
            return _comm.POSTDataExam(data , "ExamPromotionRemarksFacade/Getdetails");
        }
        public ExamPromotionRemarksDTO get_class(ExamPromotionRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionRemarksFacade/get_class");
        }
        public ExamPromotionRemarksDTO get_section(ExamPromotionRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionRemarksFacade/get_section");
        }
        public ExamPromotionRemarksDTO get_exam(ExamPromotionRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionRemarksFacade/get_exam");
        }
        public ExamPromotionRemarksDTO search_student(ExamPromotionRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionRemarksFacade/search_student");
        }
        public ExamPromotionRemarksDTO save_details(ExamPromotionRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamPromotionRemarksFacade/save_details");
        }
        
    }
}
