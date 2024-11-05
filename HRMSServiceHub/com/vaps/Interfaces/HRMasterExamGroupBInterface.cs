using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface HRMasterExamGroupBInterface
    {
        HR_MasterExam_GroupBDTO getBasicData(HR_MasterExam_GroupBDTO dto);
        HR_MasterExam_GroupBDTO SaveUpdate(HR_MasterExam_GroupBDTO dto);
        HR_MasterExam_GroupBDTO editData(int id);

        HR_MasterExam_GroupBDTO deactivate(HR_MasterExam_GroupBDTO dto);
    }
}
