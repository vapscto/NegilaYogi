using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterGenderInterface
    {
        IVRM_Master_GenderDTO getBasicData(IVRM_Master_GenderDTO dto);
        IVRM_Master_GenderDTO SaveUpdate(IVRM_Master_GenderDTO dto);
        IVRM_Master_GenderDTO editData(int id);

        IVRM_Master_GenderDTO deactivate(IVRM_Master_GenderDTO dto);
    }
}
