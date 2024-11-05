using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterIncomeTaxDetailsCessInterface
    {
        HR_Master_IncomeTax_Details_CessDTO getBasicData(HR_Master_IncomeTax_Details_CessDTO dto);
        HR_Master_IncomeTax_Details_CessDTO SaveUpdate(HR_Master_IncomeTax_Details_CessDTO dto);
        HR_Master_IncomeTax_Details_CessDTO editData(int id);
        HR_Master_IncomeTax_Details_CessDTO deactivate(HR_Master_IncomeTax_Details_CessDTO dto);

    }
}
