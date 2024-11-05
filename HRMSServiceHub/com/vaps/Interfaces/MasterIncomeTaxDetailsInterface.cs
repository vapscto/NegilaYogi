using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterIncomeTaxDetailsInterface
    {
        HR_Master_IncomeTax_DetailsDTO getBasicData(HR_Master_IncomeTax_DetailsDTO dto);
        HR_Master_IncomeTax_DetailsDTO SaveUpdate(HR_Master_IncomeTax_DetailsDTO dto);
        HR_Master_IncomeTax_DetailsDTO editData(int id);
        HR_Master_IncomeTax_DetailsDTO deactivate(HR_Master_IncomeTax_DetailsDTO dto);

    }
}
