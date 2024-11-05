using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
   public interface CLGDeputationReportInterface
    {
        CLGDeputationReportDTO getdata(CLGDeputationReportDTO data);
        CLGDeputationReportDTO getreport(CLGDeputationReportDTO data);
    }
}
