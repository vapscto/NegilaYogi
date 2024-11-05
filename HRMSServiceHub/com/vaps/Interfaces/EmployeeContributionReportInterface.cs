using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeContributionReportInterface
    {
        EmployeeContributionReportDTO getBasicData(EmployeeContributionReportDTO dto);

        //FilterEmployeeData

        EmployeeContributionReportDTO FilterEmployeeData(EmployeeContributionReportDTO dto);
        EmployeeContributionReportDTO getEmployeedetailsBySelection(EmployeeContributionReportDTO dto);

        EmployeeContributionReportDTO get_depts(EmployeeContributionReportDTO dto);
        EmployeeContributionReportDTO get_desig(EmployeeContributionReportDTO dto);
    }
}
