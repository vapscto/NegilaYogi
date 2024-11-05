using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamMonthEndReportInterface
    {
        ExamMonthEndReportDTO getdetails(ExamMonthEndReportDTO data);
        ExamMonthEndReportDTO onreport(ExamMonthEndReportDTO data);
    }
}
