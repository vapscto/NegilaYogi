using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface FeeAccountsPositionInterface
    {
        FeeAccountsPositionReportDTO getdata(FeeAccountsPositionReportDTO data);
        FeeAccountsPositionReportDTO getgroupByCG(FeeAccountsPositionReportDTO data);
        FeeAccountsPositionReportDTO getReport(FeeAccountsPositionReportDTO obj);
    }
}
