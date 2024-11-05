using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface LibTransactionReportInterface
    {
        LibTransactionReportDTO getdetails(LibTransactionReportDTO id);
        LibTransactionReportDTO get_report(LibTransactionReportDTO id);
        LibTransactionReportDTO CLGget_report(LibTransactionReportDTO id);
    }
}
