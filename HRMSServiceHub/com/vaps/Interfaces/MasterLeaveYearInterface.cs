using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterLeaveYearInterface
    {
        HR_Master_LeaveYearDTO getBasicData(HR_Master_LeaveYearDTO dto);
        HR_Master_LeaveYearDTO SaveUpdate(HR_Master_LeaveYearDTO dto);
        HR_Master_LeaveYearDTO editData(int id);
        HR_Master_LeaveYearDTO deactivate(HR_Master_LeaveYearDTO dto);
        HR_Master_LeaveYearDTO validateordernumber(HR_Master_LeaveYearDTO dto);
    }
}
