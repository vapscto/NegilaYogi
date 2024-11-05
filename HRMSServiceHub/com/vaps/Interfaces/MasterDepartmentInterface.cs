using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterDepartmentInterface
    {
        HR_Master_DepartmentDTO getBasicData(HR_Master_DepartmentDTO dto);

        HR_Master_DepartmentDTO changeorderData(HR_Master_DepartmentDTO dto);
        HR_Master_DepartmentDTO SaveUpdate(HR_Master_DepartmentDTO dto);
        HR_Master_DepartmentDTO editData(int id);

        HR_Master_DepartmentDTO deactivate(HR_Master_DepartmentDTO dto);
    }
}
