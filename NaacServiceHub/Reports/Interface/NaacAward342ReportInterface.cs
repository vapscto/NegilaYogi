using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface NaacAward342ReportInterface
    {

        NaacAward342ReportDTO getdata(NaacAward342ReportDTO data);
       Task<NaacAward342ReportDTO> get_report(NaacAward342ReportDTO data);
    }
}
