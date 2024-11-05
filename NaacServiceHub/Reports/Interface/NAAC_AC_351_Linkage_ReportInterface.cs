using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public  interface NAAC_AC_351_Linkage_ReportInterface
    {
        NAAC_AC_351_Linkage_ReportDTO loaddata(NAAC_AC_351_Linkage_ReportDTO data);
       Task<NAAC_AC_351_Linkage_ReportDTO> report(NAAC_AC_351_Linkage_ReportDTO data);
    }
}
