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
    public class Master_External_TrainingCentersFacadeController : Controller
    {
        public Master_External_TrainingCentersInterface _interface;

        public Master_External_TrainingCentersFacadeController(Master_External_TrainingCentersInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public Master_External_TrainingCentersDTO onloaddata([FromBody] Master_External_TrainingCentersDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public Master_External_TrainingCentersDTO saverecord([FromBody] Master_External_TrainingCentersDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactiveY")]
        public Master_External_TrainingCentersDTO deactiveY([FromBody] Master_External_TrainingCentersDTO data)
        {
            return _interface.deactiveY(data);
        }
    }
}
