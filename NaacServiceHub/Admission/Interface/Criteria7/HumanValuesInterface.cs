using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface HumanValuesInterface
    {
        Task<NAAC_AC_7114_HumanValues_DTO> loaddata(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO savedatatab1(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO editTab1(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO deactivYTab1(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO deleteuploadfile(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO getData(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO getcomment(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO getfilecomment(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO savecomments(NAAC_AC_7114_HumanValues_DTO data);
        NAAC_AC_7114_HumanValues_DTO savefilewisecomments(NAAC_AC_7114_HumanValues_DTO data);
    }
}
