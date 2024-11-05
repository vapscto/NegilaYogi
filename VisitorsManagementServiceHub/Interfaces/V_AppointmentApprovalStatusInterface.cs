using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface V_AppointmentApprovalStatusInterface
    {
        AppointmentApprovalStatus_DTO getDetails(AppointmentApprovalStatus_DTO data);
        AppointmentApprovalStatus_DTO EditDetails(AppointmentApprovalStatus_DTO id);
        AppointmentApprovalStatus_DTO Editnew(AppointmentApprovalStatus_DTO data);
        Task<AppointmentApprovalStatus_DTO> saveDataAsync(AppointmentApprovalStatus_DTO data);
       AppointmentApprovalStatus_DTO viewuploadflies(AppointmentApprovalStatus_DTO data);
       AppointmentApprovalStatus_DTO sendMOM(AppointmentApprovalStatus_DTO data);
       AppointmentApprovalStatus_DTO ApprovalReminder(AppointmentApprovalStatus_DTO data);
       AppointmentApprovalStatus_DTO savefeedback(AppointmentApprovalStatus_DTO data);
       AppointmentApprovalStatus_DTO getfeedback(AppointmentApprovalStatus_DTO data);

    }
}
