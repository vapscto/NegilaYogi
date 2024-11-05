using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.FeedBack
{
    public class FeedbackTypeQuestionMappingDelegate
    {
        CommonDelegate<FeedbackTypeQuestionMappingDTO, FeedbackTypeQuestionMappingDTO> _delgate = new CommonDelegate<FeedbackTypeQuestionMappingDTO, FeedbackTypeQuestionMappingDTO>();

        CommonDelegate<FeedbackTypeOptionMappingDTO, FeedbackTypeOptionMappingDTO> _delgateoption = new CommonDelegate<FeedbackTypeOptionMappingDTO, FeedbackTypeOptionMappingDTO>();

        public FeedbackTypeQuestionMappingDTO getdetails(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/getdetails");
        }
        public FeedbackTypeQuestionMappingDTO onchnagetype(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/onchnagetype");
        }
        public FeedbackTypeQuestionMappingDTO savedata(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/savedata");
        }
        public FeedbackTypeQuestionMappingDTO activedeactive(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/activedeactive");
        }
        public FeedbackTypeQuestionMappingDTO getorder(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/getorder");
        }
        public FeedbackTypeQuestionMappingDTO getquestionwiseoption(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/getquestionwiseoption");
        }
        public FeedbackTypeQuestionMappingDTO onchangequestion(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/onchangequestion");
        }
        public FeedbackTypeQuestionMappingDTO savedatanew(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/savedatanew");
        }
        public FeedbackTypeQuestionMappingDTO deactiveoption(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/deactiveoption");
        }
        public FeedbackTypeQuestionMappingDTO getordernew(FeedbackTypeQuestionMappingDTO data)
        {
            return _delgate.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/getordernew");
        }

        // Type Option Mapping 

        public FeedbackTypeOptionMappingDTO optiongetdetails(FeedbackTypeOptionMappingDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/optiongetdetails");
        }
        public FeedbackTypeOptionMappingDTO optiononchnagetype(FeedbackTypeOptionMappingDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/optiononchnagetype");
        }
        public FeedbackTypeOptionMappingDTO optionsavedata(FeedbackTypeOptionMappingDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/optionsavedata");
        }
        public FeedbackTypeOptionMappingDTO optionactivedeactive(FeedbackTypeOptionMappingDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/optionactivedeactive");
        }
        public FeedbackTypeOptionMappingDTO optiongetorder(FeedbackTypeOptionMappingDTO data)
        {
            return _delgateoption.naacdetailsbypost(data, "FeedbackTypeQuestionMappingFacade/optiongetorder");
        }
    }
}
