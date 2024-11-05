using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface ChairmanFeeAudcntInterface
    {

        ChairmanFeeAudcntDTO Getdetails(ChairmanFeeAudcntDTO data);
      
        ChairmanFeeAudcntDTO Getsectionpop(ChairmanFeeAudcntDTO data);

    }
    
}
