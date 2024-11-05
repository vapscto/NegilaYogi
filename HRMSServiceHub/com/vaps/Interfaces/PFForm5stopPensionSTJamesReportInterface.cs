using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public interface PFForm5stopPensionSTJamesReportInterface
    {
        PFReportsDTO getBasicData(PFReportsDTO dto);

        //FilterEmployeeData

        PFReportsDTO FilterEmployeeData(PFReportsDTO dto);
        PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto);
        PFReportsDTO getEmployeedetailsBySelectionStjames(PFReportsDTO dto);
    }
}
