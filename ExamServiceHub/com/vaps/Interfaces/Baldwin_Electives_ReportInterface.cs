using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface Baldwin_Electives_ReportInterface
    {
        Baldwin_Electives_ReportDTO Getdetails(Baldwin_Electives_ReportDTO data);
        Baldwin_Electives_ReportDTO get_categories(Baldwin_Electives_ReportDTO data);
        Baldwin_Electives_ReportDTO get_groups(Baldwin_Electives_ReportDTO data);
        Baldwin_Electives_ReportDTO get_subjects(Baldwin_Electives_ReportDTO data);
        Baldwin_Electives_ReportDTO get_sections(Baldwin_Electives_ReportDTO data);
        Baldwin_Electives_ReportDTO get_report(Baldwin_Electives_ReportDTO data);
    }
}
