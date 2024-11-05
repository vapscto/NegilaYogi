using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
  public interface NaacIPRInterface
    {
        NAAC_AC_IPR_322_DTO loaddata(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO save(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO deactive(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO getcomment(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO savemedicaldatawisecomments(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO savefilewisecomments(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO getfilecomment(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO EditData(NAAC_AC_IPR_322_DTO obj);
        NAAC_AC_IPR_322_DTO viewuploadflies(NAAC_AC_IPR_322_DTO data);
        NAAC_AC_IPR_322_DTO deleteuploadfile(NAAC_AC_IPR_322_DTO obj);
    }
}
