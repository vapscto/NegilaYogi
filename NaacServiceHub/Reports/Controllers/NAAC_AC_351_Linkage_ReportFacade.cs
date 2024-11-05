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
    public class NAAC_AC_351_Linkage_ReportFacade : Controller
    {
        public NAAC_AC_351_Linkage_ReportInterface _Interface;

        // GET: api/<controller>
        public NAAC_AC_351_Linkage_ReportFacade(NAAC_AC_351_Linkage_ReportInterface para)
        {
            _Interface = para;
        }
        [Route("loaddata")]
       public NAAC_AC_351_Linkage_ReportDTO loaddata([FromBody] NAAC_AC_351_Linkage_ReportDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("report")]
        public Task<NAAC_AC_351_Linkage_ReportDTO> report([FromBody]NAAC_AC_351_Linkage_ReportDTO data)
        {
            return _Interface.report(data);
        }
    }
}
