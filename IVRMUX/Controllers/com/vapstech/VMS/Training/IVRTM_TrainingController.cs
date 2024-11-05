using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.Training
{
    [Route("api/[controller]")]
    public class IVRTM_TrainingController : Controller
    {
        IVRTM_Training_Delegate _del = new IVRTM_Training_Delegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public IVRTM_TrainingDTO onloaddata(int id)
        {
            IVRTM_TrainingDTO data = new IVRTM_TrainingDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.onloaddata(data);
        }


        [HttpPost]
        [Route("saverecord")]
        public IVRTM_TrainingDTO saverecord([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.saverecord(data);
        }

        [Route("deactiveY")]
        public IVRTM_TrainingDTO deactiveY([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.deactiveY(data);
        }
        [Route("Edit")]
        public IVRTM_TrainingDTO Edit([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.Edit(data);
        }
        [Route("gettrainer")]
        public IVRTM_TrainingDTO gettrainer([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.gettrainer(data);
        }



        [Route("onloaddataRequest/{id:int}")]
        public IVRTM_TrainingDTO onloaddataRequest(int id)
        {
            IVRTM_TrainingDTO data = new IVRTM_TrainingDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.onloaddataRequest(data);
        }

        [Route("saveData")]
        public IVRTM_TrainingDTO saveData([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.saveData(data);
        }
        [Route("trainerfeedback")]
        public IVRTM_TrainingDTO trainerfeedback([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.trainerfeedback(data);
        }


        /////////////////////////////IVRM_Training_Assigning/////////////////////////////////////////////////////////////////////////////
        [Route("assignonload/{id:int}")]
        public IVRTM_TrainingDTO assignonload(int id)
        {
            IVRTM_TrainingDTO data = new IVRTM_TrainingDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.assignonload(data);
        }
        [Route("EditDetails")]
        public IVRTM_TrainingDTO EditDetails([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.EditDetails(data);
        }
        [Route("saveassign")]
        public IVRTM_TrainingDTO saveassign([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.saveassign(data);
        }


        //====================================REPORT===================================////////////////////////////

        [Route("onloaddatareport/{id:int}")]
        public IVRTM_TrainingDTO onloaddatareport(int id)
        {
            IVRTM_TrainingDTO data = new IVRTM_TrainingDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.onloaddatareport(data);
        }
        [HttpPost]
        [Route("getreport")]
        public IVRTM_TrainingDTO getreport([FromBody] IVRTM_TrainingDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.getreport(data);
        }



    }
}
