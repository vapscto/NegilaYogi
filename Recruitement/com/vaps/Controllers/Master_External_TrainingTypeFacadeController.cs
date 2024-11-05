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
    public class Master_External_TrainingTypeFacadeController : Controller
    {
        public Master_External_TrainingTypeInterface _interface;

        public Master_External_TrainingTypeFacadeController(Master_External_TrainingTypeInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public Master_External_TrainingTypeDTO onloaddata([FromBody] Master_External_TrainingTypeDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public Master_External_TrainingTypeDTO saverecord([FromBody] Master_External_TrainingTypeDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactiveY")]
        public Master_External_TrainingTypeDTO deactiveY([FromBody] Master_External_TrainingTypeDTO data)
        {
            return _interface.deactiveY(data);
        }
    }
}
