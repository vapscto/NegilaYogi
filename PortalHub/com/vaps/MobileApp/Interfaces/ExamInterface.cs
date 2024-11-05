using PreadmissionDTOs.com.vaps.MobileApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.MobileApp.Interfaces
{
    public interface ExamInterface
    {
        ExamDTO.getStudent getStudent(ExamDTO.getStudent data);
        ExamDTO.getExamdata getExamdata(ExamDTO.getExamdata data);
        ExamDTO.studentExamDetails studentExamDetails(ExamDTO.studentExamDetails data);
        ExamDTO.getdetails_IT Getdetails_IT(ExamDTO.getdetails_IT data);
        ExamDTO.getdetails_IT get_Exam_grade_pc(ExamDTO.getdetails_IT data);
        ExamDTO.getdetails_IT saveddata_pc(ExamDTO.getdetails_IT data);

    }
}
