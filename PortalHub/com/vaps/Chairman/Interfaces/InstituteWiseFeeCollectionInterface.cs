using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Chairman.Interfaces
{
    public interface InstituteWiseFeeCollectionInterface
    {
        InstituteWiseFeeCollectionDTO Getdetails(InstituteWiseFeeCollectionDTO data);
        InstituteWiseFeeCollectionDTO Getsectioncount(InstituteWiseFeeCollectionDTO data);
    }
}
