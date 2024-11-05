using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAAC_AC_653_IQACInterface
    {
        NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO save(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO deactiveStudent(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO EditData(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO viewuploadflies(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO deleteuploadfile(NAAC_Criteria_6_DTO data);


        NAAC_Criteria_6_DTO savemedicaldatawisecomments(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO savefilewisecomments(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO getcomment(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO getfilecomment(NAAC_Criteria_6_DTO data);

    }
}
