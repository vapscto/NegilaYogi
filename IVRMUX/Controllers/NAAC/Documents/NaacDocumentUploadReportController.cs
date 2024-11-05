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
    public class NaacDocumentUploadReportController : Controller
    {
        public NaacDocumentUploadReportDelegate _delg = new NaacDocumentUploadReportDelegate();

        [Route("loaddata/{id:int}")]
        public NaacDocumentUploadReport_DTO loaddata(int id)
        {
            NaacDocumentUploadReport_DTO data = new NaacDocumentUploadReport_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }
    }
}
