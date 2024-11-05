using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface HouseInchargeReportInterface
    {
        HouseInchargeReport_DTO get_details(HouseInchargeReport_DTO data);
         Task<HouseInchargeReport_DTO> get_reports(HouseInchargeReport_DTO data);
        HouseInchargeReport_DTO get_house(HouseInchargeReport_DTO data);

    }
}
