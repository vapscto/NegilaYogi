using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface PFForm3AInterface
    {
    PFReportsDTO getBasicData(PFReportsDTO dto);

    //FilterEmployeeData

    PFReportsDTO FilterEmployeeData(PFReportsDTO dto);
    PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto);
  }
}
