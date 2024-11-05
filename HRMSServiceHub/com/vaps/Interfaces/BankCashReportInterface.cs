using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface BankCashReportInterface
    {
        BankCashReportDTO getBasicData(BankCashReportDTO dto);
        Task<BankCashReportDTO> getEmployeedetailsBySelection(BankCashReportDTO dto);
        BankCashReportDTO get_depts(BankCashReportDTO dto);

        BankCashReportDTO get_desig(BankCashReportDTO dto);
    }
}
