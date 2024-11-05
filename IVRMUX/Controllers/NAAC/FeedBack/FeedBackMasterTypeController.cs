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
    public class FeedBackMasterTypeController : Controller
    {
        public FeedBackMasterTypeDelegate _delobj = new FeedBackMasterTypeDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }        
      
        [HttpGet("{id}")]
        public FeedBackMasterTypeDTO getdetails(FeedBackMasterTypeDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        [Route("savedata")]
        public FeedBackMasterTypeDTO savedata([FromBody]FeedBackMasterTypeDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savedata(data);
        }
        [Route("editdata")]
        public FeedBackMasterTypeDTO editdata([FromBody]FeedBackMasterTypeDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.editdata(data);
        }
        [Route("activedeactive")]
        public FeedBackMasterTypeDTO activedeactive([FromBody]FeedBackMasterTypeDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.activedeactive(data);
        }
        [Route("getOrder")]
        public FeedBackMasterTypeDTO getOrder([FromBody]FeedBackMasterTypeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getOrder(data);
        }
        // Feedback Master Questions

        [Route("getquestiondetails")]
        public Feedback_Master_QuestionDTO getquestiondetails([FromBody]Feedback_Master_QuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getquestiondetails(data);
        }
        [Route("questionssavedata")]
        public Feedback_Master_QuestionDTO questionssavedata([FromBody]Feedback_Master_QuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.questionssavedata(data);
        }
        [Route("questionseditdata")]
        public Feedback_Master_QuestionDTO questionseditdata([FromBody]Feedback_Master_QuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.questionseditdata(data);
        }
        [Route("questionsactivedeactive")]
        public Feedback_Master_QuestionDTO questionsactivedeactive([FromBody]Feedback_Master_QuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.questionsactivedeactive(data);
        }
        [Route("questionsgetOrder")]
        public Feedback_Master_QuestionDTO questionsgetOrder([FromBody]Feedback_Master_QuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.questionsgetOrder(data);
        }
        // Feedback Master Options

        [Route("getoptiondetails")]
        public Feedback_Master_OptionDTO getoptiondetails([FromBody]Feedback_Master_OptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getoptiondetails(data);
        }
        [Route("optionsavedata")]
        public Feedback_Master_OptionDTO optionsavedata([FromBody]Feedback_Master_OptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.optionsavedata(data);
        }
        [Route("optioneditdata")]
        public Feedback_Master_OptionDTO optioneditdata([FromBody]Feedback_Master_OptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.optioneditdata(data);
        }
        [Route("optionactivedeactive")]
        public Feedback_Master_OptionDTO optionactivedeactive([FromBody]Feedback_Master_OptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.optionactivedeactive(data);
        }
        [Route("optiongetOrder")]
        public Feedback_Master_OptionDTO optiongetOrder([FromBody]Feedback_Master_OptionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.optiongetOrder(data);
        }
    }
}
