using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface SalaryloanInterfacereport
    {
        LoanReportDTO getBasicData(LoanReportDTO dto);
        Task<LoanReportDTO> getEmployeedetailsBySelection(LoanReportDTO dto);

        LoanReportDTO get_depts(LoanReportDTO dto);
        LoanReportDTO get_desig(LoanReportDTO dto);
    }
}
