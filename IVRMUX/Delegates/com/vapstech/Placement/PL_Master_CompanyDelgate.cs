using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PL_Master_CompanyDelgate
    {
        CommonDelegate<PL_Master_CompanyDTO, PL_Master_CompanyDTO> COMMC = new CommonDelegate<PL_Master_CompanyDTO, PL_Master_CompanyDTO>();
        public PL_Master_CompanyDTO loaddata(PL_Master_CompanyDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_Master_CompanyFacade/loaddata/");
        }

        //POSTDataClubManagement
        public PL_Master_CompanyDTO savedata(PL_Master_CompanyDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_Master_CompanyFacade/savedata/");
        }
        public PL_Master_CompanyDTO deactive(PL_Master_CompanyDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_Master_CompanyFacade/deactive/");
        }
    }
}
