using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class MasterSponserDelegate
    {
        CommonDelegate<MasterSponserDTO, MasterSponserDTO> COMSPRT = new CommonDelegate<MasterSponserDTO, MasterSponserDTO>();

        public MasterSponserDTO getDetails(MasterSponserDTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterSponserFacade/getDetails/");
        }
        public MasterSponserDTO save(MasterSponserDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSponserFacade/save/");
        }
        public MasterSponserDTO EditDetails(int  id)
        {
            return COMSPRT.GetDataByIdSports(id, "MasterSponserFacade/EditDetails/");
        }
        public MasterSponserDTO deactivateSponser(MasterSponserDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSponserFacade/deactivateSponser/");
        }
        

    }
}
