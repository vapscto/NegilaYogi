using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface MasterLocationInterface
    {
        HR_Master_LocationDTO getBasicData(HR_Master_LocationDTO dto);
        HR_Master_LocationDTO SaveUpdate(HR_Master_LocationDTO dto);
        HR_Master_LocationDTO getdata(HR_Master_LocationDTO dto);
        HR_Master_LocationDTO editData(int id);
        HR_Master_LocationDTO deactivate(HR_Master_LocationDTO dto);
    }
}
