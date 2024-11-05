using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface Departmentsalaryinterface
    {
        HR_Department_SalaryDTO getBasicData(HR_Department_SalaryDTO dto);
        Task<HR_Department_SalaryDTO> getEmployeedetailsBySelection(HR_Department_SalaryDTO dto);

        HR_Department_SalaryDTO get_depts(HR_Department_SalaryDTO dto);
        HR_Department_SalaryDTO get_desig(HR_Department_SalaryDTO dto);
    }
}
