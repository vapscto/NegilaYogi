using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterPayrollStandardInterface
    {
        HR_ConfigurationDTO getBasicData(HR_ConfigurationDTO dto);
        HR_ConfigurationDTO SaveUpdate(HR_ConfigurationDTO dto);
        HR_ConfigurationDTO editData(int id);
        HR_ConfigurationDTO deactivate(HR_ConfigurationDTO dto);
    }
}
