using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class mappingDelgate

    {
        CommonDelegate<mappingDTO, mappingDTO> COMMC = new CommonDelegate<mappingDTO, mappingDTO>();
        public mappingDTO loaddata(int id)
        {
            return COMMC.GetDataByPlacement(id, "mappingFacade/loaddata/");
        }

        //POSTDataClubManagement
        public mappingDTO savedata(mappingDTO data)
        {
            return COMMC.POSTDataPlacement(data, "mappingFacade/savedata/");
        }
        public mappingDTO edit(mappingDTO data)
        {
            return COMMC.POSTDataPlacement(data, "mappingFacade/edit/");
        }
        public mappingDTO deactive(mappingDTO data)
        {
            return COMMC.POSTDataPlacement(data, "mappingFacade/deactive/");
        }
    }
}
