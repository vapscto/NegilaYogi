using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Interface
{
   public interface NaacMarksSlabInterface
    {
        NAAC_AC_Criteria_MarksSlab_DTO Getdetails(NAAC_AC_Criteria_MarksSlab_DTO data);
        NAAC_AC_Criteria_MarksSlab_DTO savedata(NAAC_AC_Criteria_MarksSlab_DTO data);
        NAAC_AC_Criteria_MarksSlab_DTO editdata(NAAC_AC_Criteria_MarksSlab_DTO data);
        NAAC_AC_Criteria_MarksSlab_DTO deactive(NAAC_AC_Criteria_MarksSlab_DTO data);
    }
}
