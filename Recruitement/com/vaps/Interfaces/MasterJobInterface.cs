using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface MasterJobInterface
    {
        HR_Master_JobsDTO getBasicData(HR_Master_JobsDTO dto);
        HR_Master_JobsDTO SaveUpdate(HR_Master_JobsDTO dto);
        HR_Master_JobsDTO editData(int id);

        HR_Master_JobsDTO deactivate(HR_Master_JobsDTO dto);
    }
}
