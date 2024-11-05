using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PL_Master_Company_ContactInterface
    {
        PL_Master_Company_ContactDTO loaddata(PL_Master_Company_ContactDTO data);
       
        PL_Master_Company_ContactDTO savedata(PL_Master_Company_ContactDTO data);
        PL_Master_Company_ContactDTO deactive(PL_Master_Company_ContactDTO data);
    }
}
