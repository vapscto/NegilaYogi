using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface MarksEntry_SSInterface
    {
        MarksEntry_SSDTO getdetails(MarksEntry_SSDTO id);
        MarksEntry_SSDTO get_classes(MarksEntry_SSDTO id);
        MarksEntry_SSDTO get_sections(MarksEntry_SSDTO id);
        MarksEntry_SSDTO get_exams(MarksEntry_SSDTO id);
        MarksEntry_SSDTO get_subjects(MarksEntry_SSDTO id);
        MarksEntry_SSDTO onsearch(MarksEntry_SSDTO id);
        MarksEntry_SSDTO SaveMarks(MarksEntry_SSDTO id);
    }
}
