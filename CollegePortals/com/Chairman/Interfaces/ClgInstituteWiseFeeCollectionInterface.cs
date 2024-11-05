using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.Chairman.Interfaces
{
    public interface ClgInstituteWiseFeeCollectionInterface
    {
        ClgInstituteWiseFeeCollectionDTO Getdetails(ClgInstituteWiseFeeCollectionDTO data);
        ClgInstituteWiseFeeCollectionDTO Getsectioncount(ClgInstituteWiseFeeCollectionDTO data);

    }
}
