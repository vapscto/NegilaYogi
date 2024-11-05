using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface Master80CInterface
    {
        HR_Master_80CDTO getBasicData(HR_Master_80CDTO dto);
        HR_Master_80CDTO SaveUpdate(HR_Master_80CDTO dto);
        HR_Master_80CDTO editData(int id);

        HR_Master_80CDTO deactivate(HR_Master_80CDTO dto);
    }
}
