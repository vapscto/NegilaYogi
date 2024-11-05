using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeAutopromotionInterface
    {

        HR_Emp_AutopromotionDTO getBasicData(HR_Emp_AutopromotionDTO dto);
        HR_Emp_AutopromotionDTO SaveUpdate(HR_Emp_AutopromotionDTO dto);
        HR_Emp_AutopromotionDTO editData(int id);
        HR_Emp_AutopromotionDTO deactivate(HR_Emp_AutopromotionDTO dto);
        HR_Emp_AutopromotionDTO getDetailsByEmployee(HR_Emp_AutopromotionDTO dto);

    }
}
