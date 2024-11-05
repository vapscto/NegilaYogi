using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Interfaces
{
  public interface CallDashboardInterface
    {
        Task<CallDashboardDTO> loadData(CallDashboardDTO data);
    }
}
