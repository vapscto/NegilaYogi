using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.FeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.FeedBack;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.FeedBack
{
    [Route("api/[controller]")]
    public class FeedbackTypeQuestionMappingController : Controller
    {
        public FeedbackTypeQuestionMappingDelegate _delg = new FeedbackTypeQuestionMappingDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public FeedbackTypeQuestionMappingDTO getdetails([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getdetails(data);
        }
        [Route("onchnagetype")]
        public FeedbackTypeQuestionMappingDTO onchnagetype([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchnagetype(data);
        }
        [Route("savedata")]
        public FeedbackTypeQuestionMappingDTO savedata([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedata(data);
        }
        [Route("activedeactive")]
        public FeedbackTypeQuestionMappingDTO activedeactive([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.activedeactive(data);
        }
        [Route("getorder")]
        public FeedbackTypeQuestionMappingDTO getorder([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getorder(data);
        }
        [Route("getquestionwiseoption")]
        public FeedbackTypeQuestionMappingDTO getquestionwiseoption([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getquestionwiseoption(data);
        }

        [Route("onchangequestion")]
        public FeedbackTypeQuestionMappingDTO onchangequestion([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangequestion(data);
        }

        [Route("savedatanew")]
        public FeedbackTypeQuestionMappingDTO savedatanew([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedatanew(data);
        }

        [Route("deactiveoption")]
        public FeedbackTypeQuestionMappingDTO deactiveoption([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactiveoption(data);
        }

        [Route("getordernew")]
        public FeedbackTypeQuestionMappingDTO getordernew([FromBody]FeedbackTypeQuestionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getordernew(data);
        }

        // Type Option Mapping 

        [Route("optiongetdetails")]
        public FeedbackTypeOptionMappingDTO optiongetdetails([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.optiongetdetails(data);
        }
        [Route("optiononchnagetype")]
        public FeedbackTypeOptionMappingDTO optiononchnagetype([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.optiononchnagetype(data);
        }
        [Route("optionsavedata")]
        public FeedbackTypeOptionMappingDTO optionsavedata([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.optionsavedata(data);
        }
        [Route("optionactivedeactive")]
        public FeedbackTypeOptionMappingDTO optionactivedeactive([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.optionactivedeactive(data);
        }
        [Route("optiongetorder")]
        public FeedbackTypeOptionMappingDTO optiongetorder([FromBody]FeedbackTypeOptionMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.optiongetorder(data);
        }
    }
}
