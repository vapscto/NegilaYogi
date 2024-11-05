using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface HRMasterExamGroupAInterface
    {
        HR_MasterExam_GroupADTO getBasicData(HR_MasterExam_GroupADTO dto);
        HR_MasterExam_GroupADTO SaveUpdate(HR_MasterExam_GroupADTO dto);
        HR_MasterExam_GroupADTO editData(int id);

        HR_MasterExam_GroupADTO deactivate(HR_MasterExam_GroupADTO dto);
    }
}
