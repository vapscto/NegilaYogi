using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveCreditInterface
    {
        LeaveCreditDTO getleave(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_leavecode(LeaveCreditDTO data);
        LeaveCreditDTO SaveData(LeaveCreditDTO data);

        LeaveCreditDTO get_grade(LeaveCreditDTO data);

    }
}
