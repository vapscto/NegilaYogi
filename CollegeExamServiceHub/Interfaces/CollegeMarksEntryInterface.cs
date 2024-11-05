using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface CollegeMarksEntryInterface
    {
        CollegeMarksEntryDTO getdetails(CollegeMarksEntryDTO data);
        CollegeMarksEntryDTO onchangeyear(CollegeMarksEntryDTO id);
        CollegeMarksEntryDTO onchangecourse(CollegeMarksEntryDTO id);
        CollegeMarksEntryDTO onchangebranch(CollegeMarksEntryDTO id);
        CollegeMarksEntryDTO get_exams(CollegeMarksEntryDTO id);
        CollegeMarksEntryDTO get_subjects(CollegeMarksEntryDTO id);
        CollegeMarksEntryDTO getsubjectscheme(CollegeMarksEntryDTO id);
        CollegeMarksEntryDTO getsubjectschemetype(CollegeMarksEntryDTO id);
        Task<CollegeMarksEntryDTO> onsearch(CollegeMarksEntryDTO id);        
        CollegeMarksEntryDTO SaveMarks(CollegeMarksEntryDTO id);
     
        Task<CollegeMarksEntryDTO> onsearchMobile(CollegeMarksEntryDTO id);
        CollegeMarksEntryDTO onchangesubsubject(CollegeMarksEntryDTO id);
    }
}