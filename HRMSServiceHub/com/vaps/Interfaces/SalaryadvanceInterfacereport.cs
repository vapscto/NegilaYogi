using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface SalaryadvanceInterfacereport
    {
        AdvanceReportDTO getBasicData(AdvanceReportDTO dto);
        Task<AdvanceReportDTO> getEmployeedetailsBySelection(AdvanceReportDTO dto);

        AdvanceReportDTO get_depts(AdvanceReportDTO dto);
        AdvanceReportDTO get_desig(AdvanceReportDTO dto);
    }
}
