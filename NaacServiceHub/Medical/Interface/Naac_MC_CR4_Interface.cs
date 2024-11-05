using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface Naac_MC_CR4_Interface
    {
        Naac_MC_CR4_DTO loaddata(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> Report(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> InOutPatientReport(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> Membership433Report(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> MedExpenditure434Report(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> Econtent436Report(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> PhyAcaFacility451Report(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> ClassSeminarhall441Report(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> BandwidthRangeReport(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> InfrastructureReport(Naac_MC_CR4_DTO data);
        Task<Naac_MC_CR4_DTO> MEDStudentExposed423Report(Naac_MC_CR4_DTO data);
    }
}
