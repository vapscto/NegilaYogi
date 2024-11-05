using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterDesignationInterface
    {
        HR_Master_DesignationDTO getBasicData(HR_Master_DesignationDTO dto);
        HR_Master_DesignationDTO changeorderData(HR_Master_DesignationDTO dto);
        HR_Master_DesignationDTO SaveUpdate(HR_Master_DesignationDTO dto);
        HR_Master_DesignationDTO editData(int id);

        HR_Master_DesignationDTO deactivate(HR_Master_DesignationDTO dto);
    }
}
