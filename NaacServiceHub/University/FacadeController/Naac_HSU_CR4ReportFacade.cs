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
    public class Naac_HSU_CR4ReportFacade : Controller
    {

        public Naac_HSU_CR4ReportInterface inter;

        public Naac_HSU_CR4ReportFacade(Naac_HSU_CR4ReportInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public Naac_HSU_CR4Report_DTO loaddata([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.loaddata(data);
        }

        [Route("HSUclinicalinfra423Report")]
        public Task<Naac_HSU_CR4Report_DTO> HSUclinicalinfra423Report([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSUclinicalinfra423Report(data);
        }

        [Route("ClinicalLabReport")]
        public Task<Naac_HSU_CR4Report_DTO> ClinicalLabReport([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.ClinicalLabReport(data);
        }

        [Route("HSUMembership433Report")]
        public Task<Naac_HSU_CR4Report_DTO> HSUMembership433Report([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSUMembership433Report(data);
        }


        [Route("HSU_ExpenditureBook434Report")]
        public Task<Naac_HSU_CR4Report_DTO> HSU_ExpenditureBook434Report([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSU_ExpenditureBook434Report(data);
        }



        [Route("HSUEcontent435Report")]
        public Task<Naac_HSU_CR4Report_DTO> HSUEcontent435Report([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSUEcontent435Report(data);
        }
        [Route("HSUClassSeminarhall441Report")]
        public Task<Naac_HSU_CR4Report_DTO> HSUClassSeminarhall441Report([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSUClassSeminarhall441Report(data);
        }
        [Route("HSUBandwidthRangeReport")]
        public Task<Naac_HSU_CR4Report_DTO> HSUBandwidthRangeReport([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSUBandwidthRangeReport(data);
        }
        [Route("HSU_PhyAcaFacility451Report")]
        public Task<Naac_HSU_CR4Report_DTO> HSU_PhyAcaFacility451Report([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSU_PhyAcaFacility451Report(data);
        }
        [Route("HSU_Infrastructureexpenditure414Report")]
        public Task<Naac_HSU_CR4Report_DTO> HSU_Infrastructureexpenditure414Report([FromBody] Naac_HSU_CR4Report_DTO data)
        {
            return inter.HSU_Infrastructureexpenditure414Report(data);
        }
    }
}
