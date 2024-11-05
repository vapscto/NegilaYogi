using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PL_CI_Schedule_CompanyDelgate
    {
        CommonDelegate<PL_CI_Schedule_CompanyDTO, PL_CI_Schedule_CompanyDTO> COMMC = new CommonDelegate<PL_CI_Schedule_CompanyDTO, PL_CI_Schedule_CompanyDTO>();
        public PL_CI_Schedule_CompanyDTO loaddata(PL_CI_Schedule_CompanyDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_CompanyFacade/loaddata/");
        }
        //POSTDataClubManagement
        public PL_CI_Schedule_CompanyDTO savedata(PL_CI_Schedule_CompanyDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_CompanyFacade/savedata/");
        }
        public PL_CI_Schedule_CompanyDTO deactive(PL_CI_Schedule_CompanyDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_CompanyFacade/deactive/");
        }

        public PL_CI_Schedule_CompanyDTO editdetails(PL_CI_Schedule_CompanyDTO data)        {            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_CompanyFacade/editdetails");        }
    }
}
