using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface NaacMOU352ReportInterface
    {

        NaacMOU352ReportDTO getdata(NaacMOU352ReportDTO data);
       Task<NaacMOU352ReportDTO> get_report(NaacMOU352ReportDTO data);
    }
}
