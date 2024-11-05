using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FeeServiceHub.com.vaps.interfaces
{
    public interface MonthlyCollectionReportInterface
    {
        MonthlyCollectionReportDTO getdetails(MonthlyCollectionReportDTO data);

        MonthlyCollectionReportDTO getstuddet(MonthlyCollectionReportDTO data);

        Task<MonthlyCollectionReportDTO> getreport(MonthlyCollectionReportDTO datare);
    }
}
