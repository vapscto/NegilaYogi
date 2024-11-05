
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class CISReportDelegate
    {
        CommonDelegate<PL_CampusInterview_ScheduleDTO, PL_CampusInterview_ScheduleDTO> COMMC = new CommonDelegate<PL_CampusInterview_ScheduleDTO, PL_CampusInterview_ScheduleDTO>();
        public PL_CampusInterview_ScheduleDTO getdetails(PL_CampusInterview_ScheduleDTO data)
        {
            return COMMC.POSTDataPlacement(data, "CISReportFacade/getdetails");
        }
        //report
        public PL_CampusInterview_ScheduleDTO report(PL_CampusInterview_ScheduleDTO data)
        {
            return COMMC.POSTDataPlacement(data, "CISReportFacade/report");
        }

    }
}


