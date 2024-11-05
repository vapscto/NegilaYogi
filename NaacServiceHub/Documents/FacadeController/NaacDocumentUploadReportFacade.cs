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
    public class NaacDocumentUploadReportFacade : Controller
    {
        public NaacDocumentUploadReportInterface _interface;


        public NaacDocumentUploadReportFacade(NaacDocumentUploadReportInterface _inte)
        {
            _interface = _inte;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
      

        [Route("onload")]
        public Task<NaacDocumentUploadReport_DTO> loaddata([FromBody] NaacDocumentUploadReport_DTO data)
        {
            return _interface.loaddata(data);
        }
    }
}
