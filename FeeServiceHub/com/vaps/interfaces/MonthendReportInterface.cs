using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface MonthendReportInterface
    {
        FeeMonthEndReportDTO getdata123(FeeMonthEndReportDTO data);
        Task<FeeMonthEndReportDTO> getreport(FeeMonthEndReportDTO data);
    
    }
}
