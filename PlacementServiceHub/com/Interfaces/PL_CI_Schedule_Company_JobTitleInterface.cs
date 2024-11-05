 using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PL_CI_Schedule_Company_JobTitleInterface
    {
        PL_CI_Schedule_Company_JobTitleDTO loaddata(int dto);
        PL_CI_Schedule_Company_JobTitleDTO saveRecord(PL_CI_Schedule_Company_JobTitleDTO data);
        PL_CI_Schedule_Company_JobTitleDTO EditDetails(PL_CI_Schedule_Company_JobTitleDTO data);
        PL_CI_Schedule_Company_JobTitleDTO deactivate(PL_CI_Schedule_Company_JobTitleDTO obj);

    }
}

