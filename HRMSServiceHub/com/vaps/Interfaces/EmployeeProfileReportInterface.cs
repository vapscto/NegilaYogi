using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeProfileReportInterface
    {
        EmployeeProfileReportDTO getBasicData(EmployeeProfileReportDTO dto);

        EmployeeProfileReportDTO FilterEmployeedetailsBySelection(EmployeeProfileReportDTO dto);
        Task<EmployeeProfileReportDTO> getEmployeedetailsBySelection(EmployeeProfileReportDTO dto);

        EmployeeProfileReportDTO get_depts(EmployeeProfileReportDTO dto);

        EmployeeProfileReportDTO get_desig(EmployeeProfileReportDTO dto);

    }
}
