
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface CISReportInterface
    {
        PL_CampusInterview_ScheduleDTO getdetails(PL_CampusInterview_ScheduleDTO data);
        PL_CampusInterview_ScheduleDTO report(PL_CampusInterview_ScheduleDTO data);
      
    }
}
