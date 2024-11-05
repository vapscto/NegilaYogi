using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
   public interface TTMonthEndReportInterface
    {
        TTMonthEndReportDTO getdata123(TTMonthEndReportDTO data);
        Task<TTMonthEndReportDTO> getreport(TTMonthEndReportDTO data);
    }
}
