using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Interface
{
    public interface FeedbackTypeQuestionMappingInterface
    {
        FeedbackTypeQuestionMappingDTO getdetails(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO onchnagetype(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO savedata(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO activedeactive(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO getorder(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO getquestionwiseoption(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO onchangequestion(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO savedatanew(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO deactiveoption(FeedbackTypeQuestionMappingDTO data);
        FeedbackTypeQuestionMappingDTO getordernew(FeedbackTypeQuestionMappingDTO data);

        // Type Option Mapping 

        FeedbackTypeOptionMappingDTO optiongetdetails(FeedbackTypeOptionMappingDTO data);
        FeedbackTypeOptionMappingDTO optiononchnagetype(FeedbackTypeOptionMappingDTO data);
        FeedbackTypeOptionMappingDTO optionsavedata(FeedbackTypeOptionMappingDTO data);
        FeedbackTypeOptionMappingDTO optionactivedeactive(FeedbackTypeOptionMappingDTO data);
        FeedbackTypeOptionMappingDTO optiongetorder(FeedbackTypeOptionMappingDTO data);
    }
}
