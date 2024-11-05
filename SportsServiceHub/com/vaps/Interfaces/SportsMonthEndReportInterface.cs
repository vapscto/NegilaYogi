using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface SportsMonthEndReportInterface
    {
        SportsMonthEndReport_DTO getdeatils(SportsMonthEndReport_DTO data);

        SportsMonthEndReport_DTO GetReport(SportsMonthEndReport_DTO data);

        
    }
}
