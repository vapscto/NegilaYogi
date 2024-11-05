using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface PlicyInterface
    {
        Task<LeavepolicyDTO> getpolicyAsync(LeavepolicyDTO data);
    }
}
