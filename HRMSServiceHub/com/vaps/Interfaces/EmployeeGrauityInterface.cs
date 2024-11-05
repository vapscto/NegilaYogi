using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.HRMS;
namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeGrauityInterface
    {
        HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryDTO dto);
        Task<HR_Employee_SalaryDTO> GenerateEmployeeSalarySlip(HR_Employee_SalaryDTO dto);
    }
}
