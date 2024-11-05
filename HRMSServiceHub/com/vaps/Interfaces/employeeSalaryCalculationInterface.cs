using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface employeeSalaryCalculationInterface
    {
        HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto);

        Task<HR_Employee_SalaryDTO> calculateSelectedEmployeeSalary(HR_Employee_SalaryDTO dto);

        HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO dto);

        HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO dto);
    }
}
