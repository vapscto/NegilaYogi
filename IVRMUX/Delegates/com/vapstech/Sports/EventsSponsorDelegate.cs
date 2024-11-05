using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class EventsSponsorDelegate
    {
        CommonDelegate<EventsSponsorDTO, EventsSponsorDTO> COMSPRT = new CommonDelegate<EventsSponsorDTO, EventsSponsorDTO>();

        public EventsSponsorDTO getDetails(EventsSponsorDTO data)
        {
            return COMSPRT.POSTDataSports(data, "EventsSponsorFacade/getDetails/");
        }
        public EventsSponsorDTO save(EventsSponsorDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsSponsorFacade/save/");
        }
        public EventsSponsorDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdSports(id, "EventsSponsorFacade/EditDetails/");
        }
        public EventsSponsorDTO deactivate(EventsSponsorDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventsSponsorFacade/deactivate/");
        }
    }
}
