using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class MasterSportsCCNameUOM_Delegate
    {
        CommonDelegate<MasterSportsCCNameUMO_DTO, MasterSportsCCNameUMO_DTO> COMSPRT = new CommonDelegate<MasterSportsCCNameUMO_DTO, MasterSportsCCNameUMO_DTO>();

        public MasterSportsCCNameUMO_DTO getDetails(MasterSportsCCNameUMO_DTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterSportsCCNameUOM_Facade/getDetails/");
        }
        public MasterSportsCCNameUMO_DTO save(MasterSportsCCNameUMO_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSportsCCNameUOM_Facade/save/");
        }
        public MasterSportsCCNameUMO_DTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdSports(id, "MasterSportsCCNameUOM_Facade/EditDetails/");
        }
        public MasterSportsCCNameUMO_DTO deactivate(MasterSportsCCNameUMO_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSportsCCNameUOM_Facade/deactivate/");
        }
    }
}
