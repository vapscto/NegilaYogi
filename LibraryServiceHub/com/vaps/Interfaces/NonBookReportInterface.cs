using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface NonBookReportInterface
    {

        NonBookReport_DTO getdetails(NonBookReport_DTO data);
        Task<NonBookReport_DTO> get_report(NonBookReport_DTO data);
    }
}
