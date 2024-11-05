using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface MasterEventsInterface
    {
        MasterEventsDTO getDetails(MasterEventsDTO data);
        MasterEventsDTO saveRecord(MasterEventsDTO obj);
        MasterEventsDTO EditDetails(MasterEventsDTO data);
        MasterEventsDTO deactivate(MasterEventsDTO obj);
    }
}
