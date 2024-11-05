using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface AddCandidateInterviewVMSInterface
    {
        HR_CandidateInterviewScheduleDTO getBasicData(HR_CandidateInterviewScheduleDTO dto);
        HR_CandidateInterviewScheduleDTO getallgrade(HR_CandidateInterviewScheduleDTO dto);
        HR_CandidateInterviewScheduleDTO SaveUpdate(HR_CandidateInterviewScheduleDTO dto);
        HR_CandidateInterviewScheduleDTO editData(int id);
        HR_CandidateInterviewScheduleDTO deactivate(HR_CandidateInterviewScheduleDTO dto);
        HR_CandidateInterviewScheduleDTO getrpt(HR_CandidateInterviewScheduleDTO dto);
        HR_CandidateInterviewScheduleDTO savefeedback(HR_CandidateInterviewScheduleDTO dto);   
    }
}
