using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface MarksEntry_SInterface
    {
        MarksEntry_SDTO getdetails(MarksEntry_SDTO id);
        MarksEntry_SDTO get_classes(MarksEntry_SDTO id);
        MarksEntry_SDTO get_sections(MarksEntry_SDTO id);
        MarksEntry_SDTO get_exams(MarksEntry_SDTO id);
        MarksEntry_SDTO get_subjects(MarksEntry_SDTO id);
        MarksEntry_SDTO onsearch(MarksEntry_SDTO id);
        MarksEntry_SDTO SaveMarks(MarksEntry_SDTO id);
    }
}
