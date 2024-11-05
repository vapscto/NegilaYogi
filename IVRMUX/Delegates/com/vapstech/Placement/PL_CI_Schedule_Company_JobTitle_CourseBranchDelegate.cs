 
    using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PL_CI_Schedule_Company_JobTitle_CourseBranchDelegate
    {
        CommonDelegate<PL_CI_Schedule_Company_JobTitle_CourseBranchDTO, PL_CI_Schedule_Company_JobTitle_CourseBranchDTO> COMMC = new CommonDelegate<PL_CI_Schedule_Company_JobTitle_CourseBranchDTO, PL_CI_Schedule_Company_JobTitle_CourseBranchDTO>();
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO loaddata(int id)
        {
            return COMMC.GetDataByPlacement(id, "PL_CI_Schedule_Company_JobTitle_CourseBranchFacade/loaddata/");
        }

        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO saveRecord(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_Company_JobTitle_CourseBranchFacade/saveRecord/");
        }
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO EditDetails(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_Company_JobTitle_CourseBranchFacade/EditDetails/");
        }

        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO deactivate(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO obj)
        {
            return COMMC.POSTDataPlacement(obj, "PL_CI_Schedule_Company_JobTitle_CourseBranchFacade/deactivate/");
        }
    }
}
