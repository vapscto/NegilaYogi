using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.PettyCash.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.PettyCash.Facade
{
    [Route("api/[controller]")]
    public class PC_Indent_ApprovalFacadeController : Controller
    {
        public PC_Indent_ApprovalInterface _interface;

        public PC_Indent_ApprovalFacadeController(PC_Indent_ApprovalInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata")]
        public PC_Indent_ApprovalDTO onloaddata([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("OnChangeInstitution")]
        public PC_Indent_ApprovalDTO OnChangeInstitution([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.OnChangeInstitution(data);
        }

        [Route("onchangedate")]
        public PC_Indent_ApprovalDTO onchangedate([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.onchangedate(data);
        }

        [Route("getindentdetails")]
        public PC_Indent_ApprovalDTO getindentdetails([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.getindentdetails(data);
        }

        [Route("saverecord")]
        public PC_Indent_ApprovalDTO saverecord([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("Viewdata")]
        public PC_Indent_ApprovalDTO Viewdata([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.Viewdata(data);
        }

        // Expenditure
        [Route("ExpenditureLoaddata")]
        public PC_Indent_ApprovalDTO ExpenditureLoaddata([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.ExpenditureLoaddata(data);
        }

        [Route("OnChangeExpenditureInstitution")]
        public PC_Indent_ApprovalDTO OnChangeExpenditureInstitution([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.OnChangeExpenditureInstitution(data);
        }

        [Route("OnChangeExpenditureIndent")]
        public PC_Indent_ApprovalDTO OnChangeExpenditureIndent([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.OnChangeExpenditureIndent(data);
        }

        [Route("OnChangeExpenditureParticular")]
        public PC_Indent_ApprovalDTO OnChangeExpenditureParticular([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.OnChangeExpenditureParticular(data);
        }

        [Route("SaveExpenditure")]
        public PC_Indent_ApprovalDTO SaveExpenditure([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.SaveExpenditure(data);
        }

        [Route("DeleteExpenditure")]
        public PC_Indent_ApprovalDTO DeleteExpenditure([FromBody] PC_Indent_ApprovalDTO data)
        {
            return _interface.DeleteExpenditure(data);
        }
    }
}
