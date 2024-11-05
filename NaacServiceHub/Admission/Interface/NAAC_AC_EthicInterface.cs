using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
  public  interface NAAC_AC_EthicInterface
    {

        NAAC_AC_331_DTO loaddata(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO save(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO deactive(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO getcomment(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO savemedicaldatawisecomments(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO savefilewisecomments(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO getfilecomment(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO EditData(NAAC_AC_331_DTO obj);
        NAAC_AC_331_DTO viewuploadflies(NAAC_AC_331_DTO data);
        NAAC_AC_331_DTO deleteuploadfile(NAAC_AC_331_DTO data);

    }
}
