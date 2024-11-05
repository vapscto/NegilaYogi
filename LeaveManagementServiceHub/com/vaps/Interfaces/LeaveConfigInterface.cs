using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveConfigInterface
    {
        HR_Leave_Policy_Config_DTO save(HR_Leave_Policy_Config_DTO data);
        HR_Leave_Policy_Config_DTO getSPName(HR_Leave_Policy_Config_DTO data);
    }
}
