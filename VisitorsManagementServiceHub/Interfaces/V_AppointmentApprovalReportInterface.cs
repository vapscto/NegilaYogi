using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
   public interface V_AppointmentApprovalReportInterface
    {

        V_AppointmentApprovalReport_DTO loaddata(V_AppointmentApprovalReport_DTO data);
        V_AppointmentApprovalReport_DTO loaddatatoday(V_AppointmentApprovalReport_DTO data);
        Task<V_AppointmentApprovalReport_DTO> report(V_AppointmentApprovalReport_DTO data);
    }
}
