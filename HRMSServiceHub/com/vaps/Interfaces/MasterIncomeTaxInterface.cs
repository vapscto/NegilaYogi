using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterIncomeTaxInterface
    {
        HR_Master_IncomeTaxDTO getBasicData(HR_Master_IncomeTaxDTO dto);
        HR_Master_IncomeTaxDTO SaveUpdate(HR_Master_IncomeTaxDTO dto);
        HR_Master_IncomeTaxDTO editData(int id);
        HR_Master_IncomeTaxDTO deactivate(HR_Master_IncomeTaxDTO dto);

    }
}
