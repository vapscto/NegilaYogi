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
    public class PC_Indent_ApprovalController : Controller
    {
        PC_Indent_ApprovalDelegate _deg = new PC_Indent_ApprovalDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public PC_Indent_ApprovalDTO onloaddata(int id)
        {
            PC_Indent_ApprovalDTO data = new PC_Indent_ApprovalDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.onloaddata(data);
        }

        [Route("OnChangeInstitution")]
        public PC_Indent_ApprovalDTO OnChangeInstitution([FromBody] PC_Indent_ApprovalDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.OnChangeInstitution(data);
        }

        [Route("onchangedate")]
        public PC_Indent_ApprovalDTO onchangedate([FromBody] PC_Indent_ApprovalDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.onchangedate(data);
        }

        [Route("getindentdetails")]
        public PC_Indent_ApprovalDTO getindentdetails([FromBody] PC_Indent_ApprovalDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.getindentdetails(data);
        }

        [Route("saverecord")]
        public PC_Indent_ApprovalDTO saverecord([FromBody] PC_Indent_ApprovalDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.saverecord(data);
        }

        [Route("Viewdata")]
        public PC_Indent_ApprovalDTO Viewdata([FromBody] PC_Indent_ApprovalDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.Viewdata(data);
        }

        // Expenditure

        [Route("ExpenditureLoaddata/{id:int}")]
        public PC_Indent_ApprovalDTO ExpenditureLoaddata(int id)
        {
            PC_Indent_ApprovalDTO data = new PC_Indent_ApprovalDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.ExpenditureLoaddata(data);
        }
        [Route("OnChangeExpenditureInstitution")]
        public PC_Indent_ApprovalDTO OnChangeExpenditureInstitution([FromBody] PC_Indent_ApprovalDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.OnChangeExpenditureInstitution(data);
        }

        [Route("OnChangeExpenditureIndent")]
        public PC_Indent_ApprovalDTO OnChangeExpenditureIndent([FromBody] PC_Indent_ApprovalDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.OnChangeExpenditureIndent(data);
        }

        [Route("OnChangeExpenditureParticular")]
        public PC_Indent_ApprovalDTO OnChangeExpenditureParticular([FromBody] PC_Indent_ApprovalDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.OnChangeExpenditureParticular(data);
        }

        [Route("SaveExpenditure")]
        public PC_Indent_ApprovalDTO SaveExpenditure([FromBody] PC_Indent_ApprovalDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.SaveExpenditure(data);
        }

        [Route("DeleteExpenditure")]
        public PC_Indent_ApprovalDTO DeleteExpenditure([FromBody] PC_Indent_ApprovalDTO data)
        {          
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deg.DeleteExpenditure(data);
        }
    }
}
