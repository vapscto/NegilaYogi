using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface LostBookReportInterface
    {

        LostBookReport_DTO getdetails(LostBookReport_DTO id);
        Task<LostBookReport_DTO> get_report(LostBookReport_DTO data);

    }
}
