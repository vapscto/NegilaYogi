using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class OnlineTestCandidateFacadeController : Controller
    {

        public OnlineTestCandidateInterface _interface;

        public OnlineTestCandidateFacadeController(OnlineTestCandidateInterface adstu)
        {
            _interface = adstu;
        }

      

        #region EXAM
        [Route("loadExamdata")]
        public OnlineTestCandidateDTO loadExamdata([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.loadExamdata(data);
        }

        [Route("getQuestion")]
        public OnlineTestCandidateDTO getQuestion([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.getQuestion(data);
        }
        [Route("Saveanswer")]
        public OnlineTestCandidateDTO Saveanswer([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.Saveanswer(data);
        }
        #endregion
    }
}
