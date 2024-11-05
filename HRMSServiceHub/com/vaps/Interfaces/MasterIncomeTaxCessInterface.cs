using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterIncomeTaxCessInterface
    {
        HR_Master_IncomeTax_CessDTO getBasicData(HR_Master_IncomeTax_CessDTO dto);
        HR_Master_IncomeTax_CessDTO SaveUpdate(HR_Master_IncomeTax_CessDTO dto);
        HR_Master_IncomeTax_CessDTO editData(int id);

        HR_Master_IncomeTax_CessDTO deactivate(HR_Master_IncomeTax_CessDTO dto);

    }
}
