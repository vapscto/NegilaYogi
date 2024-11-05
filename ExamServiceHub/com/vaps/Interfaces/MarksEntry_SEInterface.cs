using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface MarksEntry_SEInterface
    {
        MarksEntry_SEDTO getdetails(MarksEntry_SEDTO id);
        MarksEntry_SEDTO get_classes(MarksEntry_SEDTO id);
        MarksEntry_SEDTO get_sections(MarksEntry_SEDTO id);
        MarksEntry_SEDTO get_exams(MarksEntry_SEDTO id);
        MarksEntry_SEDTO get_subjects(MarksEntry_SEDTO id);
        MarksEntry_SEDTO onsearch(MarksEntry_SEDTO id);
        MarksEntry_SEDTO SaveMarks(MarksEntry_SEDTO id);
    }
}
