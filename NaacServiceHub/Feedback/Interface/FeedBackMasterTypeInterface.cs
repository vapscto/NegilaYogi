using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Interface
{
    public interface FeedBackMasterTypeInterface
    {
        // Feedback Master Type
        FeedBackMasterTypeDTO getdetails(FeedBackMasterTypeDTO data);
        FeedBackMasterTypeDTO savedata(FeedBackMasterTypeDTO data);
        FeedBackMasterTypeDTO editdata(FeedBackMasterTypeDTO data);
        FeedBackMasterTypeDTO activedeactive(FeedBackMasterTypeDTO data);
        FeedBackMasterTypeDTO getOrder(FeedBackMasterTypeDTO data);

        // Feedback Master Questions
        Feedback_Master_QuestionDTO getquestiondetails(Feedback_Master_QuestionDTO data);
        Feedback_Master_QuestionDTO questionssavedata(Feedback_Master_QuestionDTO data);
        Feedback_Master_QuestionDTO questionseditdata(Feedback_Master_QuestionDTO data);
        Feedback_Master_QuestionDTO questionsactivedeactive(Feedback_Master_QuestionDTO data);
        Feedback_Master_QuestionDTO questionsgetOrder(Feedback_Master_QuestionDTO data);
        // Feedback Master Option
        Feedback_Master_OptionDTO getoptiondetails(Feedback_Master_OptionDTO data);
        Feedback_Master_OptionDTO optionsavedata(Feedback_Master_OptionDTO data);
        Feedback_Master_OptionDTO optioneditdata(Feedback_Master_OptionDTO data);
        Feedback_Master_OptionDTO optionactivedeactive(Feedback_Master_OptionDTO data);
        Feedback_Master_OptionDTO optiongetOrder(Feedback_Master_OptionDTO data);

    }
}
