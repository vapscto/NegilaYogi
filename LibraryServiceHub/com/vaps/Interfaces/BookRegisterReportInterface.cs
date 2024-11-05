using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface BookRegisterReportInterface
    {
        BookRegisterReportDTO getdetails(int id);
        BookRegisterReportDTO get_report(BookRegisterReportDTO id);
        //BarCode
        BookRegisterReportDTO BarCode(BookRegisterReportDTO id);
    }
}
