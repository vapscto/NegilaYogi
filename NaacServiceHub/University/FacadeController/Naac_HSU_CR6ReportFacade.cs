using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Reports.Controllers
{
    [Route("api/[controller]")]
    public class Naac_HSU_CR6ReportFacade : Controller
    {

        public Naac_HSU_CR6ReportInterface inter;

        public Naac_HSU_CR6ReportFacade(Naac_HSU_CR6ReportInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public Naac_HSU_CR6Report_DTO loaddata([FromBody] Naac_HSU_CR6Report_DTO data)
        {
            return inter.loaddata(data);
        }

        [Route("HSUEGovernance623Report")]
        public Task<Naac_HSU_CR6Report_DTO> HSUEGovernance623Report([FromBody] Naac_HSU_CR6Report_DTO data)
        {
            return inter.HSUEGovernance623Report(data);
        }

        [Route("HSUFinancialSupport632Report")]
        public Task<Naac_HSU_CR6Report_DTO> HSUFinancialSupport632Report([FromBody] Naac_HSU_CR6Report_DTO data)
        {
            return inter.HSUFinancialSupport632Report(data);
        }


        [Route("HSUDevPrograms633Report")]
        public Task<Naac_HSU_CR6Report_DTO> HSUDevPrograms633Report([FromBody] Naac_HSU_CR6Report_DTO data)
        {
            return inter.HSUDevPrograms633Report(data);
        }

        [Route("HSUDevPrograms634Report")]
        public Task<Naac_HSU_CR6Report_DTO> HSUDevPrograms634Report([FromBody] Naac_HSU_CR6Report_DTO data)
        {
            return inter.HSUDevPrograms634Report(data);
        }

        [Route("HSUGovtFunding642Report")]
        public Task<Naac_HSU_CR6Report_DTO> HSUGovtFunding642Report([FromBody] Naac_HSU_CR6Report_DTO data)
        {
            return inter.HSUGovtFunding642Report(data);
        }

        [Route("HSUQualityAssurance652Report")]
        public Task<Naac_HSU_CR6Report_DTO> HSUQualityAssurance652Report([FromBody] Naac_HSU_CR6Report_DTO data)
        {
            return inter.HSUQualityAssurance652Report(data);
        }



       
    }
}
