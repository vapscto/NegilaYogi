using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;
using CommonLibrary;
namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamTermWiseRemarksDelegate
    {
        CommonDelegate<ExamTermWiseRemarksDTO, ExamTermWiseRemarksDTO> _comm = new CommonDelegate<ExamTermWiseRemarksDTO, ExamTermWiseRemarksDTO>();
        public ExamTermWiseRemarksDTO Getdetails(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/Getdetails");
        }
        public ExamTermWiseRemarksDTO get_class(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/get_class");
        }
        public ExamTermWiseRemarksDTO get_section(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/get_section");
        }
        public ExamTermWiseRemarksDTO get_term(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/get_term");
        }
        public ExamTermWiseRemarksDTO search_student(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/search_student");
        }
        public ExamTermWiseRemarksDTO save_details(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/save_details");
        }
        public ExamTermWiseRemarksDTO edit_details(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/edit_details");
        }

        // Term Wise Participate
        public ExamTermWiseRemarksDTO Getdetails_Participate(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/Getdetails_Participate");
        }
        public ExamTermWiseRemarksDTO search_student_participate(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/search_student_participate");
        }
        public ExamTermWiseRemarksDTO save_participate_details(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/save_participate_details");
        }
        public ExamTermWiseRemarksDTO ViewStudentParticipateDetails(ExamTermWiseRemarksDTO data)
        {
            return _comm.POSTDataExam(data, "ExamTermWiseRemarksFacade/ViewStudentParticipateDetails");
        }
    }
}