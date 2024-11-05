
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface ExamCategoryReportInterface
    {
        ExamCategoryReportDTO getdetails(ExamCategoryReportDTO data);
        ExamCategoryReportDTO getAttendetails(ExamCategoryReportDTO data);
    }
}
