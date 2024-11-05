using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface PassFailReportInterface
    {
        PassFailReportDTO getdetails(PassFailReportDTO data);
        PassFailReportDTO onselectCategory(PassFailReportDTO data);
        PassFailReportDTO onselectclass(PassFailReportDTO data);
        PassFailReportDTO onselectSection(PassFailReportDTO data);
        PassFailReportDTO onreport(PassFailReportDTO data);
    }
}
