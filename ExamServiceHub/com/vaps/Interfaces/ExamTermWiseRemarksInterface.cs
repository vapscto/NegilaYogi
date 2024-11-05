using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;
namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamTermWiseRemarksInterface
    {
        ExamTermWiseRemarksDTO Getdetails(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO get_class(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO get_section(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO get_term(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO search_student(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO save_details(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO edit_details(ExamTermWiseRemarksDTO data);

        // Term Wise Participate
        ExamTermWiseRemarksDTO Getdetails_Participate(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO search_student_participate(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO save_participate_details(ExamTermWiseRemarksDTO data);
        ExamTermWiseRemarksDTO ViewStudentParticipateDetails(ExamTermWiseRemarksDTO data);
    }
}
