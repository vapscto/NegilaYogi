using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces.FinancialAccounting
{
    public interface FinancialAccountingReportInterface
    {
        FinancialAccountingReportDTO GetInitialData(FinancialAccountingReportDTO data);
        FinancialAccountingReportDTO getReport(FinancialAccountingReportDTO data);
        FinancialAccountingReportDTO subreport(FinancialAccountingReportDTO data);
    }
}
