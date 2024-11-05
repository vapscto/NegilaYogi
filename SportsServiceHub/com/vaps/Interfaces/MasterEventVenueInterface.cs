using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface MasterEventVenueInterface
    {
        MasterEventVenueDTO getDetails(MasterEventVenueDTO data);
        MasterEventVenueDTO saveRecord(MasterEventVenueDTO obj);
        MasterEventVenueDTO EditDetails(int id);
        MasterEventVenueDTO deactivate(MasterEventVenueDTO obj);
    }
}
