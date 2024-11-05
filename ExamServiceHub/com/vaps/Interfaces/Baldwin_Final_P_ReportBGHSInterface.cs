using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface Baldwin_Final_P_ReportBGHSInterface
    {
        Baldwin_Final_P_ReportBGHSDTO Getdetails(Baldwin_Final_P_ReportBGHSDTO data);
        Baldwin_Final_P_ReportBGHSDTO get_classes(Baldwin_Final_P_ReportBGHSDTO data);
        Baldwin_Final_P_ReportBGHSDTO get_sections(Baldwin_Final_P_ReportBGHSDTO data);
        Baldwin_Final_P_ReportBGHSDTO get_students(Baldwin_Final_P_ReportBGHSDTO data);
        Baldwin_Final_P_ReportBGHSDTO get_report(Baldwin_Final_P_ReportBGHSDTO data);
    }
}
