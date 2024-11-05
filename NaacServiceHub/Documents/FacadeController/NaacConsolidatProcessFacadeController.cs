using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Documents.Interface;
using PreadmissionDTOs.NAAC.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Documents.FacadeController
{
    [Route("api/[controller]")]
    public class NaacConsolidatProcessFacadeController : Controller
    {
        public NaacConsolidatProcessInterface _int;
        // GET: api/<controller>

        public NaacConsolidatProcessFacadeController(NaacConsolidatProcessInterface _intf)
        {
            _int = _intf;
        }

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
        [Route("onload")]
        public NaacConsolidatProcessDTO onload([FromBody]NaacConsolidatProcessDTO data)
        {
            return _int.onload(data);
        }
        [Route("search")]
        public NaacConsolidatProcessDTO search([FromBody]NaacConsolidatProcessDTO data)
        {
            return _int.search(data);
        }
        [Route("getorganizationdata")]
        public NaacConsolidatProcessDTO getorganizationdata([FromBody]NaacConsolidatProcessDTO data)
        {
            return _int.getorganizationdata(data);
        }
        [Route("onclickapproval")]
        public NaacConsolidatProcessDTO onclickapproval([FromBody]NaacConsolidatProcessDTO data)
        {
            return _int.onclickapproval(data);
        }
        //AFFLIATED COLLEGE RELATED 
        [Route("getaffliateddata")]
        public NaacConsolidatProcessDTO getaffliateddata([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.getaffliateddata(data);
        }
        [Route("savedatawisecomments")]
        public NaacConsolidatProcessDTO savedatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.savedatawisecomments(data);
        }
        [Route("viewdatawisecomments")]
        public NaacConsolidatProcessDTO viewdatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.viewdatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NaacConsolidatProcessDTO savefilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.savefilewisecomments(data);
        }
        [Route("viewfilewisecomments")]
        public NaacConsolidatProcessDTO viewfilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.viewfilewisecomments(data);
        }
        [Route("approvedata")]
        public NaacConsolidatProcessDTO approvedata([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.approvedata(data);
        }
        [Route("approvedocuments")]
        public NaacConsolidatProcessDTO approvedocuments([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.approvedocuments(data);
        }
        [Route("getapproved")]
        public NaacConsolidatProcessDTO getapproved([FromBody] NaacConsolidatProcessDTO data)
        { 
            return _int.getapproved(data);
        }

        // ************** MEDICAL COLLEGE DATA ***************** //
        [Route("getmedicalddata")]
        public NaacConsolidatProcessDTO getmedicalddata([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.getmedicalddata(data);
        }
        [Route("getmedicalapproveddata")]
        public NaacConsolidatProcessDTO getmedicalapproveddata([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.getmedicalapproveddata(data);           
        }

        [Route("savemedicaldatawisecomments")]
        public NaacConsolidatProcessDTO savemedicaldatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.savemedicaldatawisecomments(data);
        }

        [Route("viewmedicaldatawisecomments")]
        public NaacConsolidatProcessDTO viewmedicaldatawisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.viewmedicaldatawisecomments(data);
        }
        [Route("approvemedicaldata")]
        public NaacConsolidatProcessDTO approvemedicaldata([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.approvemedicaldata(data);
        }
        [Route("savemedicalfilewisecomments")]
        public NaacConsolidatProcessDTO savemedicalfilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.savemedicalfilewisecomments(data);
        }
        [Route("viewmedicalfilewisecomments")]
        public NaacConsolidatProcessDTO viewmedicalfilewisecomments([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.viewmedicalfilewisecomments(data);
        }
        [Route("approvemedicaldocuments")]
        public NaacConsolidatProcessDTO approvemedicaldocuments([FromBody] NaacConsolidatProcessDTO data)
        {
            return _int.approvemedicaldocuments(data);
        }
    }
}
