using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface UniversalValuesInterface
    {
        Task<NAAC_AC_7117_UniversalValues_DTO> loaddata(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO savedatatab1(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO editTab1(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO savemedicaldatawisecomments(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO getfilecomment(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO savefilewisecomments(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO getcomment(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO deactivYTab1(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO deleteuploadfile(NAAC_AC_7117_UniversalValues_DTO data);
        NAAC_AC_7117_UniversalValues_DTO getData(NAAC_AC_7117_UniversalValues_DTO data);
    }
}
