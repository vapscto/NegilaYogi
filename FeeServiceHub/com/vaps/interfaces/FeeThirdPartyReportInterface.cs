using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeThirdPartyReportInterface
    {
        FeeThirdPartyReportDTO getdata123(FeeThirdPartyReportDTO data);

        Task<FeeThirdPartyReportDTO> getreport(FeeThirdPartyReportDTO data);
    }
}
