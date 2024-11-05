using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Documents.Interface;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.Documents;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Documents.FacadeController
{
    [Route("api/[controller]")]
    public class NaacConsolidatedReportFacadeController : Controller
    {
        public NaacConsolidatedReportInterface _Interface;
        public NaacConsolidatedReportFacadeController(NaacConsolidatedReportInterface para)
        {
            _Interface = para;
        }

        [Route("getdata")]
        public NaacDocumentUploadReport_DTO getdata([FromBody]NaacDocumentUploadReport_DTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("get_report")]
        public Task<NaacDocumentUploadReport_DTO> get_report([FromBody]NaacDocumentUploadReport_DTO data)
        {
            return _Interface.get_report(data);
        }




    }
}
