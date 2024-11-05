using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeDetailsReportInterface
    {
        EmployeeReportsDTO getBasicData(EmployeeReportsDTO dto);
        EmployeeReportsDTO FilterEmployeeData(EmployeeReportsDTO dto);
        Task<EmployeeReportsDTO> getEmployeedetailsBySelection(EmployeeReportsDTO dto);

        EmployeeReportsDTO get_depts(EmployeeReportsDTO dto);

        EmployeeReportsDTO get_desig(EmployeeReportsDTO dto);
    }
}
