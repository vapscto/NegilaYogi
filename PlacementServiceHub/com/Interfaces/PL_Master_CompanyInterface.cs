using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PL_Master_CompanyInterface
    {
        PL_Master_CompanyDTO loaddata(PL_Master_CompanyDTO data);
       
        PL_Master_CompanyDTO savedata(PL_Master_CompanyDTO data);
        PL_Master_CompanyDTO deactive(PL_Master_CompanyDTO data);
    }
}
