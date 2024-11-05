using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface OnlineTestCandidateInterface
    {
        #region  EXAM
      
        OnlineTestCandidateDTO loadExamdata(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO getQuestion(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO Saveanswer(OnlineTestCandidateDTO data);
        
        #endregion



    }
}
