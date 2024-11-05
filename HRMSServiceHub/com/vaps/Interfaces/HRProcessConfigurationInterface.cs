using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface HRProcessConfigurationInterface
    {
        HR_ProcessDTO getBasicData(HR_ProcessDTO dto);

        HR_ProcessDTO SaveUpdate(HR_ProcessDTO dto);
        HR_ProcessDTO editData(int id);


        HR_ProcessDTO deactivate(int id);

        HR_ProcessDTO deleteauth(HR_ProcessDTO data);


    }
}
