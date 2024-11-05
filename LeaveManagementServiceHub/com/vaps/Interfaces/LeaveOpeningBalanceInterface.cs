using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveOpeningBalanceInterface
    {
        LeaveCreditDTO getLeaveOB(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_Employe_ob(LeaveCreditDTO data);
        LeaveCreditDTO get_ob_Details(LeaveCreditDTO data);

        LeaveCreditDTO save(LeaveCreditDTO data);
        LeaveCreditDTO getpagedetails(int id);
        LeaveCreditDTO deletepages(int id);
    }
}
