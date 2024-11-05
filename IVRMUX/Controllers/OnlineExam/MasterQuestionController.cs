using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.OnlineExam;
using PreadmissionDTOs.com.vaps.OnlineExam;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterQuestionController : Controller
    {
        MasterQuestionDelegate oed = new MasterQuestionDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
       
        [Route("getloaddata/{id:int}")]
        public MasterQuestionDTO getloaddata(int id)
        {
            MasterQuestionDTO data = new MasterQuestionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.getloaddata(data);
        }
        //----------------------------1stTab

        [Route("savedetails")]
        public MasterQuestionDTO savedetails([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.savedetails(data);
        }
        [Route("viewdocumetns")]
        public MasterQuestionDTO viewdocumetns([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.viewdocumetns(data);
        }
        
        [Route("deactiveparticulars")]
        public MasterQuestionDTO deactiveparticulars([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.deactiveparticulars(data);
        }


        //----2nd tab

        [Route("savedataclass")]
        public MasterQuestionDTO savedataclass([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.savedataclass(data);
        }

        [Route("editQuestion")]
        public MasterQuestionDTO editQuestion([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.editQuestion(data);
        }
        //-----------------------------3nd Tab
        [Route("savedetails1")]
        public MasterQuestionDTO savedetails1([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.savedetails1(data);
        }
        [Route("optionChange")]
        public MasterQuestionDTO optionChange([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.optionChange(data);
        }
        [Route("optiondetails")]
        public MasterQuestionDTO optiondetails([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.optiondetails(data);
        }

        [Route("Deletedetails")]
        public MasterQuestionDTO Deletedetails([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return oed.Deletedetails(data);
        }

    }
}
