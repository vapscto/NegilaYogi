using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface Naac_MC_CR6Interface
    {
        Naac_MC_CR6_DTO loaddata(Naac_MC_CR6_DTO data);
        Task<Naac_MC_CR6_DTO> MedFinancialSupport632Report(Naac_MC_CR6_DTO data);
        Task<Naac_MC_CR6_DTO> MedDevPrograms634634Report(Naac_MC_CR6_DTO data);
        Task<Naac_MC_CR6_DTO> MedFunds643Report(Naac_MC_CR6_DTO data);
        Task<Naac_MC_CR6_DTO> MedIQAC652Report(Naac_MC_CR6_DTO data);
        Task<Naac_MC_CR6_DTO> MEDInternalQuality653(Naac_MC_CR6_DTO data);
        Task<Naac_MC_CR6_DTO> MedEGovernance622Report(Naac_MC_CR6_DTO data);
        Task<Naac_MC_CR6_DTO> MedDevPrograms633Report(Naac_MC_CR6_DTO data);
    }
}
