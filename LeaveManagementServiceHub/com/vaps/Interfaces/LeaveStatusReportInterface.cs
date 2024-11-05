using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveStatusReportInterface
    {
        LeaveCreditDTO getleavereport(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_Employees(LeaveCreditDTO data);
       Task<LeaveCreditDTO> get_report(LeaveCreditDTO data);


    }
}
