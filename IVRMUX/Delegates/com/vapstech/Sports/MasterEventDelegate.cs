using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class MasterEventDelegate
    {
        CommonDelegate<MasterEventsDTO, MasterEventsDTO> COMSPRT = new CommonDelegate<MasterEventsDTO, MasterEventsDTO>();

        public MasterEventsDTO getDetails(MasterEventsDTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterEventsFacade/getDetails/");
        }
        public MasterEventsDTO save(MasterEventsDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterEventsFacade/save/");
        }
        public MasterEventsDTO EditDetails(MasterEventsDTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterEventsFacade/EditDetails/");
        }
        public MasterEventsDTO deactivate(MasterEventsDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterEventsFacade/deactivate/");
        }
    }
}
