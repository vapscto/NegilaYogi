using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoeServiceHub.com.vaps.Interfaces
{
  public interface COEReportInterface
    {
       Task<COEReportDTO> getinitialData(int id);
        COEReportDTO getReport(COEReportDTO dto);
        COEReportDTO mothreport(COEReportDTO dto);
    }
}
