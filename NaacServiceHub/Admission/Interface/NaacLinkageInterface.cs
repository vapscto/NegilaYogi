using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
  public interface NaacLinkageInterface
    {

        NAAC_AC_351_Linkage_DTO loaddata(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO save(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO EditData(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO getcomment(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO savefilewisecomments(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO getfilecomment(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO savemedicaldatawisecomments(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO deactive(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO viewuploadflies(NAAC_AC_351_Linkage_DTO data);
        NAAC_AC_351_Linkage_DTO deleteuploadfile(NAAC_AC_351_Linkage_DTO obj);
    }
}
