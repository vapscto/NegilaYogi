using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface ArrearSalaryReportInterface
    {
        HR_Arrear_SalaryDTO getBasicData(HR_Arrear_SalaryDTO dto);
        Task<HR_Arrear_SalaryDTO> getEmployeedetailsBySelection(HR_Arrear_SalaryDTO dto);

        HR_Arrear_SalaryDTO get_depts(HR_Arrear_SalaryDTO dto);
        HR_Arrear_SalaryDTO get_desig(HR_Arrear_SalaryDTO dto);
    }
}
