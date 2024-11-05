using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface.Criteria7;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria7
{
    [Route("api/[controller]")]
    public class NAAC711GenderEquityReportFacade : Controller
    {
        public NAAC711GenderEquityReportInterface inter;

        public NAAC711GenderEquityReportFacade(NAAC711GenderEquityReportInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NAACAC7Report_DTO loaddata([FromBody] NAACAC7Report_DTO data)
        {
            return inter.loaddata(data);
        }
       
        [Route("Report")]
        public NAACAC7Report_DTO Report([FromBody] NAACAC7Report_DTO data)
        {
            return inter.Report(data);
        }
    }
}
