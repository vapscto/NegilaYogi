using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface CumulativeSalaryReportInterface
    {
        HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto);
        Task<HR_Employee_SalaryDTO> getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto);
        Task<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report> getCumulativeSalaryReport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report dto);

        HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO dto);
        HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO dto);

        Task<HR_Employee_SalaryDTO> getEmployeedetailsByDepartment(HR_Employee_SalaryDTO dto);
        Task<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report> yearlyreport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report dto);
        Task<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report> headwisereport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report dto);
    }
}
