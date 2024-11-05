using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.FeedBack
{
    public class FeedBackMasterTypeDelegate
    {
        CommonDelegate<FeedBackMasterTypeDTO, FeedBackMasterTypeDTO> _delgate = new CommonLibrary.CommonDelegate<FeedBackMasterTypeDTO, FeedBackMasterTypeDTO>();
        CommonDelegate<Feedback_Master_QuestionDTO, Feedback_Master_QuestionDTO> _delgatequestions = new CommonLibrary.CommonDelegate<Feedback_Master_QuestionDTO, Feedback_Master_QuestionDTO>();
        CommonDelegate<Feedback_Master_OptionDTO, Feedback_Master_OptionDTO> _delgateoption = new CommonDelegate<Feedback_Master_OptionDTO, Feedback_Master_OptionDTO>();

        public FeedBackMasterTypeDTO getdetails(FeedBackMasterTypeDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedBackMasterTypeFacade/getdetails");
        }
        public FeedBackMasterTypeDTO savedata(FeedBackMasterTypeDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedBackMasterTypeFacade/savedata");
        }
        public FeedBackMasterTypeDTO editdata(FeedBackMasterTypeDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedBackMasterTypeFacade/editdata");
        }
        public FeedBackMasterTypeDTO activedeactive(FeedBackMasterTypeDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedBackMasterTypeFacade/activedeactive");
        }
        public FeedBackMasterTypeDTO getOrder(FeedBackMasterTypeDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedBackMasterTypeFacade/getOrder");
        }
        // Feedback Master Questions
        public Feedback_Master_QuestionDTO getquestiondetails(Feedback_Master_QuestionDTO data)
        {
            return _delgatequestions.naacdetailsbypost(data, "FeedBackMasterTypeFacade/getquestiondetails");
        }
        public Feedback_Master_QuestionDTO questionssavedata(Feedback_Master_QuestionDTO data)
        {
            return _delgatequestions.naacdetailsbypost(data, "FeedBackMasterTypeFacade/questionssavedata");
        }
        public Feedback_Master_QuestionDTO questionseditdata(Feedback_Master_QuestionDTO data)
        {
            return _delgatequestions.naacdetailsbypost(data, "FeedBackMasterTypeFacade/questionseditdata");
        }
        public Feedback_Master_QuestionDTO questionsactivedeactive(Feedback_Master_QuestionDTO data)
        {
            return _delgatequestions.naacdetailsbypost(data, "FeedBackMasterTypeFacade/questionsactivedeactive");
        }
        public Feedback_Master_QuestionDTO questionsgetOrder(Feedback_Master_QuestionDTO data)
        {
            return _delgatequestions.naacdetailsbypost(data, "FeedBackMasterTypeFacade/questionsgetOrder");
        }
        // Feedback Master Questions
        public Feedback_Master_OptionDTO getoptiondetails(Feedback_Master_OptionDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedBackMasterTypeFacade/getoptiondetails");
        }
        public Feedback_Master_OptionDTO optionsavedata(Feedback_Master_OptionDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedBackMasterTypeFacade/optionsavedata");
        }
        public Feedback_Master_OptionDTO optioneditdata(Feedback_Master_OptionDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedBackMasterTypeFacade/optioneditdata");
        }
        public Feedback_Master_OptionDTO optionactivedeactive(Feedback_Master_OptionDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedBackMasterTypeFacade/optionactivedeactive");
        }
        public Feedback_Master_OptionDTO optiongetOrder(Feedback_Master_OptionDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedBackMasterTypeFacade/optiongetOrder");
        }

    }
}
