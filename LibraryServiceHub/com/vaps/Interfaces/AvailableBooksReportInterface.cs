using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface AvailableBooksReportInterface
    {
        AvailableBooksReport_DTO getdetails(AvailableBooksReport_DTO id);
        AvailableBooksReport_DTO get_report(AvailableBooksReport_DTO id);

    }
}
