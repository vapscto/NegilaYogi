using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface LeaveTransactionManualInterface
    {
        LeaveCreditDTO getLeavetransm(LeaveCreditDTO data);
        LeaveCreditDTO get_departments(LeaveCreditDTO data);
        LeaveCreditDTO get_designation(LeaveCreditDTO data);
        LeaveCreditDTO get_employee(LeaveCreditDTO data);
        LeaveCreditDTO get_Emp_lop(LeaveCreditDTO data);
        LeaveCreditDTO saveDATA(LeaveCreditDTO data);
        LeaveCreditDTO Deletedetails(LeaveCreditDTO data);
    }
}
