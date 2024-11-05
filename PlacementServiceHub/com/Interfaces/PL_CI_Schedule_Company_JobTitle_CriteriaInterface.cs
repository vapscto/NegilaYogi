
    using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PL_CI_Schedule_Company_JobTitle_CriteriaInterface
    {
        PL_CI_Schedule_Company_JobTitle_CriteriaDTO loaddata(int dto);
        PL_CI_Schedule_Company_JobTitle_CriteriaDTO saveRecord(PL_CI_Schedule_Company_JobTitle_CriteriaDTO data);
        PL_CI_Schedule_Company_JobTitle_CriteriaDTO EditDetails(PL_CI_Schedule_Company_JobTitle_CriteriaDTO data);
        PL_CI_Schedule_Company_JobTitle_CriteriaDTO deactivate(PL_CI_Schedule_Company_JobTitle_CriteriaDTO obj);

    }
}

