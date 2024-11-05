using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class MasterSportsCCNameDelegate
    {
        CommonDelegate<MasterSportsCCNameDTO, MasterSportsCCNameDTO> COMSPRT = new CommonDelegate<MasterSportsCCNameDTO, MasterSportsCCNameDTO>();

        public MasterSportsCCNameDTO getDetails(MasterSportsCCNameDTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterSportsCCNameFacade/getDetails/");
        }
        public MasterSportsCCNameDTO save(MasterSportsCCNameDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSportsCCNameFacade/save/");
        }
        public MasterSportsCCNameDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdSports(id, "MasterSportsCCNameFacade/EditDetails/");
        }
        public MasterSportsCCNameDTO deactivate(MasterSportsCCNameDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSportsCCNameFacade/deactivate/");
        }
    }
}
