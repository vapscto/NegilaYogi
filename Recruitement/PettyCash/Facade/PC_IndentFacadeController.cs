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
    public class PC_IndentFacadeController : Controller
    {
        public PC_IndentInterface _interface;

        public PC_IndentFacadeController(PC_IndentInterface _inter)
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
        public PC_IndentDTO onloaddata([FromBody] PC_IndentDTO data)
        {
            return _interface.onloaddata(data);
        }
        [Route("OnChangeInstitution")]
        public PC_IndentDTO OnChangeInstitution([FromBody] PC_IndentDTO data)
        {
            return _interface.OnChangeInstitution(data);
        }
        [Route("onchangedate")]
        public PC_IndentDTO onchangedate([FromBody] PC_IndentDTO data)
        {
            return _interface.onchangedate(data);
        }
        [Route("getrequisitiondetails")]
        public PC_IndentDTO getrequisitiondetails([FromBody] PC_IndentDTO data)
        {
            return _interface.getrequisitiondetails(data);
        }
        [Route("saverecord")]
        public PC_IndentDTO saverecord([FromBody] PC_IndentDTO data)
        {
            return _interface.saverecord(data);
        }
        [Route("EditData")]
        public PC_IndentDTO EditData([FromBody] PC_IndentDTO data)
        {
            return _interface.EditData(data);
        }
        [Route("deactiveY")]
        public PC_IndentDTO deactiveY([FromBody] PC_IndentDTO data)
        {
            return _interface.deactiveY(data);
        }
        [Route("Viewdata")]
        public PC_IndentDTO Viewdata([FromBody] PC_IndentDTO data)
        {
            return _interface.Viewdata(data);
        }
        [Route("deactiveparticulars")]
        public PC_IndentDTO deactiveparticulars([FromBody] PC_IndentDTO data)
        {
            return _interface.deactiveparticulars(data);
        }
    }
}
