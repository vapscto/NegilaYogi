using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface StaffParticipationInterface
    {

        Task<NAAC_AC_TParticipation_113_DTO> loaddata(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO savedata(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO editdata(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO deactivY(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO get_designation(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO get_emp(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO viewuploadflies(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO deleteuploadfile(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO savemedicaldatawisecomments(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO savefilewisecomments(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO getcomment(NAAC_AC_TParticipation_113_DTO data);
        NAAC_AC_TParticipation_113_DTO getfilecomment(NAAC_AC_TParticipation_113_DTO data);
        //saveadvance
        NAAC_AC_TParticipation_113_DTO saveadvance(NAAC_AC_TParticipation_113_DTO data);
    }
}
