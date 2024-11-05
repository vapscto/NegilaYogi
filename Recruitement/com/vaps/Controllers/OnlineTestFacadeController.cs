using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class OnlineTestFacadeController : Controller
    {

        public OnlineTestInterface _interface;

        public OnlineTestFacadeController(OnlineTestInterface adstu)
        {
            _interface = adstu;
        }

        #region  MASTER QUESTION 
        //  MASTER QUESTION 
        [Route("getmasterquestionloaddata")]
        public OnlineTestDTO getmasterquestionloaddata([FromBody] OnlineTestDTO data)
        {
            return _interface.getmasterquestionloaddata(data);
        }

        [Route("getqnspapertype")]
        public OnlineTestDTO getqnspapertype([FromBody] OnlineTestDTO data)
        {
            return _interface.getqnspapertype(data);
        }

      

        [Route("SaveMasterQuestionDetails")]
        public OnlineTestDTO SaveMasterQuestionDetails([FromBody] OnlineTestDTO data)
        {
            return _interface.SaveMasterQuestionDetails(data);
        }

        [Route("EditMasterQuestion")]
        public OnlineTestDTO EditMasterQuestion([FromBody] OnlineTestDTO data)
        {
            return _interface.EditMasterQuestion(data);
        }

        [Route("ViewMasterQuesDoc")]
        public OnlineTestDTO ViewMasterQuesDoc([FromBody] OnlineTestDTO data)
        {
            return _interface.ViewMasterQuesDoc(data);
        }

        [Route("DeactivateActivateDocument")]
        public OnlineTestDTO DeactivateActivateDocument([FromBody] OnlineTestDTO data)
        {
            return _interface.DeactivateActivateDocument(data);
        }

        [Route("ViewMasterQuesOptions")]
        public OnlineTestDTO ViewMasterQuesOptions([FromBody] OnlineTestDTO data)
        {
            return _interface.ViewMasterQuesOptions(data);
        }

        [Route("DeactivateActivateQuesOption")]
        public OnlineTestDTO DeactivateActivateQuesOption([FromBody] OnlineTestDTO data)
        {
            return _interface.DeactivateActivateQuesOption(data);
        }
        [Route("AddMoreOptions")]
        public OnlineTestDTO AddMoreOptions([FromBody] OnlineTestDTO data)
        {
            return _interface.AddMoreOptions(data);
        }
        [Route("DeactivateActivateQues")]
        public OnlineTestDTO DeactivateActivateQues([FromBody] OnlineTestDTO data)
        {
            return _interface.DeactivateActivateQues(data);
        }
        #endregion

       
    }
}
