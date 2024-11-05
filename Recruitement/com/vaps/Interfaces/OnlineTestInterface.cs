using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface OnlineTestInterface
    {
        #region  MASTER QUESTION 
        //  MASTER QUESTION 
        OnlineTestDTO getmasterquestionloaddata(OnlineTestDTO data);
        OnlineTestDTO getqnspapertype(OnlineTestDTO data);
        OnlineTestDTO SaveMasterQuestionDetails(OnlineTestDTO data);
        OnlineTestDTO EditMasterQuestion(OnlineTestDTO data);
        OnlineTestDTO ViewMasterQuesDoc(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateDocument(OnlineTestDTO data);
        OnlineTestDTO ViewMasterQuesOptions(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateQuesOption(OnlineTestDTO data);
        OnlineTestDTO AddMoreOptions(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateQues(OnlineTestDTO data);
        #endregion



    }
}
