using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.PettyCash
{
    [Route("api/[controller]")]
    public class PC_RequisitionController : Controller
    {
        PC_RequisitionDelegate _delg = new PC_RequisitionDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("onloaddata/{id:int}")]
        public PC_RequisitionDTO onloaddata (int id)
        {
            PC_RequisitionDTO data = new PC_RequisitionDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _delg.onloaddata(data);
        }

        [Route("onchangedept")]
        public PC_RequisitionDTO onchangedept([FromBody] PC_RequisitionDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _delg.onchangedept(data);
        }

        [Route("saverecord")]
        public PC_RequisitionDTO saverecord([FromBody] PC_RequisitionDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _delg.saverecord(data);
        }

        [Route("EditData")]
        public PC_RequisitionDTO EditData([FromBody] PC_RequisitionDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _delg.EditData(data);
        }

        [Route("deactiveY")]
        public PC_RequisitionDTO deactiveY([FromBody] PC_RequisitionDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _delg.deactiveY(data);
        }

        [Route("Viewdata")]
        public PC_RequisitionDTO Viewdata([FromBody] PC_RequisitionDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _delg.Viewdata(data);
        }

        [Route("deactiveparticulars")]
        public PC_RequisitionDTO deactiveparticulars([FromBody] PC_RequisitionDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _delg.deactiveparticulars(data);
        }

    }
}
