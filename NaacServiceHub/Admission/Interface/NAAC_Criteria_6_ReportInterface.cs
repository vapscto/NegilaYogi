using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAAC_Criteria_6_ReportInterface
    {
        NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data);
        NAAC_Criteria_6_DTO get_report(NAAC_Criteria_6_DTO data);
       
        
    }
}
