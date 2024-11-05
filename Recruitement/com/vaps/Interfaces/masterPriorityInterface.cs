using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface masterPriorityInterface
    {
        HR_Master_PriorityDTO getBasicData(HR_Master_PriorityDTO dto);
        HR_Master_PriorityDTO SaveUpdate(HR_Master_PriorityDTO dto);
        HR_Master_PriorityDTO getdata(HR_Master_PriorityDTO dto);
        HR_Master_PriorityDTO editData(int id);

        HR_Master_PriorityDTO deactivate(HR_Master_PriorityDTO dto);
    }
}
