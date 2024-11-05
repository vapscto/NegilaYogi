using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface GenderEquityInterface
    {
        Task<NAAC_AC_711_GenderEquity_DTO> loaddata(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO savedatatab1(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO editTab1(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO deactivYTab1(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO deleteuploadfile(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO getcomment(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO getfilecomment(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO savecomments(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO savefilewisecomments(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO viewuploadflies(NAAC_AC_711_GenderEquity_DTO data);
        NAAC_AC_711_GenderEquity_DTO getData(NAAC_AC_711_GenderEquity_DTO data);
    }
}
