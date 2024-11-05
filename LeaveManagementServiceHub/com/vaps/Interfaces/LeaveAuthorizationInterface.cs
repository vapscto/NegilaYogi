using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveAuthorizationInterface
    {
        LeaveCreditDTO getAuthLeave(LeaveCreditDTO data);
        LeaveCreditDTO saveauthdata(LeaveCreditDTO data);
        LeaveCreditDTO getauthdata(LeaveCreditDTO data);
        LeaveCreditDTO editdetails(int id);
        LeaveCreditDTO deleteauth(LeaveCreditDTO id);
        LeaveCreditDTO getemployeelist(LeaveCreditDTO data);
    }
}
