 using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PL_CI_Schedule_Company_JobTitle_CourseBranchInterface
    {
        PL_CI_Schedule_Company_JobTitle_CourseBranchDTO loaddata(int dto);
        PL_CI_Schedule_Company_JobTitle_CourseBranchDTO saveRecord(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data);
        PL_CI_Schedule_Company_JobTitle_CourseBranchDTO EditDetails(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data);
        PL_CI_Schedule_Company_JobTitle_CourseBranchDTO deactivate(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO obj);
    }
}

