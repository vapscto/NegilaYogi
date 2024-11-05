
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamImportInterface
    {

        ExamMarksDTO getdetails(ExamMarksDTO id);

        ExamMarksDTO onselectAcdYear(ExamMarksDTO id);
        ExamMarksDTO onselectclass(ExamMarksDTO id);
        ExamMarksDTO onselectSection(ExamMarksDTO id);
        ExamMarksDTO onselectExam(ExamMarksDTO id);
        Task<ExamMarksDTO> onsearch(ExamMarksDTO id);
         ExamImportStudentDTO ImportMarks(ExamImportStudentDTO stu);
    }
}
