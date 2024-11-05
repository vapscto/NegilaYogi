using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class EventVenueMappingDelegate
    {
        CommonDelegate<EventsMappingDTO, EventsMappingDTO> COMSPRT = new CommonDelegate<EventsMappingDTO, EventsMappingDTO>();

        public EventsMappingDTO getDetails(EventsMappingDTO data)
        {
            return COMSPRT.POSTDataSports(data, "EventVenueMappingFacade/getDetails/");
        }
        public EventsMappingDTO save(EventsMappingDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventVenueMappingFacade/save/");
        }
        public EventsMappingDTO EditDetails(EventsMappingDTO id)
        {
            return COMSPRT.POSTDataSports(id, "EventVenueMappingFacade/EditDetails/");
        }
        public EventsMappingDTO deactivate(EventsMappingDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventVenueMappingFacade/deactivate/");
        }
        public EventsMappingDTO get_modeldata(EventsMappingDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventVenueMappingFacade/get_modeldata/");
        }
        public EventsMappingDTO Deactivesponsor(EventsMappingDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "EventVenueMappingFacade/Deactivesponsor/");
        }
        

    }
}
