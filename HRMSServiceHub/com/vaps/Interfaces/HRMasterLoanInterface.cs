using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HRMasterLoanInterface
    {

    HRMasterLoanDTO getBasicData(HRMasterLoanDTO dto);
    HRMasterLoanDTO SaveUpdate(HRMasterLoanDTO dto);
    HRMasterLoanDTO editData(int id);

    HRMasterLoanDTO deactivate(HRMasterLoanDTO dto);

  }
}
