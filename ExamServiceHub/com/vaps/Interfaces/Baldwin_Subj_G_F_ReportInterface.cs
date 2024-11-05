using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface Baldwin_Subj_G_F_ReportInterface
    {
        Baldwin_Subj_G_F_ReportDTO Getdetails(Baldwin_Subj_G_F_ReportDTO data);
        Baldwin_Subj_G_F_ReportDTO get_classes(Baldwin_Subj_G_F_ReportDTO data);
        Baldwin_Subj_G_F_ReportDTO get_sections(Baldwin_Subj_G_F_ReportDTO data);
        Baldwin_Subj_G_F_ReportDTO get_students(Baldwin_Subj_G_F_ReportDTO data);
        Baldwin_Subj_G_F_ReportDTO get_report(Baldwin_Subj_G_F_ReportDTO data);
    }
}
