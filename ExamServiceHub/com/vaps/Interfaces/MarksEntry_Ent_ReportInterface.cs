
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MarksEntry_Ent_ReportInterface
    {
        Task<MarksEntry_Ent_ReportDTO> Getdetails(MarksEntry_Ent_ReportDTO data);
        MarksEntry_Ent_ReportDTO get_report(MarksEntry_Ent_ReportDTO data);
        MarksEntry_Ent_ReportDTO SubjectList(MarksEntry_Ent_ReportDTO data);
        

    }
}
