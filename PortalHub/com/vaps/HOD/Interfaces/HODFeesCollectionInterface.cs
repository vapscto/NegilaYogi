using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
   public interface HODFeesCollectionInterface
    {
        FEESOverAllStatusSchoolDTO Getdetails(FEESOverAllStatusSchoolDTO data);
        FEESOverAllStatusSchoolDTO Getsectioncount(FEESOverAllStatusSchoolDTO data);
    }
}
