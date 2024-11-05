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
    public class PC_IndentController : Controller
    {
        PC_IndentDelegate _deg = new PC_IndentDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public PC_IndentDTO onloaddata(int id)
        {
            PC_IndentDTO data = new PC_IndentDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.onloaddata(data);
        }

        [Route("OnChangeInstitution")]
        public PC_IndentDTO OnChangeInstitution([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.OnChangeInstitution(data);
        }

        [Route("onchangedate")]
        public PC_IndentDTO onchangedate([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.onchangedate(data);
        }

        [Route("getrequisitiondetails")]
        public PC_IndentDTO getrequisitiondetails([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.getrequisitiondetails(data);
        }

        [Route("saverecord")]
        public PC_IndentDTO saverecord([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.saverecord(data);
        }

        [Route("EditData")]
        public PC_IndentDTO EditData([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.EditData(data);
        }

        [Route("deactiveY")]
        public PC_IndentDTO deactiveY([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.deactiveY(data);
        }

        [Route("Viewdata")]
        public PC_IndentDTO Viewdata([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.Viewdata(data);
        }

        [Route("deactiveparticulars")]
        public PC_IndentDTO deactiveparticularss([FromBody] PC_IndentDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return _deg.deactiveparticulars(data);
        }
    }
}
