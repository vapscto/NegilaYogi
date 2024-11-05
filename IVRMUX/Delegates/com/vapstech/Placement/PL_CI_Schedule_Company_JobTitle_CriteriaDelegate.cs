 using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PL_CI_Schedule_Company_JobTitle_CriteriaDelegate
    {
        CommonDelegate<PL_CI_Schedule_Company_JobTitle_CriteriaDTO, PL_CI_Schedule_Company_JobTitle_CriteriaDTO> COMMC = new CommonDelegate<PL_CI_Schedule_Company_JobTitle_CriteriaDTO, PL_CI_Schedule_Company_JobTitle_CriteriaDTO>();
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO loaddata(int id)
        {
            return COMMC.GetDataByPlacement(id, "PL_CI_Schedule_Company_JobTitle_CriteriaFacade/loaddata/");
        }

        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO save(PL_CI_Schedule_Company_JobTitle_CriteriaDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_Company_JobTitle_CriteriaFacade/save/");
        }
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO EditDetails(PL_CI_Schedule_Company_JobTitle_CriteriaDTO data)
        {
            return COMMC.POSTDataPlacement(data, "PL_CI_Schedule_Company_JobTitle_CriteriaFacade/EditDetails/");
        }

        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO deactivate(PL_CI_Schedule_Company_JobTitle_CriteriaDTO obj)
        {
            return COMMC.POSTDataPlacement(obj, "PL_CI_Schedule_Company_JobTitle_CriteriaFacade/deactivate/");
        }
    }
}
