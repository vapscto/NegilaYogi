using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeSalarySlipInterfaceModified
    {
        HR_Employee_SalaryModifiedDTO getBasicData(HR_Employee_SalaryModifiedDTO dto);
        HR_Employee_SalaryModifiedDTO GetEmployeeDetailsByLeaveYearAndMonth(HR_Employee_SalaryModifiedDTO dto);

        HR_Employee_SalaryModifiedDTO GetEmployeeDetailsByLeaveYearAndMultiMonth(HR_Employee_SalaryModifiedDTO dto);
        Task<HR_Employee_SalaryModifiedDTO> GenerateEmployeeSalarySlipMultiMonth(HR_Employee_SalaryModifiedDTO dto);

        Task<HR_Employee_SalaryModifiedDTO> GenerateEmployeeSalarySlip(HR_Employee_SalaryModifiedDTO dto);
        Task<HR_Employee_SalaryModifiedDTO> SendEmailSMS(HR_Employee_SalaryModifiedDTO dto);
        HR_Employee_SalaryModifiedDTO get_depts(HR_Employee_SalaryModifiedDTO dto);
        //HR_Employee_SalaryModifiedDTO get_Months(HR_Employee_SalaryModifiedDTO dto);
        HR_Employee_SalaryModifiedDTO get_desig(HR_Employee_SalaryModifiedDTO dto);
    }
}
