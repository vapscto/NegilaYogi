using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
   public interface NAAC711GenderEquityReportInterface
    {
        NAACAC7Report_DTO loaddata(NAACAC7Report_DTO data);
        NAACAC7Report_DTO Report(NAACAC7Report_DTO data);
    }
}
