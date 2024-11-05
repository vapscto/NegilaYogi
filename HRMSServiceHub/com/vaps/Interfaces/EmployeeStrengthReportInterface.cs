using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeStrengthReportInterface
    {
        EmployeeStrengthReportDTO getBasicData(EmployeeStrengthReportDTO dto);
        EmployeeStrengthReportDTO getEmployeedetailsBySelection(EmployeeStrengthReportDTO dto);

        EmployeeStrengthReportDTO get_depts(EmployeeStrengthReportDTO dto);

        EmployeeStrengthReportDTO get_desig(EmployeeStrengthReportDTO dto);

    }
}
