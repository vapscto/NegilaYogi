using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeConsolidatedReportInterface
    {
        FeeConsolidatedReportDTO getdata123(FeeConsolidatedReportDTO data);

        Task<FeeConsolidatedReportDTO> getreport(FeeConsolidatedReportDTO data);
    }
}
