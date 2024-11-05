using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface PremonthendreportInterface 
    {
        MonthEndReportDTO getdata123(MonthEndReportDTO data);
        Task<MonthEndReportDTO> getreport(MonthEndReportDTO data);
    }
}
