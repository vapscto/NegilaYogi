using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface BookCirculationReportInterface
    {
        BookCirculationReportDTO getdetails(BookCirculationReportDTO id);
        BookCirculationReportDTO getstuddetails(BookCirculationReportDTO data);
       Task<BookCirculationReportDTO> get_report(BookCirculationReportDTO data);
    }
}
