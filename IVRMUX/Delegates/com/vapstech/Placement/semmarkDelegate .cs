using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class semmarkDelgate

    {
        CommonDelegate<semmarkDTO, semmarkDTO> COMMC = new CommonDelegate<semmarkDTO, semmarkDTO>();
        public semmarkDTO loaddata(int id)
        {
            return COMMC.GetDataByPlacement(id, "semmarkFacade/loaddata/");
        }
        
        public semmarkDTO savedata(semmarkDTO data)
        {
            return COMMC.POSTDataPlacement(data, "semmarkFacade/savedata/");
        }
        public semmarkDTO edit(semmarkDTO data)
        {
            return COMMC.POSTDataPlacement(data, "semmarkFacade/edit/");
        }
        public semmarkDTO deactive(semmarkDTO data)
        {
            return COMMC.POSTDataPlacement(data, "semmarkFacade/deactive/");
        }

    }
}
