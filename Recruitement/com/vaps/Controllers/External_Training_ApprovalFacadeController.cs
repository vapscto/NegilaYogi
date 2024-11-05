using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.PettyCash.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class External_Training_ApprovalFacadeController : Controller
    {
        public External_Training_ApprovalInterface _interface;

        public External_Training_ApprovalFacadeController(External_Training_ApprovalInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public External_Training_ApprovalDTO onloaddata([FromBody] External_Training_ApprovalDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("approvalstatus")]
        public External_Training_ApprovalDTO approvalstatus([FromBody] External_Training_ApprovalDTO data)
        {
            return _interface.approvalstatus(data);
        }
        [Route("trainingdetails")]
        public External_Training_ApprovalDTO trainingdetails([FromBody] External_Training_ApprovalDTO data)
        {
            return _interface.trainingdetails(data);
        }

        [Route("deactiveY")]
        public External_Training_ApprovalDTO deactiveY([FromBody] External_Training_ApprovalDTO data)
        {
            return _interface.deactiveY(data);
        }
    }
}
