using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterGroupTypeInterface
    {
        HR_Master_GroupTypeDTO getBasicData(HR_Master_GroupTypeDTO dto);
        HR_Master_GroupTypeDTO changeorderData(HR_Master_GroupTypeDTO dto);
        HR_Master_GroupTypeDTO SaveUpdate(HR_Master_GroupTypeDTO dto);
        HR_Master_GroupTypeDTO editData(int id);

        HR_Master_GroupTypeDTO deactivate(HR_Master_GroupTypeDTO dto);

    }
}
