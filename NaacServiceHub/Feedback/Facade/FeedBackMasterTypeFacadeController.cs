using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.FeedBack.Interface;
using PreadmissionDTOs.FeedBack;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.FeedBack.Facade
{
    [Route("api/[controller]")]
    public class FeedBackMasterTypeFacadeController : Controller
    {
        public FeedBackMasterTypeInterface _intf;

        public FeedBackMasterTypeFacadeController(FeedBackMasterTypeInterface intf)
        {
            _intf = intf;
        }

        // Feedback Master Type
        [Route("getdetails")]
        public FeedBackMasterTypeDTO getdetails([FromBody]FeedBackMasterTypeDTO data)
        {
            return _intf.getdetails(data);
        }

        [Route("savedata")]
        public FeedBackMasterTypeDTO savedata([FromBody]FeedBackMasterTypeDTO data)
        {
            return _intf.savedata(data);
        }
        [Route("editdata")]
        public FeedBackMasterTypeDTO editdata([FromBody]FeedBackMasterTypeDTO data)
        {
            return _intf.editdata(data);
        }
        [Route("activedeactive")]
        public FeedBackMasterTypeDTO activedeactive([FromBody]FeedBackMasterTypeDTO data)
        {
            return _intf.activedeactive(data);
        }
        [Route("getOrder")]
        public FeedBackMasterTypeDTO getOrder([FromBody]FeedBackMasterTypeDTO data)
        {
            return _intf.getOrder(data);
        }

        // Feedback Master Questions
        [Route("getquestiondetails")]
        public Feedback_Master_QuestionDTO getquestiondetails([FromBody]Feedback_Master_QuestionDTO data)
        {
            return _intf.getquestiondetails(data);
        }

        [Route("questionssavedata")]
        public Feedback_Master_QuestionDTO questionssavedata([FromBody]Feedback_Master_QuestionDTO data)
        {
            return _intf.questionssavedata(data);
        }
        [Route("questionseditdata")]
        public Feedback_Master_QuestionDTO questionseditdata([FromBody]Feedback_Master_QuestionDTO data)
        {
            return _intf.questionseditdata(data);
        }
        [Route("questionsactivedeactive")]
        public Feedback_Master_QuestionDTO questionsactivedeactive([FromBody]Feedback_Master_QuestionDTO data)
        {
            return _intf.questionsactivedeactive(data);
        }
        [Route("questionsgetOrder")]
        public Feedback_Master_QuestionDTO questionsgetOrder([FromBody]Feedback_Master_QuestionDTO data)
        {
            return _intf.questionsgetOrder(data);
        }
        // Feedback Master Option
        [Route("getoptiondetails")]
        public Feedback_Master_OptionDTO getoptiondetails([FromBody]Feedback_Master_OptionDTO data)
        {
            return _intf.getoptiondetails(data);
        }

        [Route("optionsavedata")]
        public Feedback_Master_OptionDTO optionsavedata([FromBody]Feedback_Master_OptionDTO data)
        {
            return _intf.optionsavedata(data);
        }
        [Route("optioneditdata")]
        public Feedback_Master_OptionDTO optioneditdata([FromBody]Feedback_Master_OptionDTO data)
        {
            return _intf.optioneditdata(data);
        }
        [Route("optionactivedeactive")]
        public Feedback_Master_OptionDTO optionactivedeactive([FromBody]Feedback_Master_OptionDTO data)
        {
            return _intf.optionactivedeactive(data);
        }
        [Route("optiongetOrder")]
        public Feedback_Master_OptionDTO optiongetOrder([FromBody]Feedback_Master_OptionDTO data)
        {
            return _intf.optiongetOrder(data);
        }
    }
}
