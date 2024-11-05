using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ClassSectionAvgInterface
    {
        ClassSectionAvgDTO getdetails(ClassSectionAvgDTO data);
        ClassSectionAvgDTO onselectCategory(ClassSectionAvgDTO data);
        ClassSectionAvgDTO onselectclass(ClassSectionAvgDTO data);
        ClassSectionAvgDTO onreport(ClassSectionAvgDTO data);

    }
}
