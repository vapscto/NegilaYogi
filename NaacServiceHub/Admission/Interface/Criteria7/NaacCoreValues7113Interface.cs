using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
   public interface NaacCoreValues7113Interface
    {

        Task<NAAC_AC_7113_CoreValues_DTO> loaddata(NAAC_AC_7113_CoreValues_DTO data);
        NAAC_AC_7113_CoreValues_DTO save(NAAC_AC_7113_CoreValues_DTO data);
        NAAC_AC_7113_CoreValues_DTO deactivate(NAAC_AC_7113_CoreValues_DTO data);
        NAAC_AC_7113_CoreValues_DTO EditData(NAAC_AC_7113_CoreValues_DTO data);
        NAAC_AC_7113_CoreValues_DTO viewuploadflies(NAAC_AC_7113_CoreValues_DTO data);
       NAAC_AC_7113_CoreValues_DTO getfilecomment(NAAC_AC_7113_CoreValues_DTO data);
        NAAC_AC_7113_CoreValues_DTO savefilewisecomments(NAAC_AC_7113_CoreValues_DTO data);
       NAAC_AC_7113_CoreValues_DTO getcomment(NAAC_AC_7113_CoreValues_DTO data);
       NAAC_AC_7113_CoreValues_DTO savemedicaldatawisecomments(NAAC_AC_7113_CoreValues_DTO data);
        NAAC_AC_7113_CoreValues_DTO deleteuploadfile(NAAC_AC_7113_CoreValues_DTO data);
        NAAC_AC_7113_CoreValues_DTO getData(NAAC_AC_7113_CoreValues_DTO data);
    }
}
