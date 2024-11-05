using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface ReligionCasteCategoryReportInterface
    {
        ReligionCasteCategoryReport_DTO loaddata(ReligionCasteCategoryReport_DTO data);
        Task<ReligionCasteCategoryReport_DTO> showdetails(ReligionCasteCategoryReport_DTO data);
    }
}
