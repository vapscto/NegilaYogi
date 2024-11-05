using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeOfferAndExperienceReportInterface
    {
        EmployeeOfferAndExperienceReportDTO getBasicData(EmployeeOfferAndExperienceReportDTO dto);
        EmployeeOfferAndExperienceReportDTO FilterEmployeeData(EmployeeOfferAndExperienceReportDTO dto);
        Task<EmployeeOfferAndExperienceReportDTO> getEmployeedetailsBySelection(EmployeeOfferAndExperienceReportDTO dto);
    }
}
