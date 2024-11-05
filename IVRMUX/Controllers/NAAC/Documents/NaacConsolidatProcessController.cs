using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Documents
{
    [Route("api/[controller]")]
    public class NaacConsolidatProcessController : Controller
    {
        public NaacConsolidatProcessDelegate _delg = new NaacConsolidatProcessDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("onload/{id:int}")]
        public NaacConsolidatProcessDTO onload(int id)
        {
            NaacConsolidatProcessDTO data = new NaacConsolidatProcessDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onload(data);
        }

        [Route("search")]
        public NaacConsolidatProcessDTO search([FromBody]  NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.search(data);
        }
        [Route("getorganizationdata")]
        public NaacConsolidatProcessDTO getorganizationdata([FromBody]  NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getorganizationdata(data);
        }

        [Route("onclickapproval")]
        public NaacConsolidatProcessDTO onclickapproval([FromBody]  NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.onclickapproval(data);
        }


        //AFFLIATED COLLEGE RELATED 
        [Route("getaffliateddata")]
        public NaacConsolidatProcessDTO getaffliateddata([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getaffliateddata(data);
        }

        [Route("savedatawisecomments")]
        public NaacConsolidatProcessDTO savedatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.savedatawisecomments(data);
        }
        [Route("viewdatawisecomments")]
        public NaacConsolidatProcessDTO viewdatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.viewdatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public NaacConsolidatProcessDTO savefilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.savefilewisecomments(data);
        }

        [Route("viewfilewisecomments")]
        public NaacConsolidatProcessDTO viewfilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.viewfilewisecomments(data);
        }
        [Route("approvedata")]
        public NaacConsolidatProcessDTO approvedata([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.approvedata(data);
        }

        [Route("approvedocuments")]
        public NaacConsolidatProcessDTO approvedocuments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.approvedocuments(data);
        }

        [Route("getapproved")]
        public NaacConsolidatProcessDTO getapproved([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getapproved(data);
        }


        //*********** MEDICAL COLLEGE DATA *********************//
        [Route("getmedicalddata")]
        public NaacConsolidatProcessDTO getmedicalddata([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmedicalddata(data);            
        }

        [Route("getmedicalapproveddata")]
        public NaacConsolidatProcessDTO getmedicalapproveddata([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmedicalapproveddata(data);            
        }

        [Route("savemedicaldatawisecomments")]
        public NaacConsolidatProcessDTO savemedicaldatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.savemedicaldatawisecomments(data);            
        }

        [Route("viewmedicaldatawisecomments")]
        public NaacConsolidatProcessDTO viewmedicaldatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.viewmedicaldatawisecomments(data);            
        }
        [Route("approvemedicaldata")]
        public NaacConsolidatProcessDTO approvemedicaldata([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.approvemedicaldata(data);            
        }
        [Route("savemedicalfilewisecomments")]
        public NaacConsolidatProcessDTO savemedicalfilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.savemedicalfilewisecomments(data);            
        }
        [Route("viewmedicalfilewisecomments")]
        public NaacConsolidatProcessDTO viewmedicalfilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.viewmedicalfilewisecomments(data);            
        }
        [Route("approvemedicaldocuments")]
        public NaacConsolidatProcessDTO approvemedicaldocuments([FromBody] NaacConsolidatProcessDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.approvemedicaldocuments(data);            
        }
    }
}
