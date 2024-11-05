using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAAC_AC_AwardsInterface
    {
        NAAC_AC_Awards_342_DTO loaddata(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO save(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO savefilewisecomments(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO getcomment(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO savemedicaldatawisecomments(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO getfilecomment(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO deactive(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO EditData(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO viewuploadflies(NAAC_AC_Awards_342_DTO data);
        NAAC_AC_Awards_342_DTO deleteuploadfile(NAAC_AC_Awards_342_DTO obj);
    }
}
