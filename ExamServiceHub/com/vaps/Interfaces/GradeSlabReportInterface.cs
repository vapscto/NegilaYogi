
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface GradeSlabReportInterface
    {
        GradeSlabReportDTO getdetails(GradeSlabReportDTO data);
        GradeSlabReportDTO getAttendetails(GradeSlabReportDTO data);
    }
}
