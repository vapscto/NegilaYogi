using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MarksEntryReportInterface
    {
        MarksEntryReportDTO Getdetails(MarksEntryReportDTO data);
        MarksEntryReportDTO get_class(MarksEntryReportDTO data);
        MarksEntryReportDTO get_section(MarksEntryReportDTO data);
        MarksEntryReportDTO get_exam(MarksEntryReportDTO data);
        MarksEntryReportDTO get_report(MarksEntryReportDTO data);
        MarksEntryReportDTO get_markspublishreport(MarksEntryReportDTO data);
    }
}
