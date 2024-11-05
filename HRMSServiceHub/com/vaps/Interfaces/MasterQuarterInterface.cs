using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterQuarterInterface
    {
        HR_Master_QuarterDTO getBasicData(HR_Master_QuarterDTO dto);
        HR_Master_QuarterDTO SaveUpdate(HR_Master_QuarterDTO dto);
        HR_Master_QuarterDTO editData(int id);

        HR_Master_QuarterDTO deactivate(HR_Master_QuarterDTO dto);
    }
}
