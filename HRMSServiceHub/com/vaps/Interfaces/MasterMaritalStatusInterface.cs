using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterMaritalStatusInterface
    {
        IVRM_Master_Marital_StatusDTO getBasicData(IVRM_Master_Marital_StatusDTO dto);
        IVRM_Master_Marital_StatusDTO SaveUpdate(IVRM_Master_Marital_StatusDTO dto);
        IVRM_Master_Marital_StatusDTO editData(int id);

        IVRM_Master_Marital_StatusDTO deactivate(IVRM_Master_Marital_StatusDTO dto);
    }
}
