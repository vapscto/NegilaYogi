using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface masterPositionTypeInterface
    {
        HR_Master_PostionTypeDTO getBasicData(HR_Master_PostionTypeDTO dto);
        HR_Master_PostionTypeDTO SaveUpdate(HR_Master_PostionTypeDTO dto);
        HR_Master_PostionTypeDTO getdata(HR_Master_PostionTypeDTO dto);
        HR_Master_PostionTypeDTO editData(int id);

        HR_Master_PostionTypeDTO deactivate(HR_Master_PostionTypeDTO dto);
    }
}
