using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface MarksEntry_SSSEInterface
    {
        MarksEntry_SSSEDTO getdetails(MarksEntry_SSSEDTO id);
        MarksEntry_SSSEDTO get_classes(MarksEntry_SSSEDTO id);
        MarksEntry_SSSEDTO get_sections(MarksEntry_SSSEDTO id);
        MarksEntry_SSSEDTO get_exams(MarksEntry_SSSEDTO id);
        MarksEntry_SSSEDTO get_subjects(MarksEntry_SSSEDTO id);
        MarksEntry_SSSEDTO onsearch(MarksEntry_SSSEDTO id);
        MarksEntry_SSSEDTO SaveMarks(MarksEntry_SSSEDTO id);
    }
}
