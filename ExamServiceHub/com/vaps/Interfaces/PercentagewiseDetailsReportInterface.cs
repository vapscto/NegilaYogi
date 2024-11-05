using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface PercentagewiseDetailsReportInterface
    {
        PercentagewiseDetailsReportDTO getdetails(PercentagewiseDetailsReportDTO data);
        PercentagewiseDetailsReportDTO onselectAcdYear(PercentagewiseDetailsReportDTO data);
        PercentagewiseDetailsReportDTO onselectCategory(PercentagewiseDetailsReportDTO data);
        PercentagewiseDetailsReportDTO onselectclass(PercentagewiseDetailsReportDTO data);
        PercentagewiseDetailsReportDTO onselectSection(PercentagewiseDetailsReportDTO data);
        PercentagewiseDetailsReportDTO onreport(PercentagewiseDetailsReportDTO data);
    }
}
