using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface CandidateListVMSInterface
    {
        HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto);
        HR_Candidate_DetailsDTO editData(int id);

    }
}
