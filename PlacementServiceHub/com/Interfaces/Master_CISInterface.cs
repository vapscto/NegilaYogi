using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface Master_CISInterface
    {
        PL_CampusInterview_ScheduleDTO loaddata(int dto);
        PL_CampusInterview_ScheduleDTO savedata(PL_CampusInterview_ScheduleDTO data);
        PL_CampusInterview_ScheduleDTO edit(PL_CampusInterview_ScheduleDTO data);
        PL_CampusInterview_ScheduleDTO deactive(PL_CampusInterview_ScheduleDTO data);
        
    }
}
