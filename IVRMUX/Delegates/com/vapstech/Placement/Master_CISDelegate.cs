using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class Master_CISDelgate
        
    {
        CommonDelegate<PL_CampusInterview_ScheduleDTO, PL_CampusInterview_ScheduleDTO> COMMC = new CommonDelegate<PL_CampusInterview_ScheduleDTO, PL_CampusInterview_ScheduleDTO>();
        public PL_CampusInterview_ScheduleDTO loaddata(int id)
        {
            return COMMC.GetDataByPlacement(id, "Master_CISFacade/loaddata/");
        }

        public PL_CampusInterview_ScheduleDTO savedata(PL_CampusInterview_ScheduleDTO data)
        {
            return COMMC.POSTDataPlacement(data, "Master_CISFacade/savedata/");
        }

        public PL_CampusInterview_ScheduleDTO edit(PL_CampusInterview_ScheduleDTO data)
        {
            return COMMC.POSTDataPlacement(data, "Master_CISFacade/edit/");
        }
        public PL_CampusInterview_ScheduleDTO deactive(PL_CampusInterview_ScheduleDTO data)
        {
            return COMMC.POSTDataPlacement(data, "Master_CISFacade/deactive/");
        }
    }
}
