using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface ClgMarksEntryInterface
    {
        ClgMarksEntryDTO getdetails(ClgMarksEntryDTO data);
        ClgMarksEntryDTO onchangeyear(ClgMarksEntryDTO id);
        ClgMarksEntryDTO onchangecourse(ClgMarksEntryDTO id);
        ClgMarksEntryDTO onchangebranch(ClgMarksEntryDTO id);
        ClgMarksEntryDTO get_exams(ClgMarksEntryDTO id);
        ClgMarksEntryDTO get_subjects(ClgMarksEntryDTO id);
        ClgMarksEntryDTO getsubjectscheme(ClgMarksEntryDTO id);
        ClgMarksEntryDTO getsubjectschemetype(ClgMarksEntryDTO id);
        Task<ClgMarksEntryDTO> onsearch(ClgMarksEntryDTO id);
        ClgMarksEntryDTO SaveMarks(ClgMarksEntryDTO id);


    }
}
