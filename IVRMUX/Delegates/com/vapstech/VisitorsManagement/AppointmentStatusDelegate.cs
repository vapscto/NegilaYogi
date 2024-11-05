using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class AppointmentStatusDelegate
    {
        CommonDelegate<AppointmentStatusDTO, AppointmentStatusDTO> COMVISITOR = new CommonDelegate<AppointmentStatusDTO, AppointmentStatusDTO>();
        public AppointmentStatusDTO getDetails(AppointmentStatusDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AppointmentStatusFacade/getDetails/");
        }

        public AppointmentStatusDTO EditDetails(AppointmentStatusDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "AppointmentStatusFacade/EditDetails/");
        }
        public AppointmentStatusDTO saveData(AppointmentStatusDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "AppointmentStatusFacade/saveData/");
        }

    }
}
