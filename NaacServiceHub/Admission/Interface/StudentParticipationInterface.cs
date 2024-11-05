using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface StudentParticipationInterface
    {
        Task<NAAC_AC_SParticipation_123_Students_DTO> loaddata(NAAC_AC_SParticipation_123_Students_DTO data);
        NAAC_AC_SParticipation_123_Students_DTO savedata(NAAC_AC_SParticipation_123_Students_DTO data);
        Task<NAAC_AC_SParticipation_123_Students_DTO> editdata(NAAC_AC_SParticipation_123_Students_DTO data);
        Task<NAAC_AC_SParticipation_123_Students_DTO> deactivY(NAAC_AC_SParticipation_123_Students_DTO data);       
        NAAC_AC_SParticipation_123_Students_DTO get_branch(NAAC_AC_SParticipation_123_Students_DTO data);
        NAAC_AC_SParticipation_123_Students_DTO get_student(NAAC_AC_SParticipation_123_Students_DTO data);
        Task<NAAC_AC_SParticipation_123_Students_DTO> get_MappedStudentList(NAAC_AC_SParticipation_123_Students_DTO data);
        NAAC_AC_SParticipation_123_Students_DTO viewuploadflies(NAAC_AC_SParticipation_123_Students_DTO data);
        NAAC_AC_SParticipation_123_Students_DTO deleteuploadfile(NAAC_AC_SParticipation_123_Students_DTO data);
        Task<NAAC_AC_SParticipation_123_Students_DTO> get_coursebrnch(NAAC_AC_SParticipation_123_Students_DTO data);

        NAAC_AC_SParticipation_123_Students_DTO savemedicaldatawisecomments(NAAC_AC_SParticipation_123_Students_DTO data);
        NAAC_AC_SParticipation_123_Students_DTO savefilewisecomments(NAAC_AC_SParticipation_123_Students_DTO data);
        NAAC_AC_SParticipation_123_Students_DTO getcomment(NAAC_AC_SParticipation_123_Students_DTO data);
        NAAC_AC_SParticipation_123_Students_DTO getfilecomment(NAAC_AC_SParticipation_123_Students_DTO data);

    }
}
