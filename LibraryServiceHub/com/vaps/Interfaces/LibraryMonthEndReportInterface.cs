using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface LibraryMonthEndReportInterface
    {
        LibraryMonthEndReportDTO getdetails(int id);
        LibraryMonthEndReportDTO Savedata(LibraryMonthEndReportDTO data);
   
    }
}
