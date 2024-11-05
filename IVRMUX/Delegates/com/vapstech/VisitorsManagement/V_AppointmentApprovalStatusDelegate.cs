using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class V_AppointmentApprovalStatusDelegate
    {
        CommonDelegate<AppointmentApprovalStatus_DTO, AppointmentApprovalStatus_DTO> COMMNFUN = new CommonDelegate<AppointmentApprovalStatus_DTO, AppointmentApprovalStatus_DTO>();
        public AppointmentApprovalStatus_DTO getDetails(AppointmentApprovalStatus_DTO obj)
        {
            return COMMNFUN.POSTDataVisitors(obj, "V_AppointmentApprovalStatusFacade/getDetails/");
        }
        public AppointmentApprovalStatus_DTO EditDetails(AppointmentApprovalStatus_DTO id)
        {
            return COMMNFUN.POSTDataVisitors(id, "V_AppointmentApprovalStatusFacade/EditDetails/");
        }
        public AppointmentApprovalStatus_DTO Editnew(AppointmentApprovalStatus_DTO id)
        {
            return COMMNFUN.POSTDataVisitors(id, "V_AppointmentApprovalStatusFacade/Editnew/");
        }
        public AppointmentApprovalStatus_DTO saveData(AppointmentApprovalStatus_DTO obj)
        {
            return COMMNFUN.POSTDataVisitors(obj, "V_AppointmentApprovalStatusFacade/saveData/");
        }
        public AppointmentApprovalStatus_DTO viewuploadflies(AppointmentApprovalStatus_DTO obj)
        {
            return COMMNFUN.POSTDataVisitors(obj, "V_AppointmentApprovalStatusFacade/viewuploadflies/");
        }
        public AppointmentApprovalStatus_DTO sendMOM(AppointmentApprovalStatus_DTO obj)
        {
            return COMMNFUN.POSTDataVisitors(obj, "V_AppointmentApprovalStatusFacade/sendMOM/");
        }
        public AppointmentApprovalStatus_DTO savefeedback(AppointmentApprovalStatus_DTO obj)
        {
            return COMMNFUN.POSTDataVisitors(obj, "V_AppointmentApprovalStatusFacade/savefeedback/");
        }
        public AppointmentApprovalStatus_DTO getfeedback(AppointmentApprovalStatus_DTO obj)
        {
            return COMMNFUN.POSTDataVisitors(obj, "V_AppointmentApprovalStatusFacade/getfeedback/");
        }
    }
}
