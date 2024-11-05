using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Interface
{
    public interface ProgramIntroduceInterface
    {
        Task<NAAC_AC_Programs_112_DTO> loaddata(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO savedata(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO editdata(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO deactivY(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO get_Discontinuedflagdata(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO saveContinued(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO savemappingdata(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO deactivYTab2(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO edittab2(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO viewuploadflies(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO deleteuploadfile(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO get_branch(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO get_program(NAAC_AC_Programs_112_DTO data);
        Task<NAAC_AC_Programs_112_DTO> get_Course(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO savemedicaldatawisecomments(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO savefilewisecomments(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO getcomment(NAAC_AC_Programs_112_DTO data);
        NAAC_AC_Programs_112_DTO getfilecomment(NAAC_AC_Programs_112_DTO data);
        //added by sanjeev
       NAAC_AC_Programs_112_DTO saveadvanceAsync(NAAC_AC_Programs_112_DTO data);
    }
}
