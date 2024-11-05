using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveApprovalInterface
    {
        LeaveCreditDTO getApprovalStatus(LeaveCreditDTO data);
        Task<LeaveCreditDTO> get_status(LeaveCreditDTO data);
        Task<LeaveCreditDTO> reject_status(LeaveCreditDTO data);
        LeaveCreditDTO getApprovedLeave(LeaveCreditDTO data);
        LeaveCreditDTO Viewleavebalancehistory(LeaveCreditDTO data);

        LeaveCreditDTO getRequestStatus(LeaveCreditDTO data);
       
        Task<LeaveCreditDTO> get_approvestatusAsync(LeaveCreditDTO data);

    //periodwiseapproval///////////////////////////////////////////////////////////////////////
        LeaveCreditDTO getperiodApprovalStatus(LeaveCreditDTO data);
        LeaveCreditDTO periodleavestatus(LeaveCreditDTO data);
    }
}
