using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface ESIReportInterface
    {
        ESIReportDTO getBasicData(ESIReportDTO dto);
        Task<ESIReportDTO> getEmployeedetailsBySelection(ESIReportDTO dto);

        ESIReportDTO get_depts(ESIReportDTO dto);

        ESIReportDTO get_desig(ESIReportDTO dto);
    }
}
