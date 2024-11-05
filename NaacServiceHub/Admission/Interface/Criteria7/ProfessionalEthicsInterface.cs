using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface ProfessionalEthicsInterface
    {
        Task<NAAC_AC_7115_ProfessionalEthics_DTO> loaddata(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO savedatatab1(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO editTab1(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO deactivYTab1(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO getcomment(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO savemedicaldatawisecomments(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO getfilecomment(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO savefilewisecomments(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO deleteuploadfile(NAAC_AC_7115_ProfessionalEthics_DTO data);
        NAAC_AC_7115_ProfessionalEthics_DTO getData(NAAC_AC_7115_ProfessionalEthics_DTO data);
    }
}
