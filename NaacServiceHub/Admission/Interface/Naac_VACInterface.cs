using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
    public interface Naac_VACInterface
    {

        Task<NAAC_AC_VAC_DTO> loaddata(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO savedatatab1(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO getcommentmaster(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO savemedicaldatawisecommentsmaster(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO savefilewisecommentsmaster(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO getfilecommentmaster(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO editTab1(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO deactivYTab1(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO get_student(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO savedatatab2(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO getcomment(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO getfilecomment(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO savefilewisecomments(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO savemedicaldatawisecomments(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO deactivYTabstudent(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO edittab2(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO deactivYTab2(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO viewstudent(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO get_Mappedstudentlist(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO get_Continuedflagdata(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO saveContinued(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO get_Completedflagdata(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO saveCompletedflag(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO viewuploadfliesmain(NAAC_AC_VAC_DTO data);       
        NAAC_AC_VAC_DTO deletemainfile(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO viewuploadfliesstudent(NAAC_AC_VAC_DTO data);
        NAAC_AC_VAC_DTO deletestudentfiles(NAAC_AC_VAC_DTO data);
        //added by sanjeev
        NAAC_AC_VAC_DTO saveadvance(NAAC_AC_VAC_DTO data);
        
    }
}
