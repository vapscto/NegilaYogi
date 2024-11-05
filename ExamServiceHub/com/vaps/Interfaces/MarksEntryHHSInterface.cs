using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface MarksEntryHHSInterface
    {
        MarksEntryHHSDTO getdetails(MarksEntryHHSDTO id);
        MarksEntryHHSDTO get_classes(MarksEntryHHSDTO id);
        MarksEntryHHSDTO get_sections(MarksEntryHHSDTO id);
        MarksEntryHHSDTO get_exams(MarksEntryHHSDTO id);
        MarksEntryHHSDTO get_subjects(MarksEntryHHSDTO id);
        MarksEntryHHSDTO onsearch(MarksEntryHHSDTO id);
        MarksEntryHHSDTO SaveMarks(MarksEntryHHSDTO id);
    }
}
