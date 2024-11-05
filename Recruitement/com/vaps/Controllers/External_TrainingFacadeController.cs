using System;
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
    public class External_TrainingFacadeController : Controller
    {
        public External_TrainingInterface _interface;

        public External_TrainingFacadeController(External_TrainingInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public External_TrainingDTO onloaddata([FromBody] External_TrainingDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public External_TrainingDTO saverecord([FromBody] External_TrainingDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactiveY")]
        public External_TrainingDTO deactiveY([FromBody] External_TrainingDTO data)
        {
            return _interface.deactiveY(data);
        }
        [Route("Edit")]
        public External_TrainingDTO Edit([FromBody] External_TrainingDTO data)
        {
            return _interface.Edit(data);
        }
    }
}
