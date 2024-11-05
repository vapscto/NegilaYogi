using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamLoginPrivilegesReportInterface
    {
        ExamLoginPrivilegesReportDTO getdetails(ExamLoginPrivilegesReportDTO data);
        ExamLoginPrivilegesReportDTO onselectAcdYear(ExamLoginPrivilegesReportDTO data);
        ExamLoginPrivilegesReportDTO onchangecategory(ExamLoginPrivilegesReportDTO data);
        ExamLoginPrivilegesReportDTO onselectclass(ExamLoginPrivilegesReportDTO data);
        ExamLoginPrivilegesReportDTO onchangesection(ExamLoginPrivilegesReportDTO data);
        ExamLoginPrivilegesReportDTO onreport(ExamLoginPrivilegesReportDTO data);
        
    }
}
