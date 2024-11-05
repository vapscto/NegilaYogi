using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Sports;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface YearEndReportInterface
    {
        YearEndReportDTO loadDrpDwn(YearEndReportDTO data);
        Task<YearEndReportDTO> getReport(YearEndReportDTO report);
        YearEndReportDTO getReportGraph(YearEndReportDTO report1);
        

    }
}
