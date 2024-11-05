using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface EPFcontributionRegisterInterface
    {
        PFReportsDTO getBasicData(PFReportsDTO dto);

        //FilterEmployeeData

        PFReportsDTO FilterEmployeeData(PFReportsDTO dto);
        PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto);
        PFReportsDTO getEmployeedetailsBySelectionBBKV(PFReportsDTO dto);
        PFReportsDTO getEmployeedetailsBySelectionStJames(PFReportsDTO dto);
    }
}
