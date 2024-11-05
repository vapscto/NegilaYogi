using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.TT;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public interface ClasswiseConsolidatedReportInterface
    {
     
        TT_ClasswiseConsolidatedReportDTO getalldetails(int id);
        TT_ClasswiseConsolidatedReportDTO Report(TT_ClasswiseConsolidatedReportDTO data);
        TT_ClasswiseConsolidatedReportDTO getabreport(TT_ClasswiseConsolidatedReportDTO data);
        
    }
}
