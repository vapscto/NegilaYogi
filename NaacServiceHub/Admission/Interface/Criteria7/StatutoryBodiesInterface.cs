using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface StatutoryBodiesInterface
    {
        Task<NAAC_AC_7116_StatutoryBodies_DTO> loaddata(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO savedatatab1(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO editTab1(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO getfilecomment(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO getcomment(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO savemedicaldatawisecomments(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO savefilewisecomments(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO deactivYTab1(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO deleteuploadfile(NAAC_AC_7116_StatutoryBodies_DTO data);
        NAAC_AC_7116_StatutoryBodies_DTO getData(NAAC_AC_7116_StatutoryBodies_DTO data);
    }
}
