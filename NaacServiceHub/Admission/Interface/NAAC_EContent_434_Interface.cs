using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
    public interface NAAC_EContent_434_Interface
    {

        NAAC_AC_434_EContent_DTO loaddata(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO savedata(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO editdata(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO deactiveStudent(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO getcomment(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO getfilecomment(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO savefilewisecomments(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO savemedicaldatawisecomments(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO viewuploadflies(NAAC_AC_434_EContent_DTO data);
        NAAC_AC_434_EContent_DTO deleteuploadfile(NAAC_AC_434_EContent_DTO data);

    }
}
