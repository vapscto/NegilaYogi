using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface VikasaLUInterface
    {
        VikasaLUReportDTO getdetails(VikasaLUReportDTO data);
        VikasaLUReportDTO onselectAcdYear(VikasaLUReportDTO data);
        VikasaLUReportDTO onselectclass(VikasaLUReportDTO data);
        VikasaLUReportDTO onselectSection(VikasaLUReportDTO data);
        VikasaLUReportDTO onreport(VikasaLUReportDTO data);
    }
}
