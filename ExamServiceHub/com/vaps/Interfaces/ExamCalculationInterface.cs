
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamCalculationInterface
    {

        ExamReportDTO Getdetails(ExamReportDTO data);
        ExamReportDTO get_cls_sections(ExamReportDTO data);
        ExamReportDTO Calculation(ExamReportDTO data);

        // Marks Approved Process To Display In Portals 
        ExamReportDTO getexam(ExamReportDTO data);
        ExamReportDTO getclass(ExamReportDTO data);
        ExamReportDTO saveapprove(ExamReportDTO data);
    }
}
