using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Interfaces
{
    public interface VProgressReportExamInterface
    {
        Task<VikasaSubjectwiseCumulativeReportDTO> showdetails(VikasaSubjectwiseCumulativeReportDTO data);
      
        VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_exam(VikasaSubjectwiseCumulativeReportDTO data);
        VikasaSubjectwiseCumulativeReportDTO get_Category(VikasaSubjectwiseCumulativeReportDTO data);
        Task<VikasaSubjectwiseCumulativeReportDTO> aggregativereport(VikasaSubjectwiseCumulativeReportDTO data);


    }
}
