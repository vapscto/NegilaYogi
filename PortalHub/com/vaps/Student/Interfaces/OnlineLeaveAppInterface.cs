using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface OnlineLeaveAppInterface
    {
        OnlineLeaveApp_DTO getdetails(OnlineLeaveApp_DTO sddto);
        Task<OnlineLeaveApp_DTO> leaveapply(OnlineLeaveApp_DTO sddto);
        OnlineLeaveApp_DTO editdata(OnlineLeaveApp_DTO sddto);
        Task<OnlineLeaveApp_DTO> leaveApproved(OnlineLeaveApp_DTO sddto);
        Task<OnlineLeaveApp_DTO> leaveRejected(OnlineLeaveApp_DTO sddto);
        OnlineLeaveApp_DTO deactiveY(OnlineLeaveApp_DTO sddto);
        OnlineLeaveApp_DTO cancellationRecord(OnlineLeaveApp_DTO sddto);
        OnlineLeaveApp_DTO getdate_sla(OnlineLeaveApp_DTO sddto);
        OnlineLeaveApp_DTO getsection(OnlineLeaveApp_DTO sddto);
        OnlineLeaveApp_DTO getstudent(OnlineLeaveApp_DTO sddto);
        OnlineLeaveApp_DTO get_leave_Report(OnlineLeaveApp_DTO data);
        TransferCertificate_DTO get_TC_Report(TransferCertificate_DTO data);
    }
}
