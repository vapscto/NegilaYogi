using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface PS7andPS8FormReportInterface
    {
        PFReportsDTO getBasicData(PFReportsDTO dto);

        
      
        PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto);
        PFReportsDTO getdataps8(PFReportsDTO dto);
    }
}

