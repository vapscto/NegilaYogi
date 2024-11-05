using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface Naac_M_811StudentsEnrolledInProgrammeInterface
    {
        NAAC_811MC_NEET_DTO getdata(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_811M_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_813M_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_819M_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_8110M_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_813D_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_815D_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_816D_report(NAAC_811MC_NEET_DTO data);
        //Task<NAAC_811MC_NEET_DTO> get_817D_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_8111D_report(NAAC_811MC_NEET_DTO data);
        Task<NAAC_811MC_NEET_DTO> get_818N_report(NAAC_811MC_NEET_DTO data);
    }
}
