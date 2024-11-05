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
    public class FeedbackTypeQuestionMappingFacadeController : Controller
    {
        public FeedbackTypeQuestionMappingInterface _intf;

        public FeedbackTypeQuestionMappingFacadeController(FeedbackTypeQuestionMappingInterface intf)
        {
            _intf = intf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public FeedbackTypeQuestionMappingDTO getdetails([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.getdetails(data);
        }
        [Route("onchnagetype")]
        public FeedbackTypeQuestionMappingDTO onchnagetype([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.onchnagetype(data);
        }
        [Route("savedata")]
        public FeedbackTypeQuestionMappingDTO savedata([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.savedata(data);
        }
        [Route("activedeactive")]
        public FeedbackTypeQuestionMappingDTO activedeactive([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.activedeactive(data);
        }
        [Route("getorder")]
        public FeedbackTypeQuestionMappingDTO getorder([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.getorder(data);
        }
        [Route("getquestionwiseoption")]
        public FeedbackTypeQuestionMappingDTO getquestionwiseoption([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.getquestionwiseoption(data);
        }
        [Route("onchangequestion")]
        public FeedbackTypeQuestionMappingDTO onchangequestion([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.onchangequestion(data);
        }
        [Route("savedatanew")]
        public FeedbackTypeQuestionMappingDTO savedatanew([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.savedatanew(data);
        }
        [Route("deactiveoption")]
        public FeedbackTypeQuestionMappingDTO deactiveoption([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.deactiveoption(data);
        }
        [Route("getordernew")]
        public FeedbackTypeQuestionMappingDTO getordernew([FromBody]FeedbackTypeQuestionMappingDTO data)
        {            
            return _intf.getordernew(data);
        }

        // Type Order  Mapping 

        [Route("optiongetdetails")]
        public FeedbackTypeOptionMappingDTO optiongetdetails([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            return _intf.optiongetdetails(data);
        }
        [Route("optiononchnagetype")]
        public FeedbackTypeOptionMappingDTO optiononchnagetype([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            return _intf.optiononchnagetype(data);
        }
        [Route("optionsavedata")]
        public FeedbackTypeOptionMappingDTO optionsavedata([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            return _intf.optionsavedata(data);
        }
        [Route("optionactivedeactive")]
        public FeedbackTypeOptionMappingDTO optionactivedeactive([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            return _intf.optionactivedeactive(data);
        }
        [Route("optiongetorder")]
        public FeedbackTypeOptionMappingDTO optiongetorder([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            return _intf.optiongetorder(data);
        }
    }
}
