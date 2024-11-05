using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface DifferentlyAbledInterface
    {
        Task<NAAC_AC_719_DifferentlyAbled_DTO> loaddata(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO savedatatab1(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO editTab1(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO deactivYTab1(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO deleteuploadfile(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO getData(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO getDataMC(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO saveMC(NAAC_AC_719_DifferentlyAbled_DTO data);
        NAAC_AC_719_DifferentlyAbled_DTO EditDataMC(NAAC_AC_719_DifferentlyAbled_DTO data);
    }
}
