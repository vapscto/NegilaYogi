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
    public class NaacDocumentUploadFacade : Controller
    {
        public NaacDocumentUploadInterface _interface;


        public NaacDocumentUploadFacade(NaacDocumentUploadInterface _inte)
        {
            _interface = _inte;
        }

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

        [Route("onload")]
        public NaacDocumentUploadDTO onload([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.onload(data);
        }
        [Route("save")]
        public NaacDocumentUploadDTO save([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.save(data);
        }
        [Route("saveapproved")]
        public NaacDocumentUploadDTO saveapproved([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.saveapproved(data);
        }

        [Route("getuploaddoc")]
        public NaacDocumentUploadDTO getuploaddoc([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.getuploaddoc(data);
        }
        [Route("getcomments")]
        public NaacDocumentUploadDTO getcomments([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.getcomments(data);
        }
        [Route("savecomments")]
        public NaacDocumentUploadDTO savecomments([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.savecomments(data);
        }
        [Route("viewcomments")]
        public NaacDocumentUploadDTO viewcomments([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.viewcomments(data);
        }
        [Route("savegeneralcommetns")]
        public NaacDocumentUploadDTO savegeneralcommetns([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.savegeneralcommetns(data);
        }
        [Route("savecommentscons")]
        public NaacDocumentUploadDTO savecommentscons([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.savecommentscons(data);
        }
        [Route("savehyperlinks")]
        public NaacDocumentUploadDTO savehyperlinks([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.savehyperlinks(data);
        }
        [Route("viewaddedhyperlink")]
        public NaacDocumentUploadDTO viewaddedhyperlink([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.viewaddedhyperlink(data);
        }
        [Route("deletehyperlink")]
        public NaacDocumentUploadDTO deletehyperlink([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.deletehyperlink(data);
        }
        [Route("deleteuploadfile")]
        public NaacDocumentUploadDTO deleteuploadfile([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.deleteuploadfile(data);
        }
        [Route("saveCGPA")]
        public NaacDocumentUploadDTO saveCGPA([FromBody] NaacDocumentUploadDTO data)
        {
            return _interface.saveCGPA(data);
        }     
        
    }
}
