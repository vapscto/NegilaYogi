using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface FEESGroupHeadWiseDetailsSchoolInterface
    {

        FEESGroupHeadWiseDetailsSchoolDTO Getdetails(FEESGroupHeadWiseDetailsSchoolDTO data);
        FEESGroupHeadWiseDetailsSchoolDTO Getsectioncount(FEESGroupHeadWiseDetailsSchoolDTO data);
        FEESGroupHeadWiseDetailsSchoolDTO Getgroupclasscount(FEESGroupHeadWiseDetailsSchoolDTO data);

    }
    
}
