using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class MasterSportsCCGroupDelegate
    {
        CommonDelegate<MasterSportsCCGroupDTO, MasterSportsCCGroupDTO> COMSPRT = new CommonDelegate<MasterSportsCCGroupDTO, MasterSportsCCGroupDTO>();

        public MasterSportsCCGroupDTO getDetails(MasterSportsCCGroupDTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterSportsCCGroupFacade/getDetails/");
        }
        public MasterSportsCCGroupDTO save(MasterSportsCCGroupDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSportsCCGroupFacade/save/");
        }
        public MasterSportsCCGroupDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdSports(id, "MasterSportsCCGroupFacade/EditDetails/");
        }
        public MasterSportsCCGroupDTO deactivate(MasterSportsCCGroupDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterSportsCCGroupFacade/deactivate/");
        }
    }
}
