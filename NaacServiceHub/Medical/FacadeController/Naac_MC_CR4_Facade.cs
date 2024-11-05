using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Medical.Interface;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Medical.FacadeController
{
    [Route("api/[controller]")]
    public class Naac_MC_CR4_Facade : Controller
    {
        public Naac_MC_CR4_Interface inter;

        public Naac_MC_CR4_Facade(Naac_MC_CR4_Interface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public Naac_MC_CR4_DTO loaddata([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.loaddata(data);
        }

        [Route("Report")]
        public Task<Naac_MC_CR4_DTO> Report([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.Report(data);
        }


        [Route("MEDStudentExposed423Report")]
        public Task<Naac_MC_CR4_DTO> MEDStudentExposed423Report([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.MEDStudentExposed423Report(data);
        }
        [Route("InOutPatientReport")]
        public Task<Naac_MC_CR4_DTO> InOutPatientReport([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.InOutPatientReport(data);
        }

        [Route("Membership433Report")]
        public Task<Naac_MC_CR4_DTO> Membership433Report([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.Membership433Report(data);
        }

        [Route("MedExpenditure434Report")]
        public Task<Naac_MC_CR4_DTO> MedExpenditure434Report([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.MedExpenditure434Report(data);
        }

        [Route("Econtent436Report")]
        public Task<Naac_MC_CR4_DTO> Econtent436Report([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.Econtent436Report(data);
        }
        [Route("PhyAcaFacility451Report")]
        public Task<Naac_MC_CR4_DTO> PhyAcaFacility451Report([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.PhyAcaFacility451Report(data);
        }
        [Route("ClassSeminarhall441Report")]
        public Task<Naac_MC_CR4_DTO> ClassSeminarhall441Report([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.ClassSeminarhall441Report(data);
        }
        [Route("BandwidthRangeReport")]
        public Task<Naac_MC_CR4_DTO> BandwidthRangeReport([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.BandwidthRangeReport(data);
        }
        [Route("InfrastructureReport")]
        public Task<Naac_MC_CR4_DTO> InfrastructureReport([FromBody] Naac_MC_CR4_DTO data)
        {
            return inter.InfrastructureReport(data);
        }
    }
}
