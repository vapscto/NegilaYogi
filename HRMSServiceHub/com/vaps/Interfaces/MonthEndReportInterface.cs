using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MonthEndReportInterface
    {
        MonthEndReportDTO getBasicData(MonthEndReportDTO dto);
        Task<MonthEndReportDTO> getEmployeedetailsBySelection(MonthEndReportDTO dto);
    }
}
