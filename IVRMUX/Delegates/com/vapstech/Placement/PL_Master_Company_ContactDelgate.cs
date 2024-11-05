using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PL_Master_Company_ContactDelgate
    {
        CommonDelegate<PL_Master_Company_ContactDTO, PL_Master_Company_ContactDTO> COMMC = new CommonDelegate<PL_Master_Company_ContactDTO, PL_Master_Company_ContactDTO>();
        public PL_Master_Company_ContactDTO loaddata(PL_Master_Company_ContactDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_Master_Company_ContactFacade/loaddata/");
        }
        //POSTDataClubManagement
        public PL_Master_Company_ContactDTO savedata(PL_Master_Company_ContactDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_Master_Company_ContactFacade/savedata/");
        }
        public PL_Master_Company_ContactDTO deactive(PL_Master_Company_ContactDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_Master_Company_ContactFacade/deactive/");
        }
    }
}
