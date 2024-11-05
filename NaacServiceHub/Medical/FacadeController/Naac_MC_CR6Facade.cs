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
    public class Naac_MC_CR6Facade : Controller
    {
        public Naac_MC_CR6Interface inter;

        public Naac_MC_CR6Facade(Naac_MC_CR6Interface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public Naac_MC_CR6_DTO loaddata([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.loaddata(data);
        }

        [Route("MedFinancialSupport632Report")]
        public Task<Naac_MC_CR6_DTO> MedFinancialSupport632Report([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.MedFinancialSupport632Report(data);
        }
        [Route("MedFunds643Report")]
        public Task<Naac_MC_CR6_DTO> MedFunds643Report([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.MedFunds643Report(data);
        }
        
        [Route("MedDevPrograms634634Report")]
        public Task<Naac_MC_CR6_DTO> MedDevPrograms634634Report([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.MedDevPrograms634634Report(data);
        }
        [Route("MedIQAC652Report")]
        public Task<Naac_MC_CR6_DTO> MedIQAC652Report([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.MedIQAC652Report(data);
        }
        [Route("MEDInternalQuality653")]
        public Task<Naac_MC_CR6_DTO> MEDInternalQuality653([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.MEDInternalQuality653(data);
        }
        [Route("MedEGovernance622Report")]
        public Task<Naac_MC_CR6_DTO> MedEGovernance622Report([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.MedEGovernance622Report(data);
        }
        [Route("MedDevPrograms633Report")]
        public Task<Naac_MC_CR6_DTO> MedDevPrograms633Report([FromBody] Naac_MC_CR6_DTO data)
        {
            return inter.MedDevPrograms633Report(data);
        }

    }
}
