 using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PL_CI_Schedule_Company_JobTitleDelegate
    {
        CommonDelegate<PL_CI_Schedule_Company_JobTitleDTO, PL_CI_Schedule_Company_JobTitleDTO> COMMC = new CommonDelegate<PL_CI_Schedule_Company_JobTitleDTO, PL_CI_Schedule_Company_JobTitleDTO>();
        public PL_CI_Schedule_Company_JobTitleDTO loaddata(int id)
        {
            return COMMC.GetDataByPlacement(id, "PL_CI_Schedule_Company_JobTitleFacade/loaddata/");
        }

        public PL_CI_Schedule_Company_JobTitleDTO saveRecord(PL_CI_Schedule_Company_JobTitleDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_Company_JobTitleFacade/saveRecord/");
        }

        public PL_CI_Schedule_Company_JobTitleDTO EditDetails(PL_CI_Schedule_Company_JobTitleDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_Company_JobTitleFacade/EditDetails/");
        }

        public PL_CI_Schedule_Company_JobTitleDTO deactivate(PL_CI_Schedule_Company_JobTitleDTO obj)
        {
            return COMMC.POSTDataPlacement(obj, "PL_CI_Schedule_Company_JobTitleFacade/deactivate/");
        }
    }
}
