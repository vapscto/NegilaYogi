using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface CandidateInterviewListVMSInterface
    {
        HR_CandidateInterviewScheduleDTO getBasicData(HR_CandidateInterviewScheduleDTO dto);
        HR_CandidateInterviewScheduleDTO SaveUpdate(HR_CandidateInterviewScheduleDTO dto);
        Task<HR_CandidateInterviewScheduleDTO> editDataAsync(int id);
        HR_CandidateInterviewScheduleDTO getallwithoutcondtn(HR_CandidateInterviewScheduleDTO dto);
        HR_CandidateInterviewScheduleDTO deactivateRecordById(HR_CandidateInterviewScheduleDTO dto);
    }
}
