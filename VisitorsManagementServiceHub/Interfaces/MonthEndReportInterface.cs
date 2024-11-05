using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface MonthEndReportInterface
    {
        VisitorsMonthEndReport_DTO getdeatils(VisitorsMonthEndReport_DTO data);

        Task<VisitorsMonthEndReport_DTO> GetReport(VisitorsMonthEndReport_DTO data);

    }
}
