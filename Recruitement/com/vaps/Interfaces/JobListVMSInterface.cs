using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface JobListVMSInterface
    {
        HR_MRFRequisitionDTO getBasicData(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO SaveUpdate(HR_MRFRequisitionDTO dto);
        HR_MRFRequisitionDTO editData(HR_MRFRequisitionDTO dto);
    }
}
