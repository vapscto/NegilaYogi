using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface EventVenueMappingInterface
    {
        EventsMappingDTO getDetails(EventsMappingDTO data);
        EventsMappingDTO saveRecord(EventsMappingDTO obj);
        Task<EventsMappingDTO> EditDetails(EventsMappingDTO id);
        EventsMappingDTO deactivate(EventsMappingDTO obj);
        EventsMappingDTO get_modeldata(EventsMappingDTO obj);
        EventsMappingDTO Deactivesponsor(EventsMappingDTO obj);
        

    }
}
