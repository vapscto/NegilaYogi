
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MarksEntryInterface
    {
        ExamMarksDTO getdetails(ExamMarksDTO id);
        ExamMarksDTO onselectAcdYear(ExamMarksDTO id);
        ExamMarksDTO onselectclass(ExamMarksDTO id);
        ExamMarksDTO onselectSection(ExamMarksDTO id);
        ExamMarksDTO onselectExam(ExamMarksDTO id);
        ExamMarksDTO onselectSubject(ExamMarksDTO id);
        ExamMarksDTO onchangesubsubject(ExamMarksDTO id);
        Task<ExamMarksDTO> onsearch(ExamMarksDTO id);
        ExamMarksDTO SaveMarks(ExamMarksDTO id);
        ExamMarksDTO DeleteMarks(ExamMarksDTO id);
    }
}
