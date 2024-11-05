using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
    {
    public interface EmployeeSalaryDetailsInterface
    {
        HR_Employee_EarningsDeductionsDTO getBasicData(HR_Employee_EarningsDeductionsDTO dto);
        HR_Employee_EarningsDeductionsDTO SaveUpdate(HR_Employee_EarningsDeductionsDTO dto);

        HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetails(HR_Employee_EarningsDeductionsDTO dto);
        HR_Employee_EarningsDeductionsDTO getEmployeeSalaryDetailsByHead(HR_Employee_EarningsDeductionsDTO dto);
        HR_Employee_EarningsDeductionsDTO GetEmployeeDetailsBySelected(HR_Employee_EarningsDeductionsDTO dto);

        HR_Employee_EarningsDeductionsDTO get_depts(HR_Employee_EarningsDeductionsDTO dto);

        HR_Employee_EarningsDeductionsDTO get_desig(HR_Employee_EarningsDeductionsDTO dto);
    }
}
