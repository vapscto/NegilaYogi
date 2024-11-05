using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface masterPositionInterface
    {
        HR_Master_PositionDTO getBasicData(HR_Master_PositionDTO dto);
        HR_Master_PositionDTO SaveUpdate(HR_Master_PositionDTO dto);
        HR_Master_PositionDTO editData(int id);
        HR_Master_PositionDTO getdata(HR_Master_PositionDTO dto);
        HR_Master_PositionDTO deactivate(HR_Master_PositionDTO dto);
    }
}
