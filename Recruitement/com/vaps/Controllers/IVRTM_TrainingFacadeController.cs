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
    public class IVRTM_TrainingFacadeController : Controller
    {
        public IVRTM_TrainingInterface _interface;

        public IVRTM_TrainingFacadeController(IVRTM_TrainingInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public IVRTM_TrainingDTO onloaddata([FromBody] IVRTM_TrainingDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public IVRTM_TrainingDTO saverecord([FromBody] IVRTM_TrainingDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactiveY")]
        public IVRTM_TrainingDTO deactiveY([FromBody] IVRTM_TrainingDTO data)
        {
            return _interface.deactiveY(data);
        }
        [Route("Edit")]
        public IVRTM_TrainingDTO Edit([FromBody] IVRTM_TrainingDTO data)
        {

            return _interface.Edit(data);
        }

        [Route("gettrainer")]
        public IVRTM_TrainingDTO gettrainer([FromBody] IVRTM_TrainingDTO data)
        {

            return _interface.gettrainer(data);
        }

        [Route("onloaddataRequest")]
        public IVRTM_TrainingDTO onloaddataRequest([FromBody] IVRTM_TrainingDTO data)
        {
            return _interface.onloaddataRequest(data);
        }
        [Route("saveData")]
        public IVRTM_TrainingDTO saveData([FromBody] IVRTM_TrainingDTO data)
        {

            return _interface.saveData(data);
        }

        [Route("trainerfeedback")]
        public IVRTM_TrainingDTO trainerfeedback([FromBody] IVRTM_TrainingDTO data)
        {

            return _interface.trainerfeedback(data);
        }


        /////////////////////////////IVRM_Training_Assigning/////////////////////////////////////////////////////////////////////////////
        [Route("assignonload")]
        public IVRTM_TrainingDTO assignonload([FromBody] IVRTM_TrainingDTO data)
        {
            return _interface.assignonload(data);
        }
        [Route("EditDetails")]
        public IVRTM_TrainingDTO EditDetails([FromBody] IVRTM_TrainingDTO data)
        {

            return _interface.EditDetails(data);
        }
        [Route("saveassign")]
        public IVRTM_TrainingDTO saveassign([FromBody] IVRTM_TrainingDTO data)
        {

            return _interface.saveassign(data);
        }

        ///===================================report=========================///////////////////////


        [Route("onloaddatareport")]
        public IVRTM_TrainingDTO onloaddatareport([FromBody] IVRTM_TrainingDTO data)
        {
            return _interface.onloaddatareport(data);
        }

        [Route("getreport")]
        public IVRTM_TrainingDTO getreport([FromBody] IVRTM_TrainingDTO data)
        {
            return _interface.getreport(data);
        }


    }
}
