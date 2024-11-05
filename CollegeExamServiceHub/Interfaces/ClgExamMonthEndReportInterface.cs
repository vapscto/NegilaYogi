using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
   public interface ClgExamMonthEndReportInterface
    {
        ClgExamMonthEndReportDTO getdetails(ClgExamMonthEndReportDTO data);
        ClgExamMonthEndReportDTO onreport(ClgExamMonthEndReportDTO data);
    }
}
