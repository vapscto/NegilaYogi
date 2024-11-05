using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeYearlyReportInterface
    {
        EmployeeYearlyReportDTO getBasicData(EmployeeYearlyReportDTO dto);

        EmployeeYearlyReportDTO FilterEmployeedetailsBySelection(EmployeeYearlyReportDTO dto);
        Task<EmployeeYearlyReportDTO> getEmployeedetailsBySelection(EmployeeYearlyReportDTO dto);

        EmployeeYearlyReportDTO get_depts(EmployeeYearlyReportDTO dto);

        EmployeeYearlyReportDTO get_desig(EmployeeYearlyReportDTO dto);

        Task<EmployeeYearlyReportDTO> reportBetweenDatesBySelection(EmployeeYearlyReportDTO dto);

    }
}
