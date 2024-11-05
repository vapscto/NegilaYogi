using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface RegisterWagesInterface
    {
        HR_EmployeeRegisterDTO getBasicData(HR_EmployeeRegisterDTO dto);
        Task<HR_EmployeeRegisterDTO> getEmployeedetailsBySelection(HR_EmployeeRegisterDTO dto);

        HR_EmployeeRegisterDTO get_depts(HR_EmployeeRegisterDTO dto);
        HR_EmployeeRegisterDTO get_desig(HR_EmployeeRegisterDTO dto);
    }
}
