using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface HeadwiseReportsInterface
    {
        HeaderwiseReportDTO getBasicData(HeaderwiseReportDTO dto);
        Task<HeaderwiseReportDTO> getEmployeedetailsBySelection(HeaderwiseReportDTO dto);

        HeaderwiseReportDTO get_depts(HeaderwiseReportDTO dto);

        HeaderwiseReportDTO get_desig(HeaderwiseReportDTO dto);
    }
}
