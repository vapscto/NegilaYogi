using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Interfaces
{
 public   interface InOutCallsReportInterface
    {
        Task<IVRSInOutCallsReportDTO> getreport(IVRSInOutCallsReportDTO data);
    }
}
