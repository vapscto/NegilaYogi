using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface SalarySlipInterface
    {
        HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryDTO dto);
        Task<HR_Employee_SalaryDTO> GenerateEmployeeSalarySlip(HR_Employee_SalaryDTO dto);
          
    }
}
