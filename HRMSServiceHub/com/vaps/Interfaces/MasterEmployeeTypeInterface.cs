using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterEmployeeTypeInterface
    {
        HR_Master_EmployeeTypeDTO getBasicData(HR_Master_EmployeeTypeDTO dto);
        HR_Master_EmployeeTypeDTO SaveUpdate(HR_Master_EmployeeTypeDTO dto);
        HR_Master_EmployeeTypeDTO editData(int id);

        HR_Master_EmployeeTypeDTO deactivate(HR_Master_EmployeeTypeDTO dto);

    }
}
