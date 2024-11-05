using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
  public interface CTCReportInterface
    {
    CTCReportDTO getBasicData(CTCReportDTO dto);

    //FilterEmployeeData

   // CTCReportDTO FilterEmployeeData(CTCReportDTO dto);
    CTCReportDTO getEmployeedetailsBySelection(CTCReportDTO dto);
  }
}
