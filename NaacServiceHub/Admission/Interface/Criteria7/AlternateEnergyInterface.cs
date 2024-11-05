using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface AlternateEnergyInterface
    {
        Task<NAAC_AC_713_AlternateEnergy_DTO> loaddata(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO savedatatab1(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO editTab1(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO deactivYTab1(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO deleteuploadfile(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO getData(NAAC_AC_713_AlternateEnergy_DTO data);

        NAAC_AC_713_AlternateEnergy_DTO getDataMC(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO savedatatabMC(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO editTabMC(NAAC_AC_713_AlternateEnergy_DTO data);
        NAAC_AC_713_AlternateEnergy_DTO deactivateMC(NAAC_AC_713_AlternateEnergy_DTO data);
    }
}
