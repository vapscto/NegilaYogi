using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
   public interface HRMasterPANInterface
    {

        HRMasterPANDTO getBasicData(HRMasterPANDTO dto);
        HRMasterPANDTO SaveUpdate(HRMasterPANDTO dto);
        HRMasterPANDTO editData(int id);

        HRMasterPANDTO deactivate(HRMasterPANDTO dto);

  }
}
