using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface EventsSponsorInterface
    {
        EventsSponsorDTO getDetails(EventsSponsorDTO data);
        EventsSponsorDTO saveRecord(EventsSponsorDTO obj);
        EventsSponsorDTO EditDetails(int id);
        EventsSponsorDTO deactivate(EventsSponsorDTO obj);
    }
}
