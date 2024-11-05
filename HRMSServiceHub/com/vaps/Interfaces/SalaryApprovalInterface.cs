using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface SalaryApprovalInterface
    {
        //  HR_Emp_SalaryAdvanceDTO getBasicData(HR_Emp_SalaryAdvanceDTO dto);

        HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto);
    }
}
