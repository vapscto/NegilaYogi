using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface StudentPerformanceReportInterface
    {
        StudentPerformanceReportDTO getdetails(StudentPerformanceReportDTO data);
        StudentPerformanceReportDTO onselectCategory(StudentPerformanceReportDTO data);
        StudentPerformanceReportDTO onselectclass(StudentPerformanceReportDTO data);
        StudentPerformanceReportDTO onselectSection(StudentPerformanceReportDTO data);
        StudentPerformanceReportDTO onshow(StudentPerformanceReportDTO data);
    }
}
