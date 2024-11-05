using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface GroupDeptDessgInterface
    {
        HRGroupDeptDessgDTO getBasicData(HRGroupDeptDessgDTO dto);
        HRGroupDeptDessgDTO savedata(HRGroupDeptDessgDTO dto);
        HRGroupDeptDessgDTO Editdata(HRGroupDeptDessgDTO dto);
        HRGroupDeptDessgDTO masterDecative(HRGroupDeptDessgDTO dto);
    }
}
