using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface SportHouseCommitteeReportInterface
    {
        Task<House_Committe_Report_DTO> showdetailsAsync(House_Committe_Report_DTO data);
        House_Committe_Report_DTO get_House(House_Committe_Report_DTO data);
        House_Committe_Report_DTO Getdetails(House_Committe_Report_DTO data);
    }
}
