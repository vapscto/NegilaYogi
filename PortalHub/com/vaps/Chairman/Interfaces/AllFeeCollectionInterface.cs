using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface AllFeeCollectionInterface
    {

        AllFeeCollectionDTO Getdetails(AllFeeCollectionDTO data);
        AllFeeCollectionDTO Getsectioncount(AllFeeCollectionDTO data);

    }
    
}
