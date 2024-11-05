using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Reports.Controllers
{
    [Route("api/[controller]")]
    public class NaacCriteria4ReportFacade : Controller
    {
        public NaacCriteria4ReportInterface inter;

        public NaacCriteria4ReportFacade(NaacCriteria4ReportInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NaacCriteria4ReportDTO loaddata([FromBody] NaacCriteria4ReportDTO data)
        {
            return inter.loaddata(data);
        }

        [Route("Report")]
        public Task<NaacCriteria4ReportDTO> Report([FromBody] NaacCriteria4ReportDTO data)
        {
            return inter.Report(data);
        }

       
        [Route("ReportCriteria4")]
        public NaacCriteria4ReportDTO ReportCriteria4([FromBody]NaacCriteria4ReportDTO data)
        {
            return inter.ReportCriteria4(data);
        }

        [Route("ExpAcaReport")]
        public Task<NaacCriteria4ReportDTO> ExpAcaReport([FromBody]NaacCriteria4ReportDTO data)
        {
            return inter.ExpAcaReport(data);
        }
    }
}
