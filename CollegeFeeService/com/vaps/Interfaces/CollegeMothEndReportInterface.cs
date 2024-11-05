using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeMothEndReportInterface
    {
        FeeMonthEndReportDTO getdata123(FeeMonthEndReportDTO data);
        Task<FeeMonthEndReportDTO> getreport(FeeMonthEndReportDTO data);
    }
}
