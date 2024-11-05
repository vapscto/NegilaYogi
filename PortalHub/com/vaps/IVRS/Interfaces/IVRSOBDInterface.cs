using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Interfaces
{
   public interface IVRSOBDInterface
    {
        IVRSOBD getdetails(IVRSOBD data);
        IVRSOBD ivrgetstudetails(IVRSOBD data);
        IVRSOBD initiatecalls(IVRSOBD data);
        IVRSOBD initiatecallsmobiglitz(IVRSOBD data);
    }
}
