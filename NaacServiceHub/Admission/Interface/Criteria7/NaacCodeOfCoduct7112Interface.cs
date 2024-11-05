using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
  public interface NaacCodeOfCoduct7112Interface
    {
        Task<NAAC_AC_7112_CodeOfCoduct_DTO> loaddata(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO save(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO deactivate(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO EditData(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO viewuploadflies(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO getfilecomment(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO savefilewisecomments(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO getcomment(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO savemedicaldatawisecomments(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO deleteuploadfile(NAAC_AC_7112_CodeOfCoduct_DTO data);
        NAAC_AC_7112_CodeOfCoduct_DTO getData(NAAC_AC_7112_CodeOfCoduct_DTO data);
    }
}
