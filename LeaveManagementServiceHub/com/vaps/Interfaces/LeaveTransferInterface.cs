using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using PreadmissionDTOs.com.vaps.HRMS;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveTransferInterface
    {
        LeaveCreditDTO getBasicData(LeaveCreditDTO dto);
        LeaveCreditDTO getLeaveOB(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_Employe_ob(LeaveCreditDTO data);
        LeaveCreditDTO get_ob_Details(LeaveCreditDTO data);
        LeaveCreditDTO SaveDetails(LeaveCreditDTO data);
        LeaveCreditDTO SaveDetails11(LeaveCreditDTO data);
        LeaveCreditDTO leavecarryforward(LeaveCreditDTO data);
        LeaveCreditDTO deletepages(int id);
    }
}
