using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterAllowanceInterface
    {
        MasterAllowanceDTO getBasicData(MasterAllowanceDTO dto);
        MasterAllowanceDTO SaveUpdate(MasterAllowanceDTO dto);
        MasterAllowanceDTO editData(int id);

        MasterAllowanceDTO deactivate(MasterAllowanceDTO dto);
    }
}
