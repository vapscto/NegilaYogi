using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public  interface Baldwin_Final_P_C_ReportInterface
    {
        Baldwin_Final_P_C_ReportDTO Getdetails(Baldwin_Final_P_C_ReportDTO data);
        Baldwin_Final_P_C_ReportDTO get_classes(Baldwin_Final_P_C_ReportDTO data);
        Baldwin_Final_P_C_ReportDTO get_sections(Baldwin_Final_P_C_ReportDTO data);
        Baldwin_Final_P_C_ReportDTO get_students(Baldwin_Final_P_C_ReportDTO data);
        Baldwin_Final_P_C_ReportDTO get_report(Baldwin_Final_P_C_ReportDTO data);
    }
}
